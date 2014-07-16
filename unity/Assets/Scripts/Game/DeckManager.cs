using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class DeckManager : MonoBehaviour {

    public static DeckManager deckManager;
    //end of the list is the top, beginning is the bottom
    List<CardDefinition> deck = new List<CardDefinition>();
    
    //end of the list is the top, beginning is the bottom
    List<CardDefinition> discard = new List<CardDefinition>();


    public delegate void OnShuffleDiscardIntoDeck();

    public OnShuffleDiscardIntoDeck onShuffleDiscardIntoDeck;

    public int deckCount = 0;
    public int discardCount = 0;

    public int DeckCount { get { return deck.Count; } }
    public int DiscardCount { get { return discard.Count; } }

	// Use this for initialization
	void Start () {

        deckManager = this;

        //just create a random deck of 10 cards for now
        //for (int i = 0; i < 10; i++)
        //{
        //    Array cardNames = Enum.GetValues(typeof(CardFactory.CardName));

        //    int random = UnityEngine.Random.Range(0, cardNames.Length);

        //    CardFactory.CardName randomCard = (CardFactory.CardName)cardNames.GetValue(random);

        //    AddCardToDeck(new CardDefinition(randomCard, 1));
        //}

        AddCardToDeck(new CardDefinition(CardFactory.CardName.AngerAttack, 1));
        AddCardToDeck(new CardDefinition(CardFactory.CardName.AngerAttack, 1));
        AddCardToDeck(new CardDefinition(CardFactory.CardName.AngerAttack, 1));
        AddCardToDeck(new CardDefinition(CardFactory.CardName.AngerAttack, 1));

        AddCardToDeck(new CardDefinition(CardFactory.CardName.FatigueAttack, 1));
        AddCardToDeck(new CardDefinition(CardFactory.CardName.FatigueAttack, 1));
        AddCardToDeck(new CardDefinition(CardFactory.CardName.FatigueAttack, 1));

        AddCardToDeck(new CardDefinition(CardFactory.CardName.ConfusionAttack, 1));
        AddCardToDeck(new CardDefinition(CardFactory.CardName.ConfusionAttack, 1));
        AddCardToDeck(new CardDefinition(CardFactory.CardName.ConfusionAttack, 1));

        ShuffleDeck();
	
	}
	
	// Update is called once per frame
	void Update () {

        deckCount = DeckCount;
        discardCount = DiscardCount;
	
	}

    public void ShuffleDiscardIntoDeck()
    {
        deck.AddRange(discard);
        discard = new List<CardDefinition>();
        ShuffleDeck();

        // Notify the listener
        if (onShuffleDiscardIntoDeck != null)
        {
            onShuffleDiscardIntoDeck();
        }
    }

    public void ShuffleDeck()
    {
        deck.Shuffle();
    }

    public void AddCardToDeck(CardDefinition card)
    {
        AddCardToDeck(card, true);
    }

    public void AddCardToDeck(CardDefinition card, bool top)
    {
        if (top)
        {
            deck.Add(card);
        }
        else
        {
            deck.Insert(0, card);
        }
    }

    public void AddCardToDiscard(CardDefinition card)
    {
        discard.Add(card);
    }

    public static CardDefinition GetCardDefinition(GameObject cardObject)
    {
        GameCard gameCard = cardObject.GetComponent<GameCard>();

        if (gameCard != null)
        {
            return gameCard.cardDefinition;
        }

        return null;

    }

    public CardDefinition DrawFromDeck(bool discardPile, bool peek)
    {
        List<CardDefinition> targetPile;

        //check if target pile is empty
        if (discardPile)
        {            
            if (discard.Count == 0)
            {
                return null;
            }
            targetPile = discard;
        }
        else
        {
            if (deck.Count == 0)
            {
                if (discard.Count == 0)
                {
                    //no cards left anywhere!
                    return null;
                }
                else
                {
                    ShuffleDiscardIntoDeck();
                }
            }
            targetPile = deck;
        }

        int lastIndex = targetPile.Count - 1;
        CardDefinition cardToDraw = targetPile[lastIndex];

        if (!peek)
        {
            targetPile.RemoveAt(lastIndex);
        }

        return cardToDraw;
    }

}