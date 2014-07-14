using UnityEngine;
using System.Collections;

public class BattleManager : MonoBehaviour {

    public static BattleManager battleManager;

    public int playerMaxAnger = 100;
    public int PlayerMaxAnger
    {
        get { return playerMaxAnger; }
        set
        {
            playerMaxAnger = Mathf.Max(1, value);
            playerAngerLabel.text = PlayerCurrentAnger.ToString() + " / " + PlayerMaxAnger;
        }
    }
    
    public int playerCurrentAnger = 100;
    public int PlayerCurrentAnger
    {
        get { return playerCurrentAnger; }
        set
        {
            playerCurrentAnger = Mathf.Min(PlayerMaxAnger, value);
            playerAngerLabel.text = PlayerCurrentAnger.ToString() + " / " + PlayerMaxAnger;
        }
    }

    public int enemyMaxAnger = 10;
    public int EnemyMaxAnger
    {
        get { return enemyMaxAnger; }
        set
        {
            enemyMaxAnger = Mathf.Max(1, value);
            enemyAngerLabel.text = EnemyCurrentAnger.ToString() + " / " + EnemyMaxAnger;
        }
    }
    
    public int enemyCurrentAnger = 10;
    public int EnemyCurrentAnger
    {
        get { return enemyCurrentAnger; }
        set
        {
            enemyCurrentAnger = Mathf.Min(EnemyMaxAnger, value);
            enemyAngerLabel.text = EnemyCurrentAnger.ToString() + " / " + EnemyMaxAnger;
        }
    }
    
    public int enemyMaxConfusion = 10;
    public int EnemyMaxConfusion
    {
        get { return enemyMaxConfusion; }
        set
        {
            enemyMaxConfusion = Mathf.Max(1, value);
            enemyConfusionLabel.text = EnemyCurrentConfusion.ToString() + " / " + EnemyMaxConfusion;
        }
    }
    
    public int enemyCurrentConfusion = 10;
    public int EnemyCurrentConfusion
    {
        get { return enemyCurrentConfusion; }
        set
        {
            enemyCurrentConfusion = Mathf.Min(EnemyMaxConfusion, value);
            enemyConfusionLabel.text = EnemyCurrentConfusion.ToString() + " / " + EnemyMaxConfusion;
        }
    }
    
    public int enemyMaxFatigue = 10;
    public int EnemyMaxFatigue
    {
        get { return enemyMaxFatigue; }
        set
        {
            enemyMaxFatigue = Mathf.Max(1, value);
            enemyFatigueLabel.text = EnemyCurrentFatigue.ToString() + " / " + EnemyMaxFatigue;
        }
    }
    
    public int enemyCurrentFatigue = 10;
    public int EnemyCurrentFatigue
    {
        get { return enemyCurrentFatigue; }
        set
        {
            enemyCurrentFatigue = Mathf.Min(EnemyMaxFatigue, value);
            enemyFatigueLabel.text = EnemyCurrentFatigue.ToString() + " / " + EnemyMaxFatigue;
        }
    }

    public UILabel playerAngerLabel;
    public UILabel enemyAngerLabel;
    public UILabel enemyConfusionLabel;
    public UILabel enemyFatigueLabel;

	// Use this for initialization
	void Start () {
        battleManager = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}



}
