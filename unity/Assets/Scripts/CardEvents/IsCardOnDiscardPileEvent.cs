using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class IsCardOnDiscardPileEvent : CardEvent
{

    public CardFactory.CardName cardName;

    public string resultVariable = "IsCardOnDiscard";

    public string resultValueTrue = true.ToString();
    public string resultValueFalse = false.ToString();

    public override bool Execute()
    {
        CardDefinition cardOnDiscard = DeckManager.deckManager.DrawFromDeck(true, true);

        bool isCardOnDiscard = false;

        if (cardOnDiscard != null)
        {
            if (cardOnDiscard.CardName == cardName)
            {
                isCardOnDiscard = true;
            }
        }

        gameCard.eventVariables[resultVariable] = (isCardOnDiscard ? resultValueTrue : resultValueFalse);           

        eventFinished = true;
        return eventFinished;
    }

}
