using UnityEngine;
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
        StartBattle,
        EndShow
    }
    
    private TurnState currentTurnState = TurnState.OutOfBattle;
    public TurnState CurrentState { get { return currentTurnState; } }

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
            case TurnState.EndShow:
                DoEndShow();
                break;
            default:
                break;
        }
    }

    private void DoEndShow()
    {
        currentTurnState = TurnState.EndShow;

        Globals.GetInstance().SaveSession();

        Application.LoadLevel("outofbattle");
        //end the show, clean up, go back to outofbattle scene

    }

    private void DoStartBattle()
    {
        Globals.GetInstance().PlayerFinishedLastShow = false;

        currentTurnState = TurnState.StartBattle;
        BattleManager.battleManager.StartNewBattle();

        GameMessageManager.gameMessageManager.AddLine("How to Play: Play cards by dragging them to the play area.", false);
        GameMessageManager.gameMessageManager.AddLine("After each turn, all cards in play and in hand will be sent to the Discard Pile.", false);
        GameMessageManager.gameMessageManager.AddLine("When you have to draw a card and there are none in the Draw Pile, Discard Pile will be shuffled to make a new Draw Pile.", false);
        GameMessageManager.gameMessageManager.AddLine("Spite cards can be played for free to increase Spite.", false);
        GameMessageManager.gameMessageManager.AddLine("Argument cards use Spite when played. Action cards use Actions when played.", false);
        GameMessageManager.gameMessageManager.AddLine("Any changes you make to your deck during play by Trashing (removing) or Gaining (adding) cards are permanent, and your deck will persist across battles.", false);

        ChangeState(TurnState.PlayerDraw);
    }

    private void DoPlayerCleanup()
    {
        if (currentTurnState == TurnState.PlayerActive)
        {
            currentTurnState = TurnState.PlayerCleanup;

            DiscardHand();
            CleanupCardsInPlay();

            GameMessageManager.gameMessageManager.SetText("", true);

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
                    DeckManager.deckManager.AddCardToDiscard(gameCard.cardDefinition, gameCard.isGainedCard);
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
                    DeckManager.deckManager.AddCardToDiscard(gameCard.cardDefinition, gameCard.isGainedCard);
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
            if (BattleManager.battleManager.IsPlayerAlive())
            {
                //do player draw phase
                currentTurnState = TurnState.PlayerDraw;
                DrawPileController.drawPile.DrawToFullHand();
                //ChangeState(TurnState.PlayerActive);
                StartCoroutine(ChangeToStateAfterSeconds(TurnState.PlayerActive, 1.5f));

                //set a new turn
                RulesManager.rulesManager.ResetTurn();
            }
            else
            {
                DoBattleLost();
            }
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
                EnemyActionManager.enemyActionManager.TakeTurn();
            }
            else
            {
                DoBattleWon();
            }    
        }
    }

    private void DoBattleWon()
    {
        Globals.GetInstance().PlayerBattlesWon++;
        if (BattleManager.battleManager.BattleNumber >= BattleManager.battleManager.BattlesPerShow)
        {
            //we've finished the show
            GameMessageManager.gameMessageManager.AddLine("\"Looks like we're just about out of time for today's show.\"", false);
            GameMessageManager.gameMessageManager.AddLine("", false);

            Globals.GetInstance().PlayerLevel++;

            CardGainController.cardGainController.GainFinishedShowCard();
            Globals.GetInstance().PlayerFinishedLastShow = true;
            StartCoroutine(ChangeToStateWhenDisplayEmpty(TurnState.EndShow));
        }
        else
        {
            GameMessageManager.gameMessageManager.AddLine("\"Ha, another caller who couldn't take the heat!\"", false);
            GameMessageManager.gameMessageManager.AddLine("", false);

            CardGainController.cardGainController.GainBattleWonCard();
            StartCoroutine(ChangeToStateWhenDisplayEmpty(TurnState.OutOfBattle));
        }
        
    }

    private void DoBattleLost()
    {
        Globals.GetInstance().PlayerBattlesLost++;
        //BattleManager.battleManager.PlayerCurrentAnger = BattleManager.battleManager.PlayerMaxAnger;

        GameMessageManager.gameMessageManager.AddLine("\"I CAN'T HANDLE ANY MORE OF THESE IDIOTS TONIGHT!\"", false);
        GameMessageManager.gameMessageManager.AddLine("", false);

        CardGainController.cardGainController.GainBattleLostCard();
        Globals.GetInstance().PlayerFinishedLastShow = false;
        StartCoroutine(ChangeToStateWhenDisplayEmpty(TurnState.EndShow));        
    }

    private void DoOutOfBattle()
    {
        currentTurnState = TurnState.OutOfBattle;

        GameMessageManager.gameMessageManager.SetText("Are you ready for the next caller?", false);

        DiscardHand();
        DeckManager.deckManager.ShuffleDiscardIntoDeck();

        Globals.GetInstance().SaveSession();
    }

    public IEnumerator ChangeToStateAfterSeconds(TurnState newState, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        ChangeState(newState);
    }

    public IEnumerator ChangeToStateWhenDisplayEmpty(TurnState newState)
    {
        while (CardDisplayController.cardDisplayController.displayedCards.Count > 0)
        {
            yield return new WaitForSeconds(0.25f);
            Debug.Log("Cards left in display: " + CardDisplayController.cardDisplayController.displayedCards.Count);
        }

        Debug.Log("Changing to Out of Battle State");

        ChangeState(newState);
    }

    public bool CanPlayCards()
    {
        return CurrentState == TurnState.PlayerActive;
    }
}
