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
        FatigueBoost,
        ConfusionBoost,
        AngerBoost
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
            gameCard = SetupCard(gameCard, cardDefinition);
            gameCard.cardDefinition = cardDefinition;
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
                gameCard.Title = "Fatigue";
                gameCard.AbilityText = "Draw a card, play another Fatigue card.";
                gameCard.cardType = GameCard.CardType.Action;
                gameCard.damageType = GameCard.DamageType.Fatigue;
                gameCard.BaseDamage = level;
                gameCard.CurrentDamage = level;
                break;
            case CardName.ConfusionAttack:
                gameCard.Title = "Confusion";
                gameCard.AbilityText = "Reveal cards, do damage based on Contradictions.";
                gameCard.cardType = GameCard.CardType.Action;
                gameCard.damageType = GameCard.DamageType.Confusion;
                gameCard.BaseDamage = level;
                gameCard.CurrentDamage = level;
                break;
            case CardName.AngerAttack:
                gameCard.Title = "Anger";
                gameCard.AbilityText = "Boost damage rate increases.";
                gameCard.cardType = GameCard.CardType.Action;
                gameCard.damageType = GameCard.DamageType.Anger;
                gameCard.BaseDamage = level * 2;
                gameCard.CurrentDamage = level * 2;
                break;
            case CardName.FatigueBoost:
                gameCard.Title = "Fallacy";
                gameCard.AbilityText = "Play on Fatigue cards.";
                gameCard.cardType = GameCard.CardType.Boost;
                gameCard.damageType = GameCard.DamageType.Fatigue;
                gameCard.BaseDamage = 0;
                gameCard.CurrentDamage = 0;
                break;
            case CardName.ConfusionBoost:
                gameCard.Title = "Contradiction";
                gameCard.AbilityText = "Play on Confusion cards.";
                gameCard.cardType = GameCard.CardType.Boost;
                gameCard.damageType = GameCard.DamageType.Confusion;
                gameCard.BaseDamage = 0;
                gameCard.CurrentDamage = 0;
                break;
            case CardName.AngerBoost:
                gameCard.Title = "Insult";
                gameCard.AbilityText = "Play on Anger cards.";
                gameCard.cardType = GameCard.CardType.Boost;
                gameCard.damageType = GameCard.DamageType.Anger;
                gameCard.BaseDamage = 0;
                gameCard.CurrentDamage = 0;
                break;
            default:
                break;
        }


        return gameCard;
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
