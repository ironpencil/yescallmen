using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CardDefinition : ScriptableObject
{
    public CardFactory.CardName CardName;

    public int Level;

    public static CardDefinition GetNewInstance(CardFactory.CardName cardName, int level)
    {
        CardDefinition card = ScriptableObject.CreateInstance<CardDefinition>();

        card.CardName = cardName;
        card.Level = level;

        return card;
    }
}
