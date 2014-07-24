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
        Leveler,
        SpiteChecker,
        IncreaseMaxAnger,
        Lab,
        Warehouse,
        Embassy,
        Jack,
        HealthAttack,
        GainerAttack,
        FreeAttack
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

        //clear events in case this was an existing card
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
                GenerateSmithy(gameCard, level);
                break;
            case CardName.Leveler:
                GenerateLeveler(gameCard, level);
                break;
            case CardName.SpiteChecker:
                GenerateSpiteChecker(gameCard, level);
                break;
            case CardName.IncreaseMaxAnger:
                GenerateIncreaseMaxAngerCard(gameCard, level);
                break;
            case CardName.Lab:
                GenerateLab(gameCard, level);
                break;
            case CardName.Warehouse:
                GenerateWarehouse(gameCard, level);
                break;
            case CardName.Embassy:
                GenerateEmbassy(gameCard, level);
                break;
            case CardName.Jack:
                GenerateJack(gameCard, level);
                break;
            case CardName.HealthAttack:
                GenerateHealthAttack(gameCard, level);
                break;
            case CardName.GainerAttack:
                GenerateGainerAttack(gameCard, level);
                break;
            case CardName.FreeAttack:
                GenerateFreeAttack(gameCard, level);
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

    private static void GenerateFreeAttack(GameCard gameCard, int level)
    {
        gameCard.Title = "Free Arg";
        gameCard.cardType = GameCard.CardType.Argument;
        gameCard.damageType = GameCard.DamageType.Fatigue;
        gameCard.BaseDamage = level * 5;
        gameCard.CurrentDamage = gameCard.BaseDamage;
        gameCard.spiteUsed = 0;
        gameCard.AbilityText = "Argument. -" + gameCard.spiteUsed + " Spite.\r\nTrash this card when played.";

        DealDamageEvent baseDmgEvent = ScriptableObject.CreateInstance<DealDamageEvent>();
        baseDmgEvent.damageTypeSource = DealDamageEvent.ValueSource.Card;
        baseDmgEvent.damageAmountSource = DealDamageEvent.ValueSource.Card;
        gameCard.AddEvent(baseDmgEvent);

        TrashSelfEvent trashEvent = new TrashSelfEvent();
        gameCard.AddEvent(trashEvent);

    }

    private static void GenerateGainerAttack(GameCard gameCard, int level)
    {
        int spiteCardLevel = level+1;

        gameCard.Title = "Gainer Arg";
        gameCard.cardType = GameCard.CardType.Argument;
        gameCard.damageType = GameCard.DamageType.Fatigue;
        gameCard.BaseDamage = level * 3;
        gameCard.CurrentDamage = gameCard.BaseDamage;
        gameCard.spiteUsed = level + 3;
        gameCard.AbilityText = "Argument. -" + gameCard.spiteUsed + " Spite.\r\nGain a Level " + spiteCardLevel + " Spite card, putting it into your hand.";

        DealDamageEvent baseDmgEvent = ScriptableObject.CreateInstance<DealDamageEvent>();
        baseDmgEvent.damageTypeSource = DealDamageEvent.ValueSource.Card;
        baseDmgEvent.damageAmountSource = DealDamageEvent.ValueSource.Card;
        gameCard.AddEvent(baseDmgEvent);

        GainCardEvent gainEvent = ScriptableObject.CreateInstance<GainCardEvent>();
        gainEvent.cardToGain = new CardDefinition(CardName.Spite, spiteCardLevel);
        gainEvent.addToZone = CardContainer.CardZone.Hand;

        gameCard.AddEvent(gainEvent);

    }

    private static void GenerateHealthAttack(GameCard gameCard, int level)
    {
        int bonusDamage = level * 3;
        gameCard.Title = "HealthCheckArg";
        gameCard.cardType = GameCard.CardType.Argument;
        gameCard.damageType = GameCard.DamageType.Fatigue;
        gameCard.BaseDamage = level * 3;
        gameCard.CurrentDamage = gameCard.BaseDamage;
        gameCard.spiteUsed = level;
        gameCard.AbilityText = "Argument. -" + gameCard.spiteUsed + " Spite.\r\nIf Current Anger is over half of Max Anger, do " + bonusDamage + " bonus damage.";

        DealDamageEvent baseDmgEvent = ScriptableObject.CreateInstance<DealDamageEvent>();
        baseDmgEvent.damageTypeSource = DealDamageEvent.ValueSource.Card;
        baseDmgEvent.damageAmountSource = DealDamageEvent.ValueSource.Card;
        gameCard.AddEvent(baseDmgEvent);

        HealthCheckDamageEvent bonusDmgEvent = ScriptableObject.CreateInstance<HealthCheckDamageEvent>();
        bonusDmgEvent.bonusDamage = bonusDamage;
        bonusDmgEvent.maxAngerPercent = 0.5;
        gameCard.AddEvent(bonusDmgEvent);
        
    }

    private void GenerateJack(GameCard gameCard, int level)
    {
        gameCard.Title = "Jack";
        gameCard.AbilityText = "Action. +3 Cards, +3 Actions, +3 Spite. Trash this card when played.";
        gameCard.cardType = GameCard.CardType.Action;

        gameCard.actionsAdded = 3;
        gameCard.spiteAdded = 3;

        DrawCardsEvent drawEvent = new DrawCardsEvent();
        drawEvent.numCards = 3;
        drawEvent.drawToZone = CardContainer.CardZone.Hand;

        gameCard.AddEvent(drawEvent);

        TrashSelfEvent trashEvent = new TrashSelfEvent();
        gameCard.AddEvent(trashEvent);
    }

    private void GenerateEmbassy(GameCard gameCard, int level)
    {
        gameCard.Title = "Embassy";
        gameCard.AbilityText = "Action. +5 Cards. Must then discard 2 cards.";
        gameCard.cardType = GameCard.CardType.Action;

        DrawCardsEvent drawEvent = new DrawCardsEvent();
        drawEvent.numCards = 5;
        drawEvent.drawToZone = CardContainer.CardZone.Hand;

        gameCard.AddEvent(drawEvent);

        DiscardCardsEvent discardEvent = new DiscardCardsEvent();
        discardEvent.numCards = 2;
        discardEvent.numRequiredCards = 2;
        discardEvent.promptText = "You must discard 2 cards.";

        gameCard.AddEvent(discardEvent);
    }

    private void GenerateWarehouse(GameCard gameCard, int level)
    {
        gameCard.Title = "Warehouse";
        gameCard.AbilityText = "Action. +3 Cards, +1 Action, +1 Spite. Must then discard 3 cards.";
        gameCard.cardType = GameCard.CardType.Action;

        gameCard.actionsAdded = 1;
        gameCard.spiteAdded = 1;

        DrawCardsEvent drawEvent = new DrawCardsEvent();
        drawEvent.numCards = 3;
        drawEvent.drawToZone = CardContainer.CardZone.Hand;

        gameCard.AddEvent(drawEvent);

        DiscardCardsEvent discardEvent = new DiscardCardsEvent();
        discardEvent.numCards = 3;
        discardEvent.numRequiredCards = 3;
        discardEvent.promptText = "You must discard 3 cards.";

        gameCard.AddEvent(discardEvent);
    }

    private void GenerateLab(GameCard gameCard, int level)
    {
        gameCard.Title = "Lab";
        gameCard.AbilityText = "Action. +2 Cards, +1 Action. May put 1 card from hand on top of deck.";
        gameCard.cardType = GameCard.CardType.Action;

        gameCard.actionsAdded = 1;

        DrawCardsEvent drawEvent = new DrawCardsEvent();
        drawEvent.numCards = 2;
        drawEvent.drawToZone = CardContainer.CardZone.Hand;

        gameCard.AddEvent(drawEvent);

        PutCardsOnDeckEvent deckEvent = new PutCardsOnDeckEvent();
        deckEvent.numCards = 1;
        deckEvent.numRequiredCards = 0;
        deckEvent.promptText = "Put a card from hand on top of deck.";

        gameCard.AddEvent(deckEvent);
    }

    private void GenerateIncreaseMaxAngerCard(GameCard gameCard, int level)
    {
        gameCard.Title = "Raise Max Anger";
        gameCard.AbilityText = "Special (No Action). +5 Cards, +1 Action. Raise Max Anger by 50. Trash this card when played.";
        gameCard.cardType = GameCard.CardType.Special;
        gameCard.actionsAdded = 1;

        DrawCardsEvent drawEvent = new DrawCardsEvent();
        drawEvent.numCards = 5;
        drawEvent.drawToZone = CardContainer.CardZone.Hand;

        gameCard.AddEvent(drawEvent);        

        ChangeAngerEvent healEvent = new ChangeAngerEvent();
        healEvent.useTotalSpite = false;
        healEvent.angerAmount = 50;
        healEvent.changeMaxAnger = true;

        gameCard.AddEvent(healEvent);

        TrashSelfEvent trashEvent = new TrashSelfEvent();
        gameCard.AddEvent(trashEvent);
    }

    private void GenerateSpiteChecker(GameCard gameCard, int level)
    {
        gameCard.Title = "Spite Checker";
        gameCard.AbilityText = "Action. +1 Card, +1 Action. If you have 10 or more Total Spite: +1 Card, +1 Action.";
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

    private void GenerateLeveler(GameCard gameCard, int level)
    {
        gameCard.Title = "Remake";
        gameCard.AbilityText = "Action. May level up 2 cards from hand, then discard them. Trash this card when played.";
        gameCard.cardType = GameCard.CardType.Action;

        LevelSelectedCardsEvent levelEvent = new LevelSelectedCardsEvent();
        levelEvent.numCards = 2;
        levelEvent.numRequiredCards = 0;
        levelEvent.promptText = "Level up 2 cards from hand, then discard them.";
        levelEvent.cardDestination = CardContainer.CardZone.Discard;
        levelEvent.canCancel = true;

        gameCard.AddEvent(levelEvent);

        TrashSelfEvent trashEvent = new TrashSelfEvent();
        gameCard.AddEvent(trashEvent);
    }

    private void GenerateSmithy(GameCard gameCard, int level)
    {
        gameCard.Title = "Smithy";
        gameCard.AbilityText = "Action. +3 Cards, +1 Spite.";
        gameCard.cardType = GameCard.CardType.Action;

        gameCard.spiteAdded = 1;

        DrawCardsEvent drawEvent = new DrawCardsEvent();
        drawEvent.numCards = 3;
        drawEvent.drawToZone = CardContainer.CardZone.Hand;

        gameCard.AddEvent(drawEvent);

    }

    private void GenerateVillage(GameCard gameCard, int level)
    {
        gameCard.Title = "Village";
        gameCard.AbilityText = "Action. +1 Card, +2 Actions, +1 Spite.";
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
        gameCard.AbilityText = "Action. Reduce Current Anger by Total Spite.";
        gameCard.cardType = GameCard.CardType.Action;

        ChangeAngerEvent healEvent = new ChangeAngerEvent();
        healEvent.useTotalSpite = true;
        healEvent.amountModifier = -1.0;

        gameCard.AddEvent(healEvent);
    }

    private void GenerateNotAllMen(GameCard gameCard, int level)
    {
        gameCard.Title = "Not All Men";
        gameCard.AbilityText = "Special (No Action).\r\n+5 Cards, +1 Action. Level up all cards in hand. Trash this card when played.";
        gameCard.cardType = GameCard.CardType.Special;
        gameCard.actionsAdded = 1;

        DrawCardsEvent drawEvent = new DrawCardsEvent();
        drawEvent.numCards = 5;
        drawEvent.drawToZone = CardContainer.CardZone.Hand;

        gameCard.AddEvent(drawEvent);

        LevelAllCardsEvent levelEvent = new LevelAllCardsEvent();
        levelEvent.cardSource = CardContainer.CardZone.Hand;

        gameCard.AddEvent(levelEvent);

        TrashSelfEvent trashEvent = new TrashSelfEvent();
        gameCard.AddEvent(trashEvent);
    }

    private void GenerateTrasher(GameCard gameCard, int level)
    {
        gameCard.Title = "Trasher";
        gameCard.AbilityText = "Action. +1 Card, +1 Action. May Trash 1 card from hand. If you do: +1 Card, +1 Action.";
        gameCard.cardType = GameCard.CardType.Action;

        gameCard.actionsAdded = 1;

        DrawCardsEvent drawEvent = new DrawCardsEvent();
        drawEvent.numCards = 1;
        drawEvent.drawToZone = CardContainer.CardZone.Hand;

        gameCard.AddEvent(drawEvent);

        TrashSelectedCardsEvent trashEvent = new TrashSelectedCardsEvent();
        trashEvent.numCards = 1;
        trashEvent.numRequiredCards = 0;
        trashEvent.promptText = "Trash a card from your hand.";

        gameCard.AddEvent(trashEvent);

        TrasherBenefitEvent benefitEvent = new TrasherBenefitEvent();
        benefitEvent.numExtraActions = 1;
        benefitEvent.numExtraCards = 1;

        gameCard.AddEvent(benefitEvent);

    }

    private void GenerateSpite(GameCard gameCard, int level)
    {
        gameCard.Title = "Spite";
        gameCard.AbilityText = "+" + level + " Spite.";
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
        gameCard.BaseDamage = level * 3;
        gameCard.CurrentDamage = gameCard.BaseDamage;
        gameCard.spiteUsed = level;
        gameCard.AbilityText = "Argument. -" + gameCard.spiteUsed + " Spite.\r\nReveal top card of deck. If it is an Argument, play it for free. Otherwise, put it back.";

        DealDamageEvent baseDmgEvent = ScriptableObject.CreateInstance<DealDamageEvent>();
        baseDmgEvent.damageTypeSource = DealDamageEvent.ValueSource.Card;
        baseDmgEvent.damageAmountSource = DealDamageEvent.ValueSource.Card;
        gameCard.AddEvent(baseDmgEvent);

        ConfusionArgumentEvent confEvent = ScriptableObject.CreateInstance<ConfusionArgumentEvent>();
        gameCard.AddEvent(confEvent);


    }

    private static void GenerateAngerAttack(GameCard gameCard, int level)
    {
        int maxBonusDamage = level * 5;

        gameCard.Title = "Anger";        
        gameCard.cardType = GameCard.CardType.Argument;
        gameCard.damageType = GameCard.DamageType.Anger;
        gameCard.BaseDamage = level;
        gameCard.CurrentDamage = gameCard.BaseDamage;
        gameCard.spiteUsed = level + 1;
        gameCard.AbilityText = "Argument. -" + gameCard.spiteUsed + " Spite.\r\nDo bonus damage equal to Total Spite, up to " + maxBonusDamage + ".";

        DealDamageEvent baseDmgEvent = ScriptableObject.CreateInstance<DealDamageEvent>();
        baseDmgEvent.damageTypeSource = DealDamageEvent.ValueSource.Card;
        baseDmgEvent.damageAmountSource = DealDamageEvent.ValueSource.Card;
        gameCard.AddEvent(baseDmgEvent);

        AngerDamageEvent angDmgEvent = ScriptableObject.CreateInstance<AngerDamageEvent>();
        angDmgEvent.maxDamage = maxBonusDamage;
        gameCard.AddEvent(angDmgEvent);
    }

    private static void GenerateFatigueAttack(GameCard gameCard, int level)
    {
        gameCard.Title = "Fatigue";        
        gameCard.cardType = GameCard.CardType.Argument;
        gameCard.damageType = GameCard.DamageType.Fatigue;
        gameCard.BaseDamage = level * 2;
        gameCard.CurrentDamage = gameCard.BaseDamage;
        gameCard.spiteUsed = level;
        gameCard.AbilityText = "Argument. -" + gameCard.spiteUsed + " Spite.\r\n+1 Card, +1 Action.";

        gameCard.actionsAdded = 1;

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
