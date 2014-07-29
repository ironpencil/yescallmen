using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class DiscardCardsEvent : CardEvent
{

    public int numCards = 1;
    public int numRequiredCards = 0;

    public string promptText = "You may discard a card from your hand.";

    public string OKButtonText = "Discard";
    public string CancelButtonText = "No Thanks";

    public CardContainer.CardZone cardSource = CardContainer.CardZone.Hand;

    public bool canCancel = false;

    public override bool Execute()
    {
        //first check to see if we have the required number of cards in the source zone
        List<CardController> cardsInZone = CardZoneManager.cardZoneManager.GetCardsInZone(cardSource);
        int numCardsInZone = cardsInZone.Count;

        //if we require cards but don't have enough in the zone, don't bother displaying selection window
        if (numRequiredCards > numCardsInZone)
        {
            GameMessageManager.gameMessageManager.AddLine(">> Not enough cards for " + gameCard.Title + ". Discarding all cards.", false, GameMessageManager.Speaker.System);
            //we don't, so just move all cards there
            foreach (CardController card in cardsInZone)
            {
                SelectCard(card);
            }
            eventFinished = true;
            return eventFinished;
        }

        CardSelectionController.ButtonOption buttons = (numRequiredCards > 0 ? CardSelectionController.ButtonOption.okOnly : 
                                                                    CardSelectionController.ButtonOption.okCancel);

        if (canCancel) { buttons = CardSelectionController.ButtonOption.okCancel; }

        CardSelectionController csc = CardSelectionController.cardSelectionController;

        csc.Setup(promptText, buttons, canCancel, numCards, numRequiredCards, cardSource);

        csc.OKButtonText = OKButtonText;
        csc.CancelButtonText = CancelButtonText;

        csc.onFinish += OnSelectionFinished;

        //TurnManager.turnManager.ChangeState(TurnManager.TurnState.PlayerInactive);
        csc.Show();

        eventFinished = false;
        return eventFinished;
    }

    private void OnSelectionFinished()
    {
        Debug.Log("OnSelectionFinished()");
        List<CardController> selectedCards = CardSelectionController.cardSelectionController.GetCards();

        foreach (CardController card in selectedCards)
        {
            if (CardSelectionController.cardSelectionController.Result == CardSelectionController.SelectionResult.OK)
            {
                SelectCard(card);
            }
            else
            {
                Debug.Log("OSF: Return!");
                //put cards back in hand
                CardZoneManager.cardZoneManager.MoveCardToZone(card.gameObject, cardSource);
            }
        }

        CardSelectionController.cardSelectionController.Close();

        eventFinished = true;
        //TurnManager.turnManager.ChangeState(TurnManager.TurnState.PlayerActive);
    }

    private void SelectCard(CardController card)
    {
        Debug.Log("OSF: Destroy!");
        //DeckManager.deckManager.TrashCard(card.gameCard.cardDefinition);
        CardZoneManager.cardZoneManager.MoveCardToZone(card.gameObject, CardContainer.CardZone.Discard);

        //remember card in case an event later wants to reference it
        gameCard.rememberedCards.Add(card.gameCard.cardDefinition);   
    }

}
