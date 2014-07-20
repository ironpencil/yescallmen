using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class LevelSelectedCardsEvent : CardEvent
{

    public int numCards = 1;
    public int numRequiredCards = 0;

    public string promptText = "You may level up a card from your hand.";

    public string OKButtonText = "Level Up";
    public string CancelButtonText = "No Thanks";

    public CardContainer.CardZone cardSource = CardContainer.CardZone.Hand;

    public override bool Execute()
    {
        CardSelectionController.ButtonOption buttons = (numRequiredCards > 0 ? CardSelectionController.ButtonOption.okOnly : 
                                                                    CardSelectionController.ButtonOption.okCancel);

        CardSelectionController csc = CardSelectionController.cardSelectionController;

        csc.Setup(promptText, buttons, numCards, numRequiredCards, cardSource);

        csc.OKButtonText = OKButtonText;
        csc.CancelButtonText = CancelButtonText;

        csc.onFinish += OnSelectionFinished;

        TurnManager.turnManager.ChangeState(TurnManager.TurnState.PlayerInactive);
        csc.Show();

        eventFinished = false;
        return eventFinished;
    }

    private void OnSelectionFinished()
    {
        Debug.Log("LevelUp:OnSelectionFinished()");
        List<CardController> selectedCards = CardSelectionController.cardSelectionController.GetCards();

        foreach (CardController card in selectedCards)
        {
            if (CardSelectionController.cardSelectionController.Result == CardSelectionController.SelectionResult.OK)
            {
                Debug.Log("OSF: Level Up!");
                card.gameCard.LevelUp();
            }
                Debug.Log("OSF: Return!");
                //put cards back in hand
                CardZoneManager.cardZoneManager.MoveCardToZone(card.gameObject, CardContainer.CardZone.Hand);
        }

        CardSelectionController.cardSelectionController.Close();

        eventFinished = true;
        TurnManager.turnManager.ChangeState(TurnManager.TurnState.PlayerActive);
    }

}
