using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CardFactory : MonoBehaviour
{
    public GameObject CardPrefab;

    public static CardFactory cardFactory;

    public void Start()
    {
        cardFactory = this;
    }

    public enum CardName {
        FatigueAttack,
        ConfusionAttack,
        AngerAttack,
        Spite
    }

    public GameObject CreateCard(CardDefinition cardDefinition)
    {

        return CreateCard(cardDefinition, null);
    }

    public GameObject CreateCard(CardDefinition cardDefinition, GameObject parent)
    {
        if (parent == null) { parent = gameObject; }

        GameObject newCard = NGUITools.AddChild(parent, CardPrefab);

        newCard.transform.localPosition = CardPrefab.transform.localPosition;

        GameCard gameCard = newCard.GetComponent<GameCard>();

        if (gameCard != null)
        {
            gameCard.Level = cardDefinition.Level;
            gameCard.cardDefinition = cardDefinition;
            gameCard = SetupCard(gameCard, cardDefinition);            
            //gameCard.Text = GetCardText(cardName);
            //gameCard.Level = level;
            //gameCard.cardType = GetCardType(cardName);

        }

        return newCard;
    }

    public GameObject CreateCard(CardName cardName, int level) {
        return CreateCard(cardName, level, null);
    }

    public GameObject CreateCard(CardName cardName, int level, GameObject parent)
    {
        return CreateCard(new CardDefinition(cardName, level), parent);
    }

    private GameCard SetupCard(GameCard gameCard, CardDefinition cardDefinition)
    {
        int level = cardDefinition.Level;

        switch (cardDefinition.CardName)
        {
            case CardName.FatigueAttack:
                GenerateFatigueAttack(gameCard, level);

                break;
            case CardName.ConfusionAttack:
                GenerateConfusionAttack(gameCard, level);

                break;
            case CardName.AngerAttack:
                GenerateAngerAttack(gameCard, level);

                break;
            case CardName.Spite:
                GenerateSpite(gameCard, level);
                break;
            default:
                break;
        }


        return gameCard;
    }

    private void GenerateSpite(GameCard gameCard, int level)
    {
        gameCard.Title = "Spite";
        gameCard.AbilityText = "Gain " + level + " Spite.";
        gameCard.cardType = GameCard.CardType.Spite;
        gameCard.spiteAdded = level;
    }

    private static void GenerateConfusionAttack(GameCard gameCard, int level)
    {
        gameCard.Title = "Confusion";
        gameCard.AbilityText = "Uses 1 Spite. Reveal top card of deck. If it is an Argument, put it into play for free. Otherwise, discard it.";
        gameCard.cardType = GameCard.CardType.Argument;
        gameCard.damageType = GameCard.DamageType.Confusion;
        gameCard.BaseDamage = level * 2;
        gameCard.CurrentDamage = level * 2;
        gameCard.spiteUsed = 1;

        DealDamageEvent baseDmgEvent = new DealDamageEvent();
        baseDmgEvent.damageTypeSource = DealDamageEvent.ValueSource.Card;
        baseDmgEvent.damageAmountSource = DealDamageEvent.ValueSource.Card;
        gameCard.AddEvent(baseDmgEvent);

        ConfusionArgumentEvent confEvent = new ConfusionArgumentEvent();
        gameCard.AddEvent(confEvent);

    }

    private static void GenerateAngerAttack(GameCard gameCard, int level)
    {
        gameCard.Title = "Anger";
        gameCard.AbilityText = "Uses 2 Spite. Does damage equal to total Spite when played.";
        gameCard.cardType = GameCard.CardType.Argument;
        gameCard.damageType = GameCard.DamageType.Anger;
        gameCard.BaseDamage = 0;
        gameCard.CurrentDamage = 0;
        gameCard.spiteUsed = 2;

        AngerDamageEvent angDmgEvent = new AngerDamageEvent();
        gameCard.AddEvent(angDmgEvent);
    }

    private static void GenerateFatigueAttack(GameCard gameCard, int level)
    {
        gameCard.Title = "Fatigue";
        gameCard.AbilityText = "Uses 1 Spite. Draw a card.";
        gameCard.cardType = GameCard.CardType.Argument;
        gameCard.damageType = GameCard.DamageType.Fatigue;
        gameCard.BaseDamage = level;
        gameCard.CurrentDamage = gameCard.BaseDamage;
        gameCard.spiteUsed = 1;

        DealDamageEvent baseDmgEvent = new DealDamageEvent();
        baseDmgEvent.damageTypeSource = DealDamageEvent.ValueSource.Card;
        baseDmgEvent.damageAmountSource = DealDamageEvent.ValueSource.Card;
        gameCard.AddEvent(baseDmgEvent);

        //DealCardDamageEvent cardDamageEvent = new DealCardDamageEvent();
        //gameCard.AddEvent(cardDamageEvent);

        DrawCardsEvent drawEvent = new DrawCardsEvent();
        drawEvent.numCards = level;
        gameCard.AddEvent(drawEvent);
    }

    /* Old Card Generation Stuff
    private string GetCardText(CardName cardName)
    {
        string cardText = "";

        switch (cardName)
        {
            case CardName.FatigueAttack:
                cardText = "Fatigue";
                break;
            case CardName.ConfusionAttack:
                cardText = "Confusion";
                break;
            case CardName.AngerAttack:
                cardText = "Anger";
                break;
            case CardName.FatigueBoost:
                cardText = "Fallacy";
                break;
            case CardName.ConfusionBoost:
                cardText = "Contradiction";
                break;
            case CardName.AngerBoost:
                cardText = "Insult";
                break;
            default:
                break;
        }

        return cardText;
    }

    private List<CardName> actionCards = new List<CardName>
    {
        CardName.FatigueAttack, CardName.AngerAttack, CardName.ConfusionAttack
    };

    private List<CardName> boostCards = new List<CardName>
    {
        CardName.FatigueBoost, CardName.AngerBoost, CardName.ConfusionBoost
    };

    private List<CardName> specialCards = new List<CardName> { };    

    private GameCard.CardType GetCardType(CardName cardName)
    {
        if (actionCards.Contains(cardName)) { return GameCard.CardType.Action; }
        if (boostCards.Contains(cardName)) { return GameCard.CardType.Boost; }
        if (specialCards.Contains(cardName)) { return GameCard.CardType.Special; }

        return GameCard.CardType.None;

    }*/
}
