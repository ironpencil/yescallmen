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

        bool drewCards = false;

        if (drawPile != null)
        {

            for (int i = 0; i < numCards; i++)
            {
                if (drawPile.DrawCardToZone(drawToZone, Globals.GetInstance().SHORT_DISPLAY_TIME) != null)
                {
                    drewCards = true;
                }
                
            }
        }

        eventFinished = !drewCards;
        return eventFinished;
    }

    public override void Update()
    {
        if (CardDisplayController.cardDisplayController.displayedCards.Count == 0)
        {
            eventFinished = true;
        }
    }

}
