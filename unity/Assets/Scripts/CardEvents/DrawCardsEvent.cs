using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class DrawCardsEvent : CardEvent
{

    public int numCards = 1;

    public CardContainer.CardZone drawToZone = CardContainer.CardZone.Hand;

    public override bool Execute()
    {
        DrawPileController drawPile = DrawPileController.drawPile;

        if (drawPile != null)
        {

            for (int i = 0; i < numCards; i++)
            {
                drawPile.DrawCard(drawToZone);
            }
        }

        eventFinished = true;
        return eventFinished;
    }

}
