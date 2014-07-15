using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class DealDamageEvent : CardEvent
{

    public GameCard.DamageType damageType = GameCard.DamageType.None;

    public int damageAmount = 0;

    public bool targetPlayer = false;

    public enum ValueSource
    {
        Card,
        SpecifyVariable,
        SpecifyValue
    }

    public ValueSource damageAmountSource = ValueSource.SpecifyValue;
    public ValueSource damageTypeSource = ValueSource.SpecifyValue;

    public string damageAmountVariable = "";
    public string damageTypeVariable = "";

    public float damageMultiplier = 1.0f;

    public override bool Execute()
    {
        //default damage and type uses the "SpecifyValue" value

        int actualDamage = damageAmount;

        //determine the damage amount based on where we're picking it from
        switch (damageAmountSource)
        {
            case ValueSource.Card:
                actualDamage = gameCard.CurrentDamage;
                break;
            case ValueSource.SpecifyVariable:
                string damageString;
                if (gameCard.eventVariables.TryGetValue(damageAmountVariable, out damageString))
                {
                    int variableDamage;
                    if (int.TryParse(damageString, out variableDamage))
                    {
                        actualDamage = variableDamage;
                    }
                }
                break;
            default:
                break;
        }        

        GameCard.DamageType actualDamageType = damageType;

        if (!targetPlayer)
        {
            //determine the damage type based on where we're picking it from
            switch (damageTypeSource)
            {
                case ValueSource.Card:
                    actualDamageType = gameCard.damageType;
                    break;
                case ValueSource.SpecifyVariable:
                    string damageTypeString;
                    if (gameCard.eventVariables.TryGetValue(damageAmountVariable, out damageTypeString))
                    {
                        try
                        {
                            actualDamageType = (GameCard.DamageType)Enum.Parse(typeof(GameCard.DamageType), damageTypeString);
                        }
                        catch { }
                    }
                    break;
                default:
                    break;
            }
        }

        actualDamage = (int)(actualDamage * damageMultiplier);

        if (targetPlayer)
        {
            BattleManager.battleManager.DamagePlayer(actualDamage);
        }
        else
        {
            BattleManager.battleManager.DamageEnemy(actualDamageType, actualDamage);
        }

        eventFinished = true;
        return eventFinished;
    }

    //public List<CardEvent> OnFinished = new List<CardEvent>();
}
