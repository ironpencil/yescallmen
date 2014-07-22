using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardZoneManager : MonoBehaviour {

    public static CardZoneManager cardZoneManager;

    public GameObject handContainer;
    public GameObject playContainer;
    public GameObject discardContainer;
    public GameObject displayContainer;
    public GameObject selectionContainer;

	// Use this for initialization
	void Start () {
        cardZoneManager = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public List<CardController> GetCardsInZone(CardContainer.CardZone zone)
    {
        GameObject targetObject = GetZoneContainer(zone);

        List<CardController> cardList = new List<CardController>();

        if (targetObject != null)
        {
            foreach (CardController card in targetObject.transform.GetComponentsInChildren<CardController>())
            {
                if (card.CurrentZone == zone)
                {
                    cardList.Add(card);
                }
            }
        }

        return cardList;
    }

    private GameObject GetZoneContainer(CardContainer.CardZone zone)
    {
        GameObject targetObject = null;

        switch (zone)
        {
            case CardContainer.CardZone.Hand:
                targetObject = handContainer;
                break;
            case CardContainer.CardZone.Play:
                targetObject = playContainer;
                break;
            case CardContainer.CardZone.Discard:
                targetObject = discardContainer;
                break;
            case CardContainer.CardZone.Display:
                targetObject = displayContainer;
                break;
            case CardContainer.CardZone.Selection:
                targetObject = selectionContainer;
                break;
            default:
                break;
        }
        return targetObject;
    }

    public static CardContainer.CardZone FindObjectZone(GameObject go)
    {
        CardContainer cardContainer = go.GetComponentInParent<CardContainer>();

        CardContainer.CardZone returnZone = CardContainer.CardZone.None;

        if (cardContainer != null)
        {
            returnZone = cardContainer.cardZone;
        }

        return returnZone;

    }

    public bool CanDragCardToZone(DragDropCard card, GameObject surface)
    {
        bool canDragCard = false;

        CardContainer.CardZone destinationZone = CardZoneManager.FindObjectZone(surface);

        Debug.Log("Surface zone = " + destinationZone.ToString());

        switch (destinationZone)
        {
            case CardContainer.CardZone.None:
                break;
            case CardContainer.CardZone.Hand:
                canDragCard = CanDragToHand(card);
                break;
            case CardContainer.CardZone.Play:
                canDragCard = CanDragToPlay(card);
                break;
            case CardContainer.CardZone.Attached:
                break;
            case CardContainer.CardZone.Discard:
                canDragCard = CanDragToDiscard(card);
                break;
            case CardContainer.CardZone.Display:
                break;
            case CardContainer.CardZone.Selection:
                canDragCard = CanDragToSlot(card, surface);
                break;
            default:
                break;
        }

        return canDragCard;
    }

    private bool CanDragToSlot(DragDropCard card, GameObject slot)
    {
        UIGrid cardSlot = slot.GetComponentInChildren<UIGrid>();

        //if we already have a card in this slot, return false
        if (cardSlot.GetChildList().size > 0) { return false; }

        //if no card in slot, then check to make sure this card matches whatever restrictions are currently in place
        if (CardSelectionController.cardSelectionController.canSlotCard != null)
        {
            bool canSlotCard = CardSelectionController.cardSelectionController.canSlotCard(card.cardController);
            if (!canSlotCard) { return false; }
        }

        //don't allow to drag from a zone that isn't valid
        if (card.cardController.CurrentZone != CardSelectionController.cardSelectionController.SourceCardZone)
        {
            return false;
        }

        //TODO

        return true;
    }

    private bool CanDragToHand(DragDropCard card)
    {
        bool returnValue = false;
        //if card is being dragged from Selection slot, and the selection source is not Hand, don't allow
        if (card.cardController.CurrentZone == CardContainer.CardZone.Selection)
        {
            if (CardSelectionController.cardSelectionController.SourceCardZone == CardContainer.CardZone.Hand)
            {
                returnValue = true;
            }
            else
            {
                return false;
            }
        }

        return returnValue;
    }

    private bool CanDragToDiscard(DragDropCard card)
    {

        if (card.cardController.CurrentZone == CardContainer.CardZone.Display &&
            CardDisplayController.cardDisplayController.CurrentDisplayMode == CardDisplayController.DisplayMode.Selection &&
            CardDisplayController.cardDisplayController.selectionDestination == CardContainer.CardZone.Discard)
        {
            CardDisplayController.cardDisplayController.SelectCard(card.gameObject);
            return true;
        }

        return false;
    }

    private bool CanDragToDeck(DragDropCard card)
    {
        return false;
    }

    private bool CanDragToPlay(DragDropCard card)
    {
        if (card.cardController.CurrentZone != CardContainer.CardZone.Hand)
        {
            return false;
        }

        if (!TurnManager.turnManager.CanPlayCards()) { return false; }

        return RulesManager.rulesManager.CanPlayCard(card.gameObject);
    }

    private bool CanDragToDisplay(DragDropCard card)
    {
        return false;
    }

    public void MoveCardToZone(GameObject cardObject, CardContainer.CardZone newZone)
    {        

        UIGrid currentGrid = cardObject.transform.parent.gameObject.GetComponent<UIGrid>();

        CardController cardController = cardObject.GetComponent<CardController>();

        if (newZone != CardContainer.CardZone.None)
        {

            GameObject newParent = GetZoneContainer(newZone);

            cardObject.transform.parent = newParent.transform;            

            cardController.UpdateCurrentZone();

            UIGrid destinationGrid = newParent.GetComponent<UIGrid>();

            if (destinationGrid != null)
            {
                destinationGrid.repositionNow = true;
            }
        }

        if (currentGrid != null)
        {
            currentGrid.repositionNow = true;
        }

        //have to enable/re-enable the scroll view to get it to update to the new parent
        UIDragScrollView dragScrollView = cardObject.GetComponent<UIDragScrollView>();

        if (dragScrollView != null)
        {
            //dragScrollView.scrollView = handScrollView;
            dragScrollView.enabled = false;
            dragScrollView.enabled = true;
        }

        NGUITools.MarkParentAsChanged(cardObject);


        switch (newZone)
        {
            case CardContainer.CardZone.None:
                Destroy(cardObject);
                break;
            case CardContainer.CardZone.Hand:
                break;
            case CardContainer.CardZone.Play:
                break;
            case CardContainer.CardZone.Discard:
                NGUITools.BringForward(cardObject);
                DeckManager.deckManager.AddCardToDiscard(DeckManager.GetCardDefinition(cardObject), cardController.gameCard.isGainedCard);
                break;
            default:
                break;
        }
    }
}
