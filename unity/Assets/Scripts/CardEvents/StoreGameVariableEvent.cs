using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class StoreGameVariableEvent : CardEvent
{

    public enum GameVariable
    {
        Actions,
        TotalSpite
    }

    public GameVariable variableToStore = GameVariable.TotalSpite;
    
    public string resultVariable = "GameVariable";

    public override bool Execute()
    {
        int variableValue = 0;

        switch (variableToStore)
        {
            case GameVariable.Actions:
                variableValue = RulesManager.rulesManager.ActionsLeft;
                break;
            case GameVariable.TotalSpite:
                variableValue = RulesManager.rulesManager.SpiteTotal;
                break;
            default:
                break;
        }

        gameCard.eventVariables[resultVariable] = variableValue.ToString();           

        eventFinished = true;
        return eventFinished;
    }

}
