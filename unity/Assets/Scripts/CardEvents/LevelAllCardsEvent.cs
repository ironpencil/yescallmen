using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class LevelAllCardsEvent : CardEvent
{

    public CardContainer.CardZone cardSource = CardContainer.CardZone.Hand;

    public override bool Execute()
    {
        List<CardController> cardsInZone = CardZoneManager.cardZoneManager.GetCardsInZone(cardSource);

        int cardsLeveled = 0;

        foreach (CardController card in cardsInZone)
        {
            if (card.gameCard.LevelUp())
            {
                cardsLeveled++;
            }
        }

        if (cardsLeveled == 1)
        {
            GameMessageManager.gameMessageManager.AddLine(">> " + gameCard.Title + ": Leveled up 1 card!", false, GameMessageManager.gameMessageManager.SystemColorHex);
        }
        else
        {
            GameMessageManager.gameMessageManager.AddLine(">> " + gameCard.Title + ": Leveled up " + cardsLeveled + " cards!", false, GameMessageManager.gameMessageManager.SystemColorHex);
        }

        eventFinished = true;
        return eventFinished;
    }

}
