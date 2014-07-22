using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class SpiteCheckerEvent : CardEvent
{
    
    public string spiteTotalVariable = "SpiteTotal";

    public int spiteThreshold = 10;

    public int numExtraCards = 1;
    public int numExtraActions = 1;

    public CardContainer.CardZone drawToZone = CardContainer.CardZone.Hand;

    public override bool Execute()
    {
        string spiteTotal;
        int spiteValue;

        //if the variable doesn't exist, or we can't parse it, set spiteValue to 0
        if (!gameCard.eventVariables.TryGetValue(spiteTotalVariable, out spiteTotal) ||
            !int.TryParse(spiteTotal, out spiteValue))
        {
                spiteValue = 0;        
        }

        if (spiteValue > spiteThreshold)
        {
            DrawPileController drawPile = DrawPileController.drawPile;

            if (drawPile != null)
            {
                for (int i = 0; i < numExtraCards; i++)
                {
                    drawPile.DrawCardToZone(drawToZone);
                }
            }

            RulesManager.rulesManager.ActionsLeft += numExtraActions;
        }        

        eventFinished = true;
        return eventFinished;
    }

}
