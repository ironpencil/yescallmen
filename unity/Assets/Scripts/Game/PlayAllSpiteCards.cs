using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayAllSpiteCards : MonoBehaviour {

    private UIButton buttonScript;
	// Use this for initialization
	void Start () {
        buttonScript = gameObject.GetComponent<UIButton>();
	}
	
	// Update is called once per frame
	void Update () {
        if (TurnManager.turnManager.CurrentState == TurnManager.TurnState.PlayerActive &&
            TutorialManager.instance.CanPlayAllSpite)
        {
            if (!buttonScript.isEnabled)
            {
                buttonScript.isEnabled = true;
            }
        }
        else
        {
            if (buttonScript.isEnabled)
            {
                buttonScript.isEnabled = false;
            }
        }	
	}

    public void PlaySpiteCards()
    {
        List<CardController> cardsInHand = CardZoneManager.cardZoneManager.GetCardsInZone(CardContainer.CardZone.Hand);

        foreach (CardController card in cardsInHand)
        {

            if (card.gameCard.cardType == GameCard.CardType.Spite)
            {
                CardZoneManager.cardZoneManager.MoveCardToZone(card.gameObject, CardContainer.CardZone.Play);
                RulesManager.rulesManager.PlayCard(card.gameObject, false);
            }
        }
    }
}
