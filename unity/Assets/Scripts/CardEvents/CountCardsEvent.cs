using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CountCardsEvent : CardEvent
{

    public CardContainer.CardZone countCardsInZone = CardContainer.CardZone.None;

    public GameCard.CardType countCardsOfType = GameCard.CardType.None;

    public CardFactory.CardName countCardsWithName = CardFactory.CardName.AngerAttack;

    public enum CountFilter
    {
        None,
        CardType,
        CardName
    }

    public CountFilter countFilter = CountFilter.None;

    public string cardCountVariable = "cardsInZone";

    public bool ignoreSelf = false;

    public override bool Execute()
    {

        List<CardController> cardsInZone = CardZoneManager.cardZoneManager.GetCardsInZone(countCardsInZone);

        int cardCount = 0;

        switch (countFilter)
        {
            case CountFilter.None:
                cardCount = cardsInZone.Count;
                break;
            case CountFilter.CardType:
                cardCount = CountCardsOfType(cardsInZone);
                break;
            case CountFilter.CardName:
                cardCount = CountCardsWithName(cardsInZone);
                break;
            default:
                break;
        }

        gameCard.eventVariables[cardCountVariable] = cardCount.ToString();

        eventFinished = true;
        return eventFinished;
    }

    private int CountCardsOfType(List<CardController> cardList)
    {
        int cardCount = 0;
        foreach (CardController card in cardList)
        {
            if (card.gameCard.cardType == countCardsOfType)
            {
                //do not count the card if it is us and we are excluding ourself
                if (!(ignoreSelf && card.gameObject == gameCard.gameObject))
                {
                    cardCount++;
                }
            }
        }
        return cardCount;
    }

    private int CountCardsWithName(List<CardController> cardList)
    {
        int cardCount = 0;
        foreach (CardController card in cardList)
        {
            if (card.gameCard.cardDefinition.CardName == countCardsWithName)
            {
                //do not count the card if it is us and we are excluding ourself
                if (!(ignoreSelf && card.gameObject == gameCard.gameObject))
                {
                    cardCount++;
                }
            }
        }
        return cardCount;
    }

    private int CountAllCards(List<CardController> cardList)
    {
        int cardCount = 0;
        if (ignoreSelf)
        {
            foreach (CardController card in cardList)
            {
                //do not count the card if it is us and we are excluding ourself
                if (!(ignoreSelf && card.gameObject == gameCard.gameObject))
                {
                    cardCount++;
                }
            }
        }
        else
        {
            cardCount = cardList.Count;
        }
        return cardCount;
    }

}
