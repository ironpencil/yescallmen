using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TrasherBenefitEvent : CardEvent
{   
    public int numExtraCards = 1;
    public int numExtraActions = 1;

    public CardContainer.CardZone drawToZone = CardContainer.CardZone.Hand;

    public override bool Execute()
    {

        if (gameCard.rememberedCards.Count > 0)
        {
            DrawPileController drawPile = DrawPileController.drawPile;

            if (drawPile != null)
            {
                for (int i = 0; i < numExtraCards; i++)
                {
                    drawPile.DrawCardToZone(drawToZone, Globals.GetInstance().SHORT_DISPLAY_TIME);
                }
            }

            RulesManager.rulesManager.ActionsLeft += numExtraActions;
        }        

        eventFinished = true;
        return eventFinished;
    }

}
