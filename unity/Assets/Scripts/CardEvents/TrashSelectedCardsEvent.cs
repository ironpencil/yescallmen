using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TrashSelectedCardsEvent : CardEvent
{

    public int numCards = 1;
    public int numRequiredCards = 0;

    public string promptText = "You may Trash a card from your hand.";

    public string OKButtonText = "Trash";
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
                Debug.Log("OSF: Destroy!");
                DeckManager.deckManager.TrashCard(card.gameCard.cardDefinition);

                //remember card in case an event later wants to reference it
                gameCard.rememberedCards.Add(card.gameCard.cardDefinition);

                //trash the card immediately
                card.TrashDelay = 0.0f;
                //move card to None zone to destroy
                CardZoneManager.cardZoneManager.MoveCardToZone(card.gameObject, CardContainer.CardZone.None);
                //UnityEngine.Object.Destroy(card.gameObject);
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

}
