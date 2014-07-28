using UnityEngine;
using System.Collections;

public class DragDropCard : UIDragDropItem {

    public CardController cardController;

    protected override void OnDragDropStart()
    {
        Debug.Log("Drag Enter: " + cardController.gameCard.Title + " - " + cardController.CurrentZone.ToString());
        bool allowedToDrag = false;
        //check to see if we're allowed to drag this item from wherever it is
        switch (cardController.CurrentZone)
        {
            case CardContainer.CardZone.None:
                allowedToDrag = false;
                break;
            case CardContainer.CardZone.Hand:
                allowedToDrag = true;
                break;
            case CardContainer.CardZone.Play:
                allowedToDrag = false;
                break;
            case CardContainer.CardZone.Attached:
                allowedToDrag = false;
                break;
            case CardContainer.CardZone.Discard:
                allowedToDrag = false;
                break;
            case CardContainer.CardZone.Display:
                allowedToDrag = CardDisplayController.cardDisplayController.CurrentDisplayMode == CardDisplayController.DisplayMode.Selection;
                break;
            case CardContainer.CardZone.Selection:
                allowedToDrag = true;
                break;
            case CardContainer.CardZone.Deck:
                allowedToDrag = false;
                break;
            default:
                allowedToDrag = false;
                break;
        }

        if (allowedToDrag)
        {
            base.OnDragDropStart();
        }
    }

	protected override void OnDragDropRelease (GameObject surface)
	{
        Debug.Log("Drop Enter: " + cardController.gameCard.Title + " - " + cardController.CurrentZone.ToString() + 
            "\r\nSurface = " + (surface==null ? "null" : surface.ToString()));
            
        if (surface != null)
        {
            bool validSurfaceFound = false;

            while (!validSurfaceFound)
            {
                CardController otherCard = surface.GetComponent<CardController>();

                //if the surface is not a card, we can drop on it
                if (otherCard == null)
                {
                    validSurfaceFound = true;
                }
                else
                {
                    //if the surface is a card, we have to make sure we're allowed

                    //are we allowed to attach this card to the target card?
                    if (otherCard.CanAttach(cardController))
                    {
                        validSurfaceFound = true;
                    }
                    else
                    {
                        //try drop this card on the other card's parent instead
                        surface = surface.transform.parent.gameObject;
                    }
                }
            }            
            
            //if we're not allowed to drag to the destination, null the surface. card well return to original parent.
            if (!CardZoneManager.cardZoneManager.CanDragCardToZone(this, surface))
            {
                surface = null;
            }
        }

        Debug.Log("Surface = " + (surface==null ? "null" : surface.ToString()));
      
		base.OnDragDropRelease(surface);

        Debug.Log("Drop Released: " + cardController.gameCard.Title + " - " + cardController.CurrentZone.ToString());

        if (surface != null)
        {
            CardContainer.CardZone previousZone = cardController.CurrentZone;

            switch (previousZone)
            {
                case CardContainer.CardZone.None:
                    break;
                case CardContainer.CardZone.Hand:
                    break;
                case CardContainer.CardZone.Play:
                    break;
                case CardContainer.CardZone.Attached:
                    break;
                case CardContainer.CardZone.Discard:
                    break;
                case CardContainer.CardZone.Display:
                    break;
                case CardContainer.CardZone.Selection:
                    CardSelectionController.cardSelectionController.FilledSlots--;
                    break;
                default:
                    break;
            }

            Debug.Log("Dropped " + cardController.gameCard.Title + " now a child of " + gameObject.transform.parent.gameObject.name);

            cardController.UpdateCurrentZone();

            Debug.Log("Drop Assigned: " + cardController.gameCard.Title + " - " + cardController.CurrentZone.ToString());

            switch (cardController.CurrentZone)
            {
                case CardContainer.CardZone.Hand:
                    //CardZoneManager.cardZoneManager.handContainer.GetComponent<UIScrollView>().UpdatePosition();
                    //CardZoneManager.cardZoneManager.handContainer.GetComponent<UIScrollView>().ResetPosition();
                    //gameObject.GetComponent<UIDragDropContainer>().enabled = false;
                    break;
                case CardContainer.CardZone.Play:
                    //card was moved to play, trigger its event
                    RulesManager.rulesManager.PlayCard(gameObject, true);                    
                    //gameObject.GetComponent<UIDragDropContainer>().enabled = true;
                    break;                
                case CardContainer.CardZone.Attached:
                    int attachedCardCount = cardController.CurrentContainer.transform.childCount;
                    //string newCardName = (999-attachedCardCount).ToString().PadLeft(3, '0');
                    string newCardName = attachedCardCount.ToString().PadLeft(3, '0');
                    gameObject.name = newCardName;
                    NGUITools.PushBack(gameObject);
                    //gameObject.GetComponent<UIDragDropContainer>().enabled = false;
                    break;
                case CardContainer.CardZone.Discard:
                    NGUITools.BringForward(gameObject);
                    GameCard gameCard = cardController.gameCard;
                    if (gameCard != null)
                    {
                        if (gameCard.cardDefinition != null)
                        {
                            DeckManager.deckManager.AddCardToDiscard(gameCard.cardDefinition, gameCard.isGainedCard);
                        }
                    }
                    //gameObject.GetComponent<UIDragDropContainer>().enabled = false;
                    break;
                case CardContainer.CardZone.Selection:                    
                    NGUITools.BringForward(gameObject);
                    CardSelectionController.cardSelectionController.FilledSlots++;
                    break;
                case CardContainer.CardZone.Deck:
                    DeckManager.deckManager.AddCardToDeck(cardController.gameCard.cardDefinition, cardController.gameCard.isGainedCard);
                    Destroy(cardController.gameObject);
                    break;
                default:
                    break;
            }

            Debug.Log("Drop Completed: " + cardController.gameCard.Title + " - " + cardController.CurrentZone.ToString());
                
            //NGUITools.PushBack(gameObject);
        }

        Debug.Log("Drop Exit: " + cardController.gameCard.Title + " - " + cardController.CurrentZone.ToString());
	}
}
