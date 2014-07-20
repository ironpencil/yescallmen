using UnityEngine;
using System.Collections.Generic;

public class CardDisplayController : MonoBehaviour {

    public static CardDisplayController cardDisplayController;
    public UIGrid displayGrid;

    public enum DisplayMode
    {
        DisplayOnly,
        Selection
    }

    private DisplayMode displayMode = DisplayMode.DisplayOnly;
    public DisplayMode CurrentDisplayMode
    {
        get { return displayMode; }
        set
        {
            displayMode = value;
        }
    }

    public enum SelectionResult
    {
        None,
        OK,
        Cancel
    }

    public SelectionResult result = SelectionResult.None;

    public UIButton OKButton;
    public UILabel OKButtonLabel;
    public UIButton CancelButton;
    public UILabel CancelButtonLabel;

    public int numCardsToSelect = 0;
    public int numCardsSelected = 0;

    public bool showOKButton = false;
    public bool showCancelButton = false;

    public CardContainer.CardZone selectionDestination = CardContainer.CardZone.Discard;

    public float defaultDisplayTime = 3.0f;

    //public UIScrollView handScrollView;

    public List<DisplayedCard> displayedCards = new List<DisplayedCard>();
    protected List<DisplayedCard> removeCards = new List<DisplayedCard>();

	// Use this for initialization
	public virtual void Start () {
        cardDisplayController = this;
	}
	
	// Update is called once per frame
	public virtual void Update () {

        foreach (DisplayedCard card in removeCards)
        {
            displayedCards.Remove(card);
        }

        removeCards.Clear();

        if (CurrentDisplayMode == DisplayMode.DisplayOnly)
        {
            foreach (DisplayedCard card in displayedCards)
            {
                card.Update();
                //if (card.timeUp && card.destinationZone != CardContainer.CardZone.None)
                if (card.timeUp)
                {
                    MoveCardToDestination(card);
                }
            }
        }
        else if (CurrentDisplayMode == DisplayMode.Selection)
        {
            if (showOKButton != OKButton.gameObject.activeSelf)
            {
                OKButton.gameObject.SetActive(showOKButton);
            }

            if (showCancelButton != CancelButton.gameObject.activeSelf)
            {
                CancelButton.gameObject.SetActive(showCancelButton);
            }

            if (numCardsSelected >= numCardsToSelect)
            {
                //we are done selecting because we have selected the maximum number of cards
                result = SelectionResult.None;
                ClearSelectionChoices();
            }
        }

        if (displayedCards.Count == 0 && collider.enabled)
        {
            collider.enabled = false;
        }

	}

    public void ClearSelectionChoices()
    {
        //we just turn Display back on, the remaining cards will be moved to CardZone.None, which destroys them
        CurrentDisplayMode = DisplayMode.DisplayOnly;
        numCardsToSelect = 0;
        numCardsSelected = 0;

        showOKButton = false;
        showCancelButton = false;

        OKButtonLabel.text = "OK";
        CancelButtonLabel.text = "Cancel";

        OKButton.gameObject.SetActive(false);
        CancelButton.gameObject.SetActive(false);
    }

    public void DoOK()
    {
        result = SelectionResult.OK;
        ClearSelectionChoices();
    }

    public void DoCancel()
    {
        result = SelectionResult.Cancel;
        ClearSelectionChoices();
    }

    public void DisplayCard(GameObject cardObject, CardContainer.CardZone destinationZone)
    {
        DisplayCard(cardObject, destinationZone, defaultDisplayTime);
    }

    public void DisplayCard(GameObject cardObject, CardContainer.CardZone destinationZone, float displayTime)
    {
        if (cardObject == null) return;

        //TweenScale.Begin(cardObject, 0.5f, new Vector3(1.5f, 1.5f, 0.0f)).PlayForward(); 

        cardObject.transform.parent = displayGrid.transform;

        CardController displayedCardController = cardObject.GetComponent<CardController>();
        if (displayedCardController) { displayedCardController.UpdateCurrentZone(); }

        //cardObject.transform.parent = displayGrid.transform;
        NGUITools.BringForward(cardObject);
        displayGrid.repositionNow = true;

        NGUITools.MarkParentAsChanged(cardObject);

        //Debug.Log("Card displayed: " + cardObject.ToString() + ". Destination = " + destinationZone.ToString() + ". Time = " + displayTime);
        displayedCards.Add(new DisplayedCard(cardObject, destinationZone, displayTime, true));
        //Debug.Log("Displayed Cards: " + displayedCards.Count);
    }

    protected void MoveCardToDestination(DisplayedCard displayedCard)
    {
        if (displayedCard.destinationZone != CardContainer.CardZone.None)
        {
            //GameObject destinationObject = FindDestinationObject(displayedCard.destinationZone);

            //Debug.Log("Card being moved from " + displayedCard.cardObject.gameObject.ToString() +
            //    " to " + destinationObject.ToString());

            CardZoneManager.cardZoneManager.MoveCardToZone(displayedCard.cardObject, displayedCard.destinationZone);

            //ReparentCard(displayedCard.cardObject, destinationObject);
        }

        removeCards.Add(displayedCard);

        switch (displayedCard.destinationZone)
        {
            case CardContainer.CardZone.None:
                Destroy(displayedCard.cardObject);
                break;
            case CardContainer.CardZone.Hand:
                break;
            case CardContainer.CardZone.Play:
                break;
            case CardContainer.CardZone.Discard:
                NGUITools.BringForward(displayedCard.cardObject);
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

    public void SelectCard(GameObject cardObject)
    {
        foreach (DisplayedCard card in displayedCards)
        {
            if (card.cardObject == cardObject)
            {
                Debug.Log("Selecting card: " + cardObject.ToString());
                numCardsSelected++;
                removeCards.Add(card);
            }
        }        
    }

    protected GameObject FindDestinationObject(CardContainer.CardZone destination)
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

    protected void ReparentCard(GameObject cardObject, GameObject newParent)
    {
        cardObject.transform.parent = newParent.transform;

        CardController cardController = cardObject.GetComponent<CardController>();
        if (cardController != null) { cardController.UpdateCurrentZone(); }

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
