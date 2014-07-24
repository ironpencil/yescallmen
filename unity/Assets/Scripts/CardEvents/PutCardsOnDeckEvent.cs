using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PutCardsOnDeckEvent : CardEvent
{

    public int numCards = 1;
    public int numRequiredCards = 0;

    public string promptText = "You may put a card on top of the deck.";

    public string OKButtonText = "Return to Deck";
    public string CancelButtonText = "No Thanks";

    public CardContainer.CardZone cardSource = CardContainer.CardZone.Hand;

    public bool canCancel = false;

    public override bool Execute()
    {
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
                Debug.Log("Deck OSF: Return to deck!");
                CardZoneManager.cardZoneManager.MoveCardToZone(card.gameObject, CardContainer.CardZone.Deck);

            }
            else
            {
                Debug.Log("Deck OSF: Return to hand!");
                //put cards back in hand
                CardZoneManager.cardZoneManager.MoveCardToZone(card.gameObject, cardSource);
            }
        }

        CardSelectionController.cardSelectionController.Close();

        eventFinished = true;
        //TurnManager.turnManager.ChangeState(TurnManager.TurnState.PlayerActive);
    }

}
