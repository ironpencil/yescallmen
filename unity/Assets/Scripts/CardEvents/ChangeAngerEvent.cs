using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ChangeAngerEvent : CardEvent
{

    public int angerAmount = 1;

    public bool useTotalSpite;

    public double amountModifier = 1.0;

    public bool changeMaxAnger = false;

    public override bool Execute()
    {
        int modifyAngerValue = angerAmount;

        if (useTotalSpite)
        {
            modifyAngerValue = RulesManager.rulesManager.SpiteTotal;
        }

        modifyAngerValue = (int)(modifyAngerValue * amountModifier);

        if (changeMaxAnger)
        {
            BattleManager.battleManager.PlayerMaxAnger += modifyAngerValue;
        }
        else
        {
            BattleManager.battleManager.PlayerCurrentAnger += modifyAngerValue;
        }

        eventFinished = true;
        return eventFinished;
    }

}
