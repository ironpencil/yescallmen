using UnityEngine;
using System.Collections;
using System;

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

    public void Draw()
    {
        DrawCardToZone(CardContainer.CardZone.Hand);
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
                GameMessageManager.gameMessageManager.AddLine("Drew card: " + nextCard.CardName, false);
                newCard = CardFactory.cardFactory.CreateCard(nextCard, gameObject);
            }
            //newCard = CardFactory.cardFactory.CreateCard(cardName, 1, gameObject);
        }

        return newCard;
    }

    public GameObject DrawCardToZone(CardContainer.CardZone destinationZone)
    {

        //NGUITools.AddChild(HandTable.gameObject, CardPrefab);
        GameObject newCard = DrawCard();

        
        //GameObject newCard = NGUITools.AddChild(gameObject, CardPrefab);

        //newCard.transform.localPosition = CardPrefab.transform.localPosition;

        //NGUITools.BringForward(NewCard);

        //if (TweenDrawCard != null)
        //{
        //    TweenDrawCard.from = NewCard.transform.position;
        //    TweenDrawCard.to = DisplayPosition;
        //    TweenDrawCard.PlayForward();
        //}

        //ReParentNewCard(newCard);


        CardDisplayController.cardDisplayController.DisplayCard(newCard, destinationZone, 1.0f);
        
        //DisplayCard(newCard, destinationZone);

        return newCard;

        //HandTable.repositionNow = true;
    }

    public void DrawToFullHand()
    {
        int currentHandSize = CardZoneManager.cardZoneManager.GetCardsInZone(CardContainer.CardZone.Hand).Count;

        for (int i = currentHandSize; i < maximumHandSize; i++)
        {
            DrawCardToZone(CardContainer.CardZone.Hand);
        }
    }
}
