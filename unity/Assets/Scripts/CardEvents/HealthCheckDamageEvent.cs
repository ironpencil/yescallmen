using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class HealthCheckDamageEvent : CardEvent
{

    public int bonusDamage = 0;

    public double maxAngerPercent = 0.5;

    public override bool Execute()
    {
        int currentAnger = BattleManager.battleManager.PlayerCurrentAnger;

        int angerThreshold = (int) (BattleManager.battleManager.PlayerMaxAnger * maxAngerPercent);

        if (currentAnger > angerThreshold)
        {

            BattleManager.battleManager.DamageEnemy(GameCard.DamageType.Anger, bonusDamage);

        }

        eventFinished = true;
        return eventFinished;
    }

    //public List<CardEvent> OnFinished = new List<CardEvent>();
}
