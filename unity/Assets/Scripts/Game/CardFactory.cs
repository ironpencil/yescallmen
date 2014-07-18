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

    private static void GenerateConfusionAttack(GameCard gameCard, int level)
    {
        gameCard.Title = "Confusion";
        gameCard.AbilityText = "Discard top of deck, bonus damage if it is a Confusion attack.";
        gameCard.cardType = GameCard.CardType.Action;
        gameCard.damageType = GameCard.DamageType.Confusion;
        gameCard.BaseDamage = level;
        gameCard.CurrentDamage = level;

        DealDamageEvent baseDmgEvent = new DealDamageEvent();
        baseDmgEvent.damageTypeSource = DealDamageEvent.ValueSource.Card;
        baseDmgEvent.damageAmountSource = DealDamageEvent.ValueSource.Card;
        gameCard.AddEvent(baseDmgEvent);

        DrawCardsEvent drawEvent = new DrawCardsEvent();
        drawEvent.numCards = 1;
        drawEvent.drawToZone = CardContainer.CardZone.Discard;
        gameCard.AddEvent(drawEvent);

        WaitForDisplayedCardsEvent waitEvent = new WaitForDisplayedCardsEvent();
        gameCard.AddEvent(waitEvent);

        IsCardOnDiscardPileEvent discardCheckEvent = new IsCardOnDiscardPileEvent();
        discardCheckEvent.cardName = gameCard.cardDefinition.CardName;
        discardCheckEvent.resultVariable = "ConfusionBonusDamage";
        discardCheckEvent.resultValueTrue = "3";
        discardCheckEvent.resultValueFalse = "0";
        gameCard.AddEvent(discardCheckEvent);        

        DealDamageEvent bonusDmgEvent = new DealDamageEvent();
        bonusDmgEvent.damageTypeSource = DealDamageEvent.ValueSource.Card;
        bonusDmgEvent.damageAmountSource = DealDamageEvent.ValueSource.SpecifyVariable;
        bonusDmgEvent.damageAmountVariable = discardCheckEvent.resultVariable;
        //angerBonusDmgEvent.damageMultiplier = 2.0f;
        gameCard.AddEvent(bonusDmgEvent);


    }

    private static void GenerateAngerAttack(GameCard gameCard, int level)
    {
        gameCard.Title = "Anger";
        gameCard.AbilityText = "Bonus damage for each other Anger attack in play.";
        gameCard.cardType = GameCard.CardType.Action;
        gameCard.damageType = GameCard.DamageType.Anger;
        gameCard.BaseDamage = level;
        gameCard.CurrentDamage = level;

        DealDamageEvent baseDmgEvent = new DealDamageEvent();
        baseDmgEvent.damageTypeSource = DealDamageEvent.ValueSource.Card;
        baseDmgEvent.damageAmountSource = DealDamageEvent.ValueSource.Card;
        gameCard.AddEvent(baseDmgEvent);

        CountCardsEvent countEvent = new CountCardsEvent();
        countEvent.countCardsInZone = CardContainer.CardZone.Play;
        countEvent.countCardsWithName = CardName.AngerAttack;
        countEvent.ignoreSelf = true;
        countEvent.countFilter = CountCardsEvent.CountFilter.CardName;
        countEvent.cardCountVariable = "anger attacks already in play";
        gameCard.AddEvent(countEvent);

        DealDamageEvent bonusDmgEvent = new DealDamageEvent();
        bonusDmgEvent.damageTypeSource = DealDamageEvent.ValueSource.Card;
        bonusDmgEvent.damageAmountSource = DealDamageEvent.ValueSource.SpecifyVariable;
        bonusDmgEvent.damageAmountVariable = countEvent.cardCountVariable;
        //angerBonusDmgEvent.damageMultiplier = 2.0f;
        gameCard.AddEvent(bonusDmgEvent);
    }

    private static void GenerateFatigueAttack(GameCard gameCard, int level)
    {
        gameCard.Title = "Fatigue";
        gameCard.AbilityText = "Draw a card.";
        gameCard.cardType = GameCard.CardType.Action;
        gameCard.damageType = GameCard.DamageType.Fatigue;
        gameCard.BaseDamage = level;
        gameCard.CurrentDamage = gameCard.BaseDamage;

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
