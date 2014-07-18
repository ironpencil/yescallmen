using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RulesManager : MonoBehaviour {

    public static RulesManager rulesManager;

    public UILabel spiteTotalLabel;
    public UILabel spiteLeftLabel;
    public UILabel actionsLeftLabel;

    private int spiteTotal = 0;
    public int SpiteTotal
    {
        get
        {
            return spiteTotal;
        }
        set
        {
            spiteTotal = value;
            spiteTotalLabel.text = SpiteTotal.ToString();
        }
    }

    private int spiteLeft = 0;
    public int SpiteLeft
    {
        get
        {
            return spiteLeft;
        }
        set
        {
            spiteLeft = value;
            spiteLeftLabel.text = SpiteLeft.ToString();
        }
    }

    private int actionsLeft = 1;
    public int ActionsLeft
    {
        get
        {
            return actionsLeft;
        }
        set
        {
            actionsLeft = value;
            actionsLeftLabel.text = ActionsLeft.ToString();
        }
    }

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

        if (gameCard.cardType == GameCard.CardType.Action && ActionsLeft == 0) { return false; }

        //if (gameCard.spiteUsed > SpiteLeft) { return false; }

        return true;
    }

    public void PlayCard(GameObject card)
    {
        CardController cardController = card.GetComponent<CardController>();

        if (cardController == null) { return; }

        GameCard gameCard = cardController.gameCard;

        if (gameCard == null) { return; }

        if (gameCard.cardType == GameCard.CardType.Action)
        {
            ActionsLeft--;
        }

        SpiteTotal += gameCard.spiteAdded;
        SpiteLeft += gameCard.spiteAdded;

        SpiteLeft -= gameCard.spiteUsed;

        cardController.UpdateCurrentZone();

        CardEventManager.cardEventManager.QueueEvents(card);

    }

    public void ResetTurn()
    {
        ActionsLeft = 1;
        SpiteTotal = 0;
        SpiteLeft = 0;
    }
}
