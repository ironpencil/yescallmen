using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RulesManager : MonoBehaviour {

    public static RulesManager rulesManager;

    private List<CardFactory.CardName> restrictedCards = new List<CardFactory.CardName>();

	// Use this for initialization
	void Start () {

        rulesManager = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public bool CanPlayCard(GameObject card)
    {
        GameCard gameCard = card.GetComponent<GameCard>();

        if (gameCard == null) { return false; }

        Debug.Log("Restricted cards: " + restrictedCards.ToString());
        if (restrictedCards.Contains(gameCard.cardDefinition.CardName))
        {
            return false;
        }

        return true;
    }

    public void PlayCard(GameObject card)
    {
        GameCard gameCard = card.GetComponent<GameCard>();

        if (gameCard == null) { return; }

        switch (gameCard.cardDefinition.CardName)
        {
            case CardFactory.CardName.FatigueAttack:
                if (restrictedCards.Count == 0)
                {
                    restrictedCards.Add(CardFactory.CardName.ConfusionAttack);
                    restrictedCards.Add(CardFactory.CardName.AngerAttack);
                }
                break;
            case CardFactory.CardName.ConfusionAttack:
                if (restrictedCards.Count == 0)
                {
                restrictedCards.Add(CardFactory.CardName.AngerAttack);
                restrictedCards.Add(CardFactory.CardName.FatigueAttack);
                }
                break;
            case CardFactory.CardName.AngerAttack:
                if (restrictedCards.Count == 0)
                {
                    restrictedCards.Add(CardFactory.CardName.FatigueAttack);
                    restrictedCards.Add(CardFactory.CardName.ConfusionAttack);
                }
                break;
            default:
                break;
        }
    }

    public void ResetTurn()
    {
        restrictedCards.Clear();
    }
}
