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
            case CardName.IncreaseMaxAnger: //invalid
                GenerateInvalidCard(gameCard, level);
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

        switch (gameCard.cardType)
        {
            case GameCard.CardType.Argument:
                gameCard.cardSprite.color = WhiteCardTint;
                break;
            case GameCard.CardType.Spite:
                gameCard.cardSprite.color = RedCardTint;
                break;
            case GameCard.CardType.Special:
                gameCard.cardSprite.color = GreenCardTint;
                gameCard.Level = 0;
                break;
            case GameCard.CardType.Action:
                gameCard.cardSprite.color = YellowCardTint;
                gameCard.Level = 0;
                break;
            default:
                gameCard.Level = 0;                
                break;
        }

        return gameCard;
    }

    private static void GenerateFreeAttack(GameCard gameCard, int level)
    {
        gameCard.Title = "Fedoras Are Cool";
        gameCard.cardType = GameCard.CardType.Argument;
        gameCard.damageType = GameCard.DamageType.Fatigue;
        gameCard.BaseDamage = level * 5;
        gameCard.CurrentDamage = gameCard.BaseDamage;
        gameCard.spiteUsed = 0;
        gameCard.AbilityText = "Argument\r\n-" + gameCard.spiteUsed + " Spite.\r\n\r\nTrash this card.";

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

        gameCard.Title = "Child Support";
        gameCard.cardType = GameCard.CardType.Argument;
        gameCard.damageType = GameCard.DamageType.Fatigue;
        gameCard.BaseDamage = level * 3;
        gameCard.CurrentDamage = gameCard.BaseDamage;
        gameCard.spiteUsed = level + 3;
        gameCard.AbilityText = "Argument\r\n-" + gameCard.spiteUsed + " Spite.\r\n\r\nGain a Level " + spiteCardLevel + " Spite card,  putting it into your hand.";

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
        gameCard.Title = "Reproductive Rights";
        gameCard.cardType = GameCard.CardType.Argument;
        gameCard.damageType = GameCard.DamageType.Fatigue;
        gameCard.BaseDamage = level * 3;
        gameCard.CurrentDamage = gameCard.BaseDamage;
        gameCard.spiteUsed = level;
        gameCard.AbilityText = "Argument\r\n-" + gameCard.spiteUsed + " Spite.\r\n\r\nIf Current Anger is more than half of Max Anger, do " + bonusDamage + " bonus damage.";

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
        gameCard.Title = "Female Privilege";
        gameCard.AbilityText = "Action\r\n\r\n+3 Cards\r\n+3 Actions\r\n+3 Spite\r\n\r\nTrash this card.";
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
        gameCard.Title = "Political Correctness";
        gameCard.AbilityText = "Action\r\n\r\n+5 Cards\r\n\r\nDiscard 2 cards.";
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
        gameCard.Title = "Spermjacking";
        gameCard.AbilityText = "Action\r\n\r\n+3 Cards\r\n+1 Action\r\n+1 Spite\r\n\r\nDiscard 3 cards.";
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
        gameCard.Title = "The Friendzone";
        gameCard.AbilityText = "Action\r\n\r\n+2 Cards\r\n+1 Action\r\n\r\nMay put 1 card from your hand on top of the deck.";
        gameCard.cardType = GameCard.CardType.Action;

        gameCard.actionsAdded = 1;

        DrawCardsEvent drawEvent = new DrawCardsEvent();
        drawEvent.numCards = 2;
        drawEvent.drawToZone = CardContainer.CardZone.Hand;

        gameCard.AddEvent(drawEvent);

        PutCardsOnDeckEvent deckEvent = new PutCardsOnDeckEvent();
        deckEvent.numCards = 1;
        deckEvent.numRequiredCards = 0;
        deckEvent.promptText = "You may put a card on top of your deck.";

        gameCard.AddEvent(deckEvent);
    }

    private void GenerateInvalidCard(GameCard gameCard, int level)
    {
        gameCard.Title = "Invalid Card";
        gameCard.AbilityText = "Special\r\n\r\nTrash this card.";
        gameCard.cardType = GameCard.CardType.Special;

        TrashSelfEvent trashEvent = new TrashSelfEvent();
        gameCard.AddEvent(trashEvent);
    }

    private void GenerateIncreaseMaxAngerCard(GameCard gameCard, int level)
    {
        gameCard.Title = "Raise Max Anger";
        gameCard.AbilityText = "Special\r\n\r\n+5 Cards\r\n+1 Action\r\n+50 Max Anger\r\n\r\nTrash this card.";
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
        gameCard.Title = "Paternity Fraud";
        gameCard.AbilityText = "Action\r\n\r\n+1 Card\r\n+1 Action\r\n\r\nIf you have 10 or more Total Spite:\r\n+1 Card\r\n+1 Action.";
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
        gameCard.Title = "Feminazis";
        gameCard.AbilityText = "Action\r\n\r\nMay level up 2 cards from hand, then discard them.\r\n\r\nTrash this card.";
        gameCard.cardType = GameCard.CardType.Action;

        LevelSelectedCardsEvent levelEvent = new LevelSelectedCardsEvent();
        levelEvent.numCards = 2;
        levelEvent.numRequiredCards = 0;
        levelEvent.promptText = "You may level up 2 cards, then discard them.";
        levelEvent.cardDestination = CardContainer.CardZone.Discard;
        levelEvent.canCancel = true;

        gameCard.AddEvent(levelEvent);

        TrashSelfEvent trashEvent = new TrashSelfEvent();
        gameCard.AddEvent(trashEvent);
    }

    private void GenerateSmithy(GameCard gameCard, int level)
    {
        gameCard.Title = "Women Are Slutty";
        gameCard.AbilityText = "Action\r\n\r\n+3 Cards\r\n+1 Spite";
        gameCard.cardType = GameCard.CardType.Action;

        gameCard.spiteAdded = 1;

        DrawCardsEvent drawEvent = new DrawCardsEvent();
        drawEvent.numCards = 3;
        drawEvent.drawToZone = CardContainer.CardZone.Hand;

        gameCard.AddEvent(drawEvent);

    }

    private void GenerateVillage(GameCard gameCard, int level)
    {
        gameCard.Title = "Women Are Frigid";
        gameCard.AbilityText = "Action\r\n\r\n+1 Card\r\n+2 Actions\r\n+1 Spite";
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
        gameCard.Title = "The Red Pill";
        gameCard.AbilityText = "Action\r\n\r\nReduce Current Anger by Total Spite.";
        gameCard.cardType = GameCard.CardType.Action;

        ChangeAngerEvent healEvent = new ChangeAngerEvent();
        healEvent.useTotalSpite = true;
        healEvent.amountModifier = -1.0;

        gameCard.AddEvent(healEvent);
    }

    private void GenerateNotAllMen(GameCard gameCard, int level)
    {
        gameCard.Title = "Not All Men!";
        gameCard.AbilityText = "Special\r\n(No Action)\r\n\r\n+5 Cards\r\n+1 Action\r\n+25 Max Anger\r\nLevel up all cards in your hand.\r\nTrash this card.";
        gameCard.cardType = GameCard.CardType.Special;
        gameCard.actionsAdded = 1;

        DrawCardsEvent drawEvent = new DrawCardsEvent();
        drawEvent.numCards = 5;
        drawEvent.drawToZone = CardContainer.CardZone.Hand;

        gameCard.AddEvent(drawEvent);

        ChangeAngerEvent healEvent = new ChangeAngerEvent();
        healEvent.useTotalSpite = false;
        healEvent.angerAmount = 25;
        healEvent.changeMaxAnger = true;

        gameCard.AddEvent(healEvent);

        LevelAllCardsEvent levelEvent = new LevelAllCardsEvent();
        levelEvent.cardSource = CardContainer.CardZone.Hand;

        gameCard.AddEvent(levelEvent);        

        TrashSelfEvent trashEvent = new TrashSelfEvent();
        gameCard.AddEvent(trashEvent);
    }

    private void GenerateTrasher(GameCard gameCard, int level)
    {
        gameCard.Title = "Wallet Rape";
        gameCard.AbilityText = "Action\r\n\r\n+1 Card\r\n+1 Action\r\n\r\nMay Trash 1 card from your hand.\r\nIf you do:\r\n+1 Card\r\n+1 Action";
        gameCard.cardType = GameCard.CardType.Action;

        gameCard.actionsAdded = 1;

        DrawCardsEvent drawEvent = new DrawCardsEvent();
        drawEvent.numCards = 1;
        drawEvent.drawToZone = CardContainer.CardZone.Hand;

        gameCard.AddEvent(drawEvent);

        TrashSelectedCardsEvent trashEvent = new TrashSelectedCardsEvent();
        trashEvent.numCards = 1;
        trashEvent.numRequiredCards = 0;
        trashEvent.promptText = "You may Trash a card from your hand.";

        gameCard.AddEvent(trashEvent);

        TrasherBenefitEvent benefitEvent = new TrasherBenefitEvent();
        benefitEvent.numExtraActions = 1;
        benefitEvent.numExtraCards = 1;

        gameCard.AddEvent(benefitEvent);

    }

    private void GenerateSpite(GameCard gameCard, int level)
    {
        gameCard.Title = "Spite";
        gameCard.AbilityText = "Spite\r\n\r\n+" + level + " Spite";
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
        gameCard.Title = "Circumcision";        
        gameCard.cardType = GameCard.CardType.Argument;
        gameCard.damageType = GameCard.DamageType.Confusion;
        gameCard.BaseDamage = level * 3;
        gameCard.CurrentDamage = gameCard.BaseDamage;
        gameCard.spiteUsed = level;
        gameCard.AbilityText = "Argument\r\n-" + gameCard.spiteUsed + " Spite\r\n\r\nReveal top card of deck. If it is an Argument, play it for free. Otherwise, discard it.";

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

        gameCard.Title = "Lying About Rape";        
        gameCard.cardType = GameCard.CardType.Argument;
        gameCard.damageType = GameCard.DamageType.Anger;
        gameCard.BaseDamage = level;
        gameCard.CurrentDamage = gameCard.BaseDamage;
        gameCard.spiteUsed = level + 1;
        gameCard.AbilityText = "Argument\r\n-" + gameCard.spiteUsed + " Spite\r\n\r\nDo bonus damage equal to Total Spite, up to " + maxBonusDamage + ".";

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
        gameCard.Title = "Divorce Laws";        
        gameCard.cardType = GameCard.CardType.Argument;
        gameCard.damageType = GameCard.DamageType.Fatigue;
        gameCard.BaseDamage = level * 2;
        gameCard.CurrentDamage = gameCard.BaseDamage;
        gameCard.spiteUsed = level;
        gameCard.AbilityText = "Argument\r\n-" + gameCard.spiteUsed + " Spite\r\n\r\n+1 Card\r\n+1 Action";

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

    public Color WhiteCardTint;
    public Color YellowCardTint;
    public Color RedCardTint;
    public Color GreenCardTint;

    public Color BluePen;
    public Color RedPen;

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
