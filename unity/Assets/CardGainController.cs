using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardGainController : MonoBehaviour {

    public static CardGainController cardGainController;
	// Use this for initialization
	void Start () {
        cardGainController = this;

        battleWonCards.Add(new CardDefinition(CardFactory.CardName.AngerAttack, 2));
        battleWonCards.Add(new CardDefinition(CardFactory.CardName.FatigueAttack, 2));
        battleWonCards.Add(new CardDefinition(CardFactory.CardName.ConfusionAttack, 2));
        battleWonCards.Add(new CardDefinition(CardFactory.CardName.Spite, 2));
	}

    private List<CardDefinition> battleWonCards = new List<CardDefinition>();    
	
	// Update is called once per frame
	void Update () {
	
	}

    public void GainBattleWonCard()
    {
        CardDisplayController cdc = CardDisplayController.cardDisplayController;
        cdc.CurrentDisplayMode = CardDisplayController.DisplayMode.Selection;
        cdc.numCardsToSelect = 1;

        cdc.showOKButton = true;
        cdc.OKButtonLabel.text = "No Thanks";

        foreach (CardDefinition cardDef in battleWonCards)
        {
            GameObject newCard = CardFactory.cardFactory.CreateCard(cardDef, CardZoneManager.cardZoneManager.displayContainer);

            cdc.DisplayCard(newCard, CardContainer.CardZone.None);
        }
        GameMessageManager.gameMessageManager.AddLine("Gain a new card by dragging it to the Discard Pile.", false);
    }    

}
