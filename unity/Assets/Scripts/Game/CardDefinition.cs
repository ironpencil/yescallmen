using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Xml.Serialization;

[Serializable]
public class CardDefinition
{
    public CardFactory.CardName CardName;

    public int Level;

    public CardDefinition(CardFactory.CardName cardName, int level)
    {
        CardName = cardName;
        Level = level;

        //Debug.Log("XML = " + this.ToXML());
        //Debug.Log("String = " + this.ToString());
    }

    public CardDefinition() { }

    //public static CardDefinition GetNewInstance(CardFactory.CardName cardName, int level)
    //{
    //    CardDefinition card = ScriptableObject.CreateInstance<CardDefinition>();

    //    card.CardName = cardName;
    //    card.Level = level;

    //    return card;
    //}

    //public string ToXML()
    //{
    //    var stringwriter = new System.IO.StringWriter();
    //    var serializer = new XmlSerializer(this.GetType());
    //    serializer.Serialize(stringwriter, this);
    //    return stringwriter.ToString();
    //}

    //public static CardDefinition LoadFromXMLString(string xmlText)
    //{
    //    var stringReader = new System.IO.StringReader(xmlText);
    //    var serializer = new XmlSerializer(typeof(CardDefinition));
    //    return serializer.Deserialize(stringReader) as CardDefinition;
    //}
}
