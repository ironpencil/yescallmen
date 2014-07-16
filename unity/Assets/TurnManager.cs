using UnityEngine;
using System.Collections;

public class TurnManager : MonoBehaviour {

    public enum TurnState
    {
        OutOfBattle,        
        EnemyTurn,
        PlayerInactive,
        PlayerDraw,
        PlayerAction,        
        PlayerCleanup,        
    }

    private TurnState currentTurnState = TurnState.OutOfBattle;

	// Use this for initialization
	void Start () {
	
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
            case TurnState.PlayerAction:
                DoPlayerAction();
                break;
            case TurnState.PlayerCleanup:
                DoPlayerCleanup();
                break;
            default:
                break;
        }
    }

    private void DoPlayerCleanup()
    {
        throw new System.NotImplementedException();
    }

    private void DoPlayerAction()
    {
        if (currentTurnState == TurnState.PlayerDraw ||
            currentTurnState == TurnState.PlayerInactive)
        {
            currentTurnState = TurnState.PlayerAction;
        }
    }

    private void DoPlayerDraw()
    {
        if (currentTurnState == TurnState.EnemyTurn ||
            currentTurnState == TurnState.OutOfBattle)
        {
            //do player draw phase
            currentTurnState = TurnState.PlayerDraw;
            DrawPileController.drawPile.DrawToFullHand();
            ChangeState(TurnState.PlayerAction);
        }
    }

    private void DoPlayerInactive()
    {
        throw new System.NotImplementedException();
    }

    private void DoEnemyTurn()
    {
        throw new System.NotImplementedException();
    }

    private void DoOutOfBattle()
    {
        throw new System.NotImplementedException();
    }

}
