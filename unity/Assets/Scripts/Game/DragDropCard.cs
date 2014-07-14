using UnityEngine;
using System.Collections;

public class DragDropCard : UIDragDropItem {

    public CardController cardController;

    protected override void OnDragDropStart()
    {
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
        }

		base.OnDragDropRelease(surface);

        if (surface != null)
        {
            Debug.Log("Dropped " + gameObject.name + " now a child of " + gameObject.transform.parent.gameObject.name);

            cardController.UpdateCurrentZone();

            switch (cardController.CurrentZone)
            {
                case CardContainer.CardZone.Hand:
                    //gameObject.GetComponent<UIDragDropContainer>().enabled = false;
                    break;
                case CardContainer.CardZone.Play:
                    //card was moved to play, trigger its event
                    CardEventManager.cardEventManager.QueueEvents(gameObject);
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
                    GameCard gameCard = gameObject.GetComponent<GameCard>();
                    if (gameCard != null)
                    {
                        if (gameCard.cardDefinition != null)
                        {
                            DeckManager.deckManager.AddCardToDiscard(gameCard.cardDefinition);
                        }
                    }
                    //gameObject.GetComponent<UIDragDropContainer>().enabled = false;
                    break;
                default:
                    break;
            }
                
            //NGUITools.PushBack(gameObject);
        }
	}
}
