using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class CardDefinition
{
    public CardFactory.CardName CardName;

    public int Level;

    public CardDefinition(CardFactory.CardName cardName, int level)
    {
        CardName = cardName;
        Level = level;
    }
}
