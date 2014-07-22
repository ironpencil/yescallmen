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
        Spite,
        Trasher,
        NotAllMen,
        HealCard,
        Village,
        Smithy,
        DblLeveler,
        SpiteChecker,
        IncreaseMaxAnger
    }

    public GameObject CreateCard(CardDefinition cardDefinition)
    {

        return CreateCard(cardDefinition, null);
    }

    public GameObject CreateCard(CardDefinition cardDefinition, GameObject parent)
    {
        //if (parent == null) { parent = gameObject; }

        GameObject newCard = NGUITools.AddChild(parent, CardPrefab);

        newCard.transform.localPosition = CardPrefab.transform.localPosition;

        GameCard gameCard = newCard.GetComponent<GameCard>();

        if (gameCard != null)
        {                        
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

    public GameCard SetupCard(GameCard gameCard, CardDefinition cardDefinition)
    {
        int level = cardDefinition.Level;

        gameCard.cardDefinition = cardDefinition;

        gameCard.Level = level;

        gameCard.cardEvents.Clear();

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
            case CardName.Trasher:
                GenerateTrasher(gameCard, level);
                break;
            case CardName.NotAllMen:
                GenerateNotAllMen(gameCard, level);
                break;
            case CardName.HealCard:
                GenerateHealCard(gameCard, level);
                break;
            case CardName.Village:
                GenerateVillage(gameCard, level);
                break;
            case CardName.Smithy:
                GenerateSmith(gameCard, level);
                break;
            case CardName.DblLeveler:
                GenerateDblLeveler(gameCard, level);
                break;
            case CardName.SpiteChecker:
                GenerateSpiteChecker(gameCard, level);
                break;
            case CardName.IncreaseMaxAnger:
                GenerateIncreaseMaxAngerCard(gameCard, level);
                break;
            default:
                break;
        }

        if (gameCard.cardType != GameCard.CardType.Argument &&
            gameCard.cardType != GameCard.CardType.Spite)
        {
            //only argument and spite cards have a level value
            gameCard.Level = 0;
        }


        return gameCard;
    }

    private void GenerateIncreaseMaxAngerCard(GameCard gameCard, int level)
    {
        gameCard.Title = "Raise Max Anger";
        gameCard.AbilityText = "Special (No Action). Draw 5 cards. Raise Max Anger by 10. Trash this card when played.";
        gameCard.cardType = GameCard.CardType.Special;

        DrawCardsEvent drawEvent = new DrawCardsEvent();
        drawEvent.numCards = 5;
        drawEvent.drawToZone = CardContainer.CardZone.Hand;

        gameCard.AddEvent(drawEvent);        

        ChangeAngerEvent healEvent = new ChangeAngerEvent();
        healEvent.useTotalSpite = false;
        healEvent.angerAmount = 10;
        healEvent.changeMaxAnger = true;

        gameCard.AddEvent(healEvent);

        TrashSelfEvent trashEvent = new TrashSelfEvent();
        gameCard.AddEvent(trashEvent);
    }

    private void GenerateSpiteChecker(GameCard gameCard, int level)
    {
        gameCard.Title = "Spite Checker";
        gameCard.AbilityText = "Action. Gain 1 Card. Gain 1 Action. If you have more than 10 Total Spite, gain an additional card and action.";
        gameCard.cardType = GameCard.CardType.Action;

        gameCard.actionsAdded = 1;

        DrawCardsEvent drawEvent = new DrawCardsEvent();
        drawEvent.numCards = 1;
        drawEvent.drawToZone = CardContainer.CardZone.Hand;

        gameCard.AddEvent(drawEvent);

        StoreGameVariableEvent storeEvent = new StoreGameVariableEvent();
        storeEvent.variableToStore = StoreGameVariableEvent.GameVariable.TotalSpite;
        storeEvent.resultVariable = "TotalSpite";

        gameCard.AddEvent(storeEvent);

        SpiteCheckerEvent spiteEvent = new SpiteCheckerEvent();
        spiteEvent.spiteThreshold = 10;
        spiteEvent.spiteTotalVariable = "TotalSpite";
        spiteEvent.numExtraActions = 1;
        spiteEvent.numExtraCards = 1;

        gameCard.AddEvent(spiteEvent);
        
    }

    private void GenerateDblLeveler(GameCard gameCard, int level)
    {
        gameCard.Title = "Remake";
        gameCard.AbilityText = "Action. Level up exactly 2 cards from your hand, then discard them.";
        gameCard.cardType = GameCard.CardType.Action;

        LevelSelectedCardsEvent levelEvent = new LevelSelectedCardsEvent();
        levelEvent.numCards = 2;
        levelEvent.numRequiredCards = 2;
        levelEvent.promptText = "Level up 2 cards from your hand. They will then be discarded.";
        levelEvent.cardDestination = CardContainer.CardZone.Discard;
        levelEvent.canCancel = true;

        gameCard.AddEvent(levelEvent);
    }

    private void GenerateSmith(GameCard gameCard, int level)
    {
        gameCard.Title = "Smithy";
        gameCard.AbilityText = "Action. Draw 3 cards.";
        gameCard.cardType = GameCard.CardType.Action;

        DrawCardsEvent drawEvent = new DrawCardsEvent();
        drawEvent.numCards = 3;
        drawEvent.drawToZone = CardContainer.CardZone.Hand;

        gameCard.AddEvent(drawEvent);

    }

    private void GenerateVillage(GameCard gameCard, int level)
    {
        gameCard.Title = "Village";
        gameCard.AbilityText = "Action. Draw a card. Gain 2 Actions. Gain 1 Spite.";
        gameCard.cardType = GameCard.CardType.Action;

        gameCard.actionsAdded = 2;
        gameCard.spiteAdded = 1;

        DrawCardsEvent drawEvent = new DrawCardsEvent();
        drawEvent.numCards = 1;
        drawEvent.drawToZone = CardContainer.CardZone.Hand;

        gameCard.AddEvent(drawEvent);
    }

    private void GenerateHealCard(GameCard gameCard, int level)
    {
        gameCard.Title = "Heal";
        gameCard.AbilityText = "Action. Lower current Anger by Total Spite.";
        gameCard.cardType = GameCard.CardType.Action;

        ChangeAngerEvent healEvent = new ChangeAngerEvent();
        healEvent.useTotalSpite = true;
        healEvent.amountModifier = -1.0;

        gameCard.AddEvent(healEvent);
    }

    private void GenerateNotAllMen(GameCard gameCard, int level)
    {
        gameCard.Title = "Not All Men";
        gameCard.AbilityText = "Special (No Action). Draw 5 cards. May level up 1 card from hand. Trash this card when played.";
        gameCard.cardType = GameCard.CardType.Special;

        DrawCardsEvent drawEvent = new DrawCardsEvent();
        drawEvent.numCards = 5;
        drawEvent.drawToZone = CardContainer.CardZone.Hand;

        gameCard.AddEvent(drawEvent);

        LevelSelectedCardsEvent levelEvent = new LevelSelectedCardsEvent();
        levelEvent.numCards = 1;
        levelEvent.numRequiredCards = 0;
        levelEvent.promptText = "You may level up a card from your hand.";

        gameCard.AddEvent(levelEvent);

        TrashSelfEvent trashEvent = new TrashSelfEvent();
        gameCard.AddEvent(trashEvent);
    }

    private void GenerateTrasher(GameCard gameCard, int level)
    {
        gameCard.Title = "Trasher";
        gameCard.AbilityText = "Action. Draw 1 card. Gain 1 Action. May trash (remove from game) 1 card from hand.";
        gameCard.cardType = GameCard.CardType.Action;

        gameCard.actionsAdded = 1;

        DrawCardsEvent drawEvent = new DrawCardsEvent();
        drawEvent.numCards = 1;
        drawEvent.drawToZone = CardContainer.CardZone.Hand;

        gameCard.AddEvent(drawEvent);

        TrashSelectedCardsEvent trashEvent = new TrashSelectedCardsEvent();
        trashEvent.numCards = 1;
        trashEvent.numRequiredCards = 0;
        trashEvent.promptText = "You may trash a card from your hand.";

        gameCard.AddEvent(trashEvent);

    }

    private void GenerateSpite(GameCard gameCard, int level)
    {
        gameCard.Title = "Spite";
        gameCard.AbilityText = "Gain " + level + " Spite.";
        gameCard.cardType = GameCard.CardType.Spite;
        gameCard.spiteAdded = level;
        gameCard.BaseDamage = 1;
        gameCard.CurrentDamage = 1;

        DealDamageEvent baseDmgEvent = ScriptableObject.CreateInstance<DealDamageEvent>();
        baseDmgEvent.damageTypeSource = DealDamageEvent.ValueSource.Card;
        baseDmgEvent.damageAmountSource = DealDamageEvent.ValueSource.Card;
        gameCard.AddEvent(baseDmgEvent);
    }

    private static void GenerateConfusionAttack(GameCard gameCard, int level)
    {
        gameCard.Title = "Confusion";        
        gameCard.cardType = GameCard.CardType.Argument;
        gameCard.damageType = GameCard.DamageType.Confusion;
        gameCard.BaseDamage = level + 2;
        gameCard.CurrentDamage = gameCard.BaseDamage;
        gameCard.spiteUsed = level;
        gameCard.AbilityText = "Argument. Uses " + gameCard.spiteUsed + " Spite. Reveal top card of deck. If it is an Argument, put it into play for free. Otherwise, discard it.";

        DealDamageEvent baseDmgEvent = ScriptableObject.CreateInstance<DealDamageEvent>();
        baseDmgEvent.damageTypeSource = DealDamageEvent.ValueSource.Card;
        baseDmgEvent.damageAmountSource = DealDamageEvent.ValueSource.Card;
        gameCard.AddEvent(baseDmgEvent);

        ConfusionArgumentEvent confEvent = ScriptableObject.CreateInstance<ConfusionArgumentEvent>();
        gameCard.AddEvent(confEvent);


    }

    private static void GenerateAngerAttack(GameCard gameCard, int level)
    {
        gameCard.Title = "Anger";        
        gameCard.cardType = GameCard.CardType.Argument;
        gameCard.damageType = GameCard.DamageType.Anger;
        gameCard.BaseDamage = level;
        gameCard.CurrentDamage = gameCard.BaseDamage;
        gameCard.spiteUsed = level + 1;
        gameCard.AbilityText = "Argument. Uses " + gameCard.spiteUsed + " Spite. Do bonus damage equal to Total Spite when played.";

        DealDamageEvent baseDmgEvent = ScriptableObject.CreateInstance<DealDamageEvent>();
        baseDmgEvent.damageTypeSource = DealDamageEvent.ValueSource.Card;
        baseDmgEvent.damageAmountSource = DealDamageEvent.ValueSource.Card;
        gameCard.AddEvent(baseDmgEvent);

        AngerDamageEvent angDmgEvent = ScriptableObject.CreateInstance<AngerDamageEvent>();
        gameCard.AddEvent(angDmgEvent);
    }

    private static void GenerateFatigueAttack(GameCard gameCard, int level)
    {
        gameCard.Title = "Fatigue";        
        gameCard.cardType = GameCard.CardType.Argument;
        gameCard.damageType = GameCard.DamageType.Fatigue;
        gameCard.BaseDamage = level + 1;
        gameCard.CurrentDamage = gameCard.BaseDamage;
        gameCard.spiteUsed = level;
        gameCard.AbilityText = "Argument. Uses " + gameCard.spiteUsed + " Spite. Draw a card.";

        DealDamageEvent baseDmgEvent = ScriptableObject.CreateInstance<DealDamageEvent>();
        baseDmgEvent.damageTypeSource = DealDamageEvent.ValueSource.Card;
        baseDmgEvent.damageAmountSource = DealDamageEvent.ValueSource.Card;
        gameCard.AddEvent(baseDmgEvent);

        //DealCardDamageEvent cardDamageEvent = new DealCardDamageEvent();
        //gameCard.AddEvent(cardDamageEvent);

        DrawCardsEvent drawEvent = ScriptableObject.CreateInstance<DrawCardsEvent>();
        drawEvent.numCards = 1;
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
