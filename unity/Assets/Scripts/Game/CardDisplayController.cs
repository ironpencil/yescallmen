using UnityEngine;
using System.Collections.Generic;

public class CardDisplayController : MonoBehaviour {

    public static CardDisplayController cardDisplayController;
    public UIGrid displayGrid;

    public bool canSelectCards = true;

    public float defaultDisplayTime = 3.0f;

    //public UIScrollView handScrollView;

    public List<DisplayedCard> displayedCards = new List<DisplayedCard>();
    private List<DisplayedCard> removeCards = new List<DisplayedCard>();

	// Use this for initialization
	void Start () {
        cardDisplayController = this;
	}
	
	// Update is called once per frame
	void Update () {

        foreach (DisplayedCard card in removeCards)
        {
            displayedCards.Remove(card);
        }

        removeCards.Clear();

        foreach (DisplayedCard card in displayedCards)
        {
            card.Update();
            if (card.timeUp && card.destinationZone != CardContainer.CardZone.None)
            {
                MoveCardToDestination(card);
            }
        }        

        if (displayedCards.Count == 0 && collider.enabled)
        {
            collider.enabled = false;
        }

	}

    public void DisplayCard(GameObject cardObject, CardContainer.CardZone destinationZone)
    {
        DisplayCard(cardObject, destinationZone, defaultDisplayTime);
    }

    public void DisplayCard(GameObject cardObject, CardContainer.CardZone destinationZone, float displayTime)
    {
        if (cardObject == null) return;

        if (!canSelectCards)
        {
            //enable this panel's collider so you can't interact with the cards
            collider.enabled = true;
        }
        //TweenScale.Begin(cardObject, 0.5f, new Vector3(1.5f, 1.5f, 0.0f)).PlayForward(); 

        CardController displayedCardController = cardObject.GetComponent<CardController>();
        if (displayedCardController) { displayedCardController.UpdateCurrentZone(); }

        cardObject.transform.parent = displayGrid.transform;
        NGUITools.BringForward(cardObject);
        displayGrid.repositionNow = true;

        NGUITools.MarkParentAsChanged(cardObject);

        //Debug.Log("Card displayed: " + cardObject.ToString() + ". Destination = " + destinationZone.ToString() + ". Time = " + displayTime);
        displayedCards.Add(new DisplayedCard(cardObject, destinationZone, displayTime, true));
        //Debug.Log("Displayed Cards: " + displayedCards.Count);
    }

    private void MoveCardToDestination(DisplayedCard displayedCard)
    {
        GameObject destinationObject = FindDestinationObject(displayedCard.destinationZone);

        //Debug.Log("Card being moved from " + displayedCard.cardObject.gameObject.ToString() +
        //    " to " + destinationObject.ToString());

        ReparentCard(displayedCard.cardObject, destinationObject);

        removeCards.Add(displayedCard);

        switch (displayedCard.destinationZone)
        {
            case CardContainer.CardZone.Hand:
                break;
            case CardContainer.CardZone.Play:
                break;
            case CardContainer.CardZone.Discard:
                DeckManager.deckManager.AddCardToDiscard(DeckManager.GetCardDefinition(displayedCard.cardObject));
                break;
            default:
                break;
        }
    }

    public void MoveCardToDestination(GameObject cardObject, CardContainer.CardZone destination)
    {
        DisplayedCard displayedCard = null;

        foreach (DisplayedCard card in displayedCards)
        {
            if (card.cardObject == cardObject)
            {
                displayedCard = card;
                break;
            }
        }

        if (displayedCard == null) { return; }

        displayedCard.destinationZone = destination;

        MoveCardToDestination(displayedCard);       
    }

    private GameObject FindDestinationObject(CardContainer.CardZone destination)
    {
        GameObject destinationObject = null;

        switch (destination)
        {
            case CardContainer.CardZone.Hand:
                destinationObject = CardZoneManager.cardZoneManager.handContainer;
                break;
            case CardContainer.CardZone.Play:
                destinationObject = CardZoneManager.cardZoneManager.playContainer;
                break;
            case CardContainer.CardZone.Discard:
                destinationObject = CardZoneManager.cardZoneManager.discardContainer;
                break;
            default:
                break;
        }

        return destinationObject;
    }

    private void ReparentCard(GameObject cardObject, GameObject newParent)
    {
        cardObject.transform.parent = newParent.transform;

        UIGrid destinationGrid = newParent.GetComponent<UIGrid>();

        if (destinationGrid != null)
        {
            destinationGrid.repositionNow = true;
        }

        displayGrid.repositionNow = true;

        //have to enable/re-enable the scroll view to get it to update to the new parent
        UIDragScrollView dragScrollView = cardObject.GetComponent<UIDragScrollView>();

        if (dragScrollView != null)
        {
            //dragScrollView.scrollView = handScrollView;
            dragScrollView.enabled = false;
            dragScrollView.enabled = true;
        }

        NGUITools.MarkParentAsChanged(cardObject);
    }
}

public class DisplayedCard
{
    public GameObject cardObject = null;
    public float displayTime = 0.0f;
    public bool timeUp = false;
    
    private float currentTime = 0.0f;

    private bool countTime = false;

    public CardContainer.CardZone destinationZone = CardContainer.CardZone.None;

    public DisplayedCard(GameObject cardObject, CardContainer.CardZone destinationZone, float displayTime, bool start)
    {
        this.cardObject = cardObject;
        this.destinationZone = destinationZone;
        this.displayTime = displayTime;

        if (start) { StartTimer(); }
    }

    public void ResetTimer()
    {
        currentTime = 0.0f;
    }

    public void StartTimer()
    {
        StartTimer(true);
    }

    public void StartTimer(bool reset)
    {
        if (reset) { ResetTimer(); }

        countTime = true;
        timeUp = false;
    }

    public void StopTimer()
    {
        countTime = false;
    }

    public void Update()
    {
        if (countTime)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= displayTime)
            {
                timeUp = true;
                StopTimer();
            }
        }
    }
}
