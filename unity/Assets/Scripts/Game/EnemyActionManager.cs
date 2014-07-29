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
        //GameMessageManager.gameMessageManager.AddLine(EnemyName + " makes their case, raising your anger by " + Damage + "!", false);
        GameMessageManager.gameMessageManager.AddLine(">> " + EnemyName + " " + MRAManager.instance.GetCallerArgument() + " (Anger increased by " + Damage + ")", false,
            GameMessageManager.Speaker.Caller);

        BattleManager.battleManager.DamagePlayer(Damage);
        TurnManager.turnManager.ChangeState(TurnManager.TurnState.PlayerDraw);
        //StartCoroutine(TurnManager.turnManager.ChangeToStateWhenMessageFinished(TurnManager.TurnState.PlayerDraw, Globals.GetInstance().LONG_DISPLAY_TIME));
    }

    public void NextEnemy(int playerLevel, int battleNumber)
    {
        int damageMultiplier = 2; // (playerLevel > 2) ? 3 : 2;
        
        int adjustedLevel = playerLevel + battleNumber - 1 - Globals.GetInstance().PlayerBattlesLost;

        Level = Mathf.Max(1, adjustedLevel);

        MaxHP = Level * (20);

        Damage = Level * damageMultiplier;

    }
}
