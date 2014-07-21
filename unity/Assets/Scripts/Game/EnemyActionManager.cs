using UnityEngine;
using System.Collections;

public class EnemyActionManager : MonoBehaviour {

    public static EnemyActionManager enemyActionManager;

    public string EnemyName = "Caller";

    public int Level = 1;

    public int MaxHP = 10;

    public int Damage = 1;    

	// Use this for initialization
	void Start () {
        enemyActionManager = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void TakeTurn()
    {
        GameMessageManager.gameMessageManager.AddLine(EnemyName + " makes their case, raising your anger by " + Damage + "!", false);
        BattleManager.battleManager.DamagePlayer(Damage);
        TurnManager.turnManager.ChangeState(TurnManager.TurnState.PlayerDraw);
    }

    public void NextEnemy(int playerLevel, int battleNumber)
    {
        
        int adjustedLevel = playerLevel + battleNumber - Globals.GetInstance().PlayerBattlesLost;

        Level = Mathf.Max(1, adjustedLevel);

        MaxHP = Level * 10;

        Damage = Level;

    }
}
