using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class DrawPileController : MonoBehaviour {

    public static DrawPileController drawPile;

    public GameObject CardPrefab;

    public bool generateRandom = false;

    public int maximumHandSize = 5;
    //private GameObject NewCard;
	// Use this for initialization
	void Start () {

        drawPile = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}    

    public GameObject DrawCard()
    {
        GameObject newCard = null;

        if (generateRandom)
        {
            Array cardNames = Enum.GetValues(typeof(CardFactory.CardName));

            int random = UnityEngine.Random.Range(0, cardNames.Length);

            CardFactory.CardName randomCard = (CardFactory.CardName)cardNames.GetValue(random);

            newCard = CardFactory.cardFactory.CreateCard(randomCard, 1, gameObject);
        }
        else
        {
            CardDefinition nextCard = DeckManager.deckManager.DrawFromDeck(false, false);

            if (nextCard != null)
            {                
                newCard = CardFactory.cardFactory.CreateCard(nextCard, gameObject);
                GameCard gameCard = newCard.GetComponent<GameCard>();
                if (gameCard != null)
                {
                    //GameMessageManager.gameMessageManager.AddLine("Drew card: " + gameCard.Title, false);
                }
            }
            //newCard = CardFactory.cardFactory.CreateCard(cardName, 1, gameObject);
        }        

        return newCard;
    }

    public GameObject DrawCardToZone(CardContainer.CardZone destinationZone, float displayTime)
    {
        GameObject newCard = DrawCard();

        CardDisplayController.cardDisplayController.DisplayCard(newCard, destinationZone, displayTime);

        return newCard;
    }

    public GameObject DrawCardToZone(CardContainer.CardZone destinationZone)
    {

        return DrawCardToZone(destinationZone, Globals.GetInstance().LONG_DISPLAY_TIME);
    }

    public List<GameObject> DrawToFullHand()
    {
        List<GameObject> cardObjects = new List<GameObject>();

        int currentHandSize = CardZoneManager.cardZoneManager.GetCardsInZone(CardContainer.CardZone.Hand).Count;

        for (int i = currentHandSize; i < maximumHandSize; i++)
        {
            GameObject newCard = DrawCardToZone(CardContainer.CardZone.Hand);
            if (newCard != null)
            {
                cardObjects.Add(newCard);
            }
        }

        return cardObjects;
    }
}
