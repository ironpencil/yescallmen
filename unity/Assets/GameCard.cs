using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class GameCard
{
    public enum CardType
    {
        None,
        Action,
        Boost,
        Special
    }

    public enum DamageType
    {
        None,
        Anger,
        Confusion,
        Fatigue
    }

    public CardType cardType = CardType.None;

    public DamageType damageType = DamageType.None;

    public int Level = 1;

    public string Name = "";

    public int BaseDamage = 1;

    public int CurrentDamage = 1;

    public List<CardEvent> cardEvents = new List<CardEvent>();
}

