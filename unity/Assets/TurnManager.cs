﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurnManager : MonoBehaviour {

    public static TurnManager turnManager;

    public enum TurnState
    {
        OutOfBattle,        
        EnemyTurn,
        PlayerInactive,
        PlayerDraw,
        PlayerActive,        
        PlayerCleanup,    
        StartBattle
    }
    
    private TurnState currentTurnState = TurnState.OutOfBattle;
    public TurnState CurrentState { get { return currentTurnState; } }

    public bool CanPlayCards()
    {
        return CurrentState == TurnState.PlayerActive;
    }

	// Use this for initialization
	void Start () {
        turnManager = this;	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ChangeState(TurnState newState)
    {
        switch (newState)
        {
            case TurnState.OutOfBattle:
                DoOutOfBattle();
                break;
            case TurnState.EnemyTurn:
                DoEnemyTurn();
                break;
            case TurnState.PlayerInactive:
                DoPlayerInactive();
                break;
            case TurnState.PlayerDraw:
                DoPlayerDraw();
                break;
            case TurnState.PlayerActive:
                DoPlayerActive();
                break;
            case TurnState.PlayerCleanup:
                DoPlayerCleanup();
                break;
            case TurnState.StartBattle:
                DoStartBattle();
                break;
            default:
                break;
        }
    }

    private void DoStartBattle()
    {
        currentTurnState = TurnState.StartBattle;
        BattleManager.battleManager.StartNewBattle();

        ChangeState(TurnState.PlayerDraw);
    }

    private void DoPlayerCleanup()
    {
        if (currentTurnState == TurnState.PlayerActive)
        {
            currentTurnState = TurnState.PlayerCleanup;

            CleanupCardsInPlay();

            ChangeState(TurnState.EnemyTurn);
        }
    }

    private static void CleanupCardsInPlay()
    {
        List<CardController> cardsInPlay = CardZoneManager.cardZoneManager.GetCardsInZone(CardContainer.CardZone.Play);

        UIGrid playGrid = CardZoneManager.cardZoneManager.playContainer.GetComponent<UIGrid>();
        UIGrid discardGrid = CardZoneManager.cardZoneManager.discardContainer.GetComponent<UIGrid>();

        foreach (CardController card in cardsInPlay)
        {
            card.gameObject.transform.parent = CardZoneManager.cardZoneManager.discardContainer.transform;
            card.CurrentZone = CardContainer.CardZone.Discard;

            GameCard gameCard = card.gameCard;
            if (gameCard != null)
            {
                if (gameCard.cardDefinition != null)
                {
                    DeckManager.deckManager.AddCardToDiscard(gameCard.cardDefinition);
                }
            }

            NGUITools.MarkParentAsChanged(card.gameObject);
        }

        if (playGrid != null) { playGrid.repositionNow = true; }
        if (discardGrid != null) { discardGrid.repositionNow = true; }
    }

    private static void DiscardHand()
    {
        List<CardController> cardsInHand = CardZoneManager.cardZoneManager.GetCardsInZone(CardContainer.CardZone.Hand);

        UIGrid handGrid = CardZoneManager.cardZoneManager.handContainer.GetComponent<UIGrid>();
        UIGrid discardGrid = CardZoneManager.cardZoneManager.discardContainer.GetComponent<UIGrid>();

        foreach (CardController card in cardsInHand)
        {
            card.gameObject.transform.parent = CardZoneManager.cardZoneManager.discardContainer.transform;
            card.CurrentZone = CardContainer.CardZone.Discard;

            GameCard gameCard = card.gameCard;
            if (gameCard != null)
            {
                if (gameCard.cardDefinition != null)
                {
                    DeckManager.deckManager.AddCardToDiscard(gameCard.cardDefinition);
                }
            }

            NGUITools.MarkParentAsChanged(card.gameObject);
        }

        if (handGrid != null) { handGrid.repositionNow = true; }
        if (discardGrid != null) { discardGrid.repositionNow = true; }
    }

    private void DoPlayerActive()
    {
        if (currentTurnState == TurnState.PlayerDraw ||
            currentTurnState == TurnState.PlayerInactive)
        {
            currentTurnState = TurnState.PlayerActive;
        }
    }

    private void DoPlayerDraw()
    {
        if (currentTurnState == TurnState.EnemyTurn ||
            currentTurnState == TurnState.StartBattle)
        {
            //do player draw phase
            currentTurnState = TurnState.PlayerDraw;
            DrawPileController.drawPile.DrawToFullHand();
            ChangeState(TurnState.PlayerActive);

            //set a new turn
            RulesManager.rulesManager.ResetTurn();
        }
    }

    private void DoPlayerInactive()
    {
        if (currentTurnState == TurnState.PlayerActive)
        {
            currentTurnState = TurnState.PlayerInactive;
        }
    }

    private void DoEnemyTurn()
    {
        if (currentTurnState == TurnState.PlayerCleanup)
        {
            if (BattleManager.battleManager.IsEnemyAlive())
            {
                currentTurnState = TurnState.EnemyTurn;
                ChangeState(TurnState.PlayerDraw);
            }
            else
            {
                ChangeState(TurnState.OutOfBattle);
            }    
        }
    }

    private void DoOutOfBattle()
    {
        currentTurnState = TurnState.OutOfBattle;


        DiscardHand();
        DeckManager.deckManager.ShuffleDiscardIntoDeck();
    }

}
