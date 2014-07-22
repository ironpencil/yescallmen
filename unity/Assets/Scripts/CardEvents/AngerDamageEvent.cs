using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class AngerDamageEvent : CardEvent
{

    public int maxDamage = 5;

    public override bool Execute()
    {
        int damage = RulesManager.rulesManager.SpiteTotal;

        //damage = damage * gameCard.Level;

        //Debug.Log("Anger played. Damage=" + damage + " Level=" + gameCard.Level);

        damage = Mathf.Min(maxDamage, damage);

        BattleManager.battleManager.DamageEnemy(GameCard.DamageType.Anger, damage);

        eventFinished = true;
        return eventFinished;
    }

    //public List<CardEvent> OnFinished = new List<CardEvent>();
}
