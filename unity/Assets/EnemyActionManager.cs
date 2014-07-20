using UnityEngine;
using System.Collections;

public class EnemyActionManager : MonoBehaviour {

    public static EnemyActionManager enemyActionManager;

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
        BattleManager.battleManager.DamagePlayer(Damage);
        TurnManager.turnManager.ChangeState(TurnManager.TurnState.PlayerDraw);
    }

    public void NextEnemy()
    {
        Level++;
        MaxHP += 2;
        Damage++;
    }
}
