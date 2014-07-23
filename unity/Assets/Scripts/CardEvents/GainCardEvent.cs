using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class GainCardEvent : CardEvent
{
    public CardDefinition cardToGain = null;

    public CardContainer.CardZone addToZone = CardContainer.CardZone.Hand;

    public override bool Execute()
    {
        GameObject newCard = null;
        GameObject parent = CardZoneManager.cardZoneManager.displayContainer;

        if (cardToGain != null)
        {
            newCard = CardFactory.cardFactory.CreateCard(cardToGain, parent);

            GameCard gameCard = newCard.GetComponent<GameCard>();

            gameCard.isGainedCard = true;

            //GameMessageManager.gameMessageManager.AddLine("Drew card: " + gameCard.Title, false);
            CardDisplayController.cardDisplayController.DisplayCard(newCard, addToZone, 1.0f);

        }

        eventFinished = true;
        return eventFinished;
    }

}
