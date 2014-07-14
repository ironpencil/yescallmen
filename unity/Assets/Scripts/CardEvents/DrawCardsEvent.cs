using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class DrawCardsEvent : CardEvent
{

    public int numCards = 1;

    public override bool Execute()
    {
        DrawPileController drawPile = DrawPileController.drawPile;

        if (drawPile != null)
        {

            for (int i = 0; i < numCards; i++)
            {
                drawPile.DrawCard();
            }
        }

        eventFinished = true;
        return eventFinished;
    }

    //public List<CardEvent> OnFinished = new List<CardEvent>();
}
