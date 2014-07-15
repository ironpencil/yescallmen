using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class DealCardDamageEvent : CardEvent
{

    public override bool Execute()
    {
        int actualDamage = gameCard.CurrentDamage;

        GameCard.DamageType damageType = gameCard.damageType;
        
        BattleManager.battleManager.DamageEnemy(damageType, actualDamage);

        eventFinished = true;
        return eventFinished;
    }
    
}
