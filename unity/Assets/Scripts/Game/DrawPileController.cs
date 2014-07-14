using UnityEngine;
using System.Collections;
using System;

public class DrawPileController : MonoBehaviour {

    public static DrawPileController drawPile;

    public UIWidgetContainer HandTable;
    public GameObject CardPrefab;

    public CardDisplayController DisplayController;

    public bool generateRandom = false;
    //private GameObject NewCard;
	// Use this for initialization
	void Start () {

        drawPile = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void DrawCard()
    {

        //NGUITools.AddChild(HandTable.gameObject, CardPrefab);
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
            CardDefinition nextCard = DeckManager.deckManager.DrawFromDeck();

            if (nextCard != null)
            {
                newCard = CardFactory.cardFactory.CreateCard(nextCard, gameObject);
            }
            //newCard = CardFactory.cardFactory.CreateCard(cardName, 1, gameObject);
        }

        //GameObject newCard = NGUITools.AddChild(gameObject, CardPrefab);

        //newCard.transform.localPosition = CardPrefab.transform.localPosition;

        if (newCard == null) { return; }

        //NGUITools.BringForward(NewCard);

        //if (TweenDrawCard != null)
        //{
        //    TweenDrawCard.from = NewCard.transform.position;
        //    TweenDrawCard.to = DisplayPosition;
        //    TweenDrawCard.PlayForward();
        //}

        //ReParentNewCard(newCard);

        

        DisplayCard(newCard);

        //HandTable.repositionNow = true;
    }

    public void DisplayCard(GameObject cardObject)
    {
        if (DisplayController != null)
        {
            DisplayController.DisplayCard(cardObject);
        }
    }

    public void ReParentNewCard(GameObject cardObject)
    {
        if (cardObject != null && HandTable != null)
        {
            cardObject.transform.parent = HandTable.transform;

            if (HandTable is UIGrid)
            {
                ((UIGrid)HandTable).repositionNow = true;
            }
            else if (HandTable is UITable)
            {
                ((UITable)HandTable).repositionNow = true;
            }
        }
    }
}
