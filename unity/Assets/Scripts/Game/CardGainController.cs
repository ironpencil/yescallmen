using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardGainController : MonoBehaviour {

    public static CardGainController cardGainController;
	// Use this for initialization
	void Start () {
        cardGainController = this;
	} 
	
	// Update is called once per frame
	void Update () {
	
	}

    public void GainBattleWonCard()
    {
        List<CardDefinition> rewardCards = GenerateBattleRewardCards(Globals.GetInstance().PlayerLevel + 1, true);

        DisplayRewardCards(rewardCards);
    }

    public void GainBattleLostCard()
    {
        List<CardDefinition> rewardCards = GenerateBattleRewardCards(Globals.GetInstance().PlayerLevel, true);

        DisplayRewardCards(rewardCards);
    }

    public void GainFinishedShowCard()
    {
        List<CardDefinition> rewardCards = GenerateShowRewardCards(Globals.GetInstance().PlayerLevel, true);

        DisplayRewardCards(rewardCards);
    }

    private List<CardDefinition> GenerateBattleRewardCards(int atLevel, bool allowRares)
    {
        List<CardDefinition> rewardCards = new List<CardDefinition>();

        rewardCards.Add(new CardDefinition(CardFactory.CardName.AngerAttack, atLevel));
        rewardCards.Add(new CardDefinition(CardFactory.CardName.FatigueAttack, atLevel));
        rewardCards.Add(new CardDefinition(CardFactory.CardName.ConfusionAttack, atLevel));
        rewardCards.Add(new CardDefinition(CardFactory.CardName.Spite, atLevel));
        rewardCards.Add(new CardDefinition(CardFactory.CardName.HealCard, atLevel));
        rewardCards.Add(new CardDefinition(CardFactory.CardName.Trasher, atLevel));
        rewardCards.Add(new CardDefinition(CardFactory.CardName.Village, atLevel));
        rewardCards.Add(new CardDefinition(CardFactory.CardName.Smithy, atLevel));
        rewardCards.Add(new CardDefinition(CardFactory.CardName.SpiteChecker, atLevel));
        rewardCards.Add(new CardDefinition(CardFactory.CardName.DblLeveler, atLevel));

        return rewardCards;
    }

    private List<CardDefinition> GenerateShowRewardCards(int atLevel, bool allowRares)
    {
        List<CardDefinition> rewardCards = new List<CardDefinition>();

        rewardCards.Add(new CardDefinition(CardFactory.CardName.NotAllMen, atLevel));
        rewardCards.Add(new CardDefinition(CardFactory.CardName.IncreaseMaxAnger, atLevel));

        return rewardCards;
    }

    private static void DisplayRewardCards(List<CardDefinition> rewardCards)
    {
        CardDisplayController cdc = CardDisplayController.cardDisplayController;
        cdc.CurrentDisplayMode = CardDisplayController.DisplayMode.Selection;
        cdc.numCardsToSelect = 1;

        cdc.showOKButton = true;
        cdc.OKButtonLabel.text = "No Thanks";

        foreach (CardDefinition cardDef in rewardCards)
        {
            GameObject newCard = CardFactory.cardFactory.CreateCard(cardDef, CardZoneManager.cardZoneManager.displayContainer);

            GameCard gameCard = newCard.GetComponent<GameCard>();

            gameCard.isGainedCard = true;

            cdc.DisplayCard(newCard, CardContainer.CardZone.None);
        }
        GameMessageManager.gameMessageManager.AddLine("You may gain a new card by dragging it to the Discard Pile.", false);
    }
}
