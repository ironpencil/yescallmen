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

    public int DamageEnemy(GameCard.DamageType damageType, int value)
    {
        int newValue = -1;

        switch (damageType)
        {
            case GameCard.DamageType.None:
                break;
            case GameCard.DamageType.Anger:
                EnemyCurrentAnger = EnemyCurrentAnger - value;
                newValue = EnemyCurrentAnger;
                break;
            case GameCard.DamageType.Confusion:
                EnemyCurrentConfusion = EnemyCurrentConfusion - value;
                newValue = EnemyCurrentConfusion;
                break;
            case GameCard.DamageType.Fatigue:
                EnemyCurrentFatigue = EnemyCurrentFatigue - value;
                newValue = EnemyCurrentFatigue;
                break;
            default:
                break;
        }

        return newValue;
    }

    public int DamagePlayer(int value)
    {
        PlayerCurrentAnger = PlayerCurrentAnger - value;

        return PlayerCurrentAnger;
    }

	// Use this for initialization
	void Start () {
        battleManager = this;

        EnemyMaxAnger = EnemyMaxAnger;
        EnemyCurrentAnger = EnemyMaxAnger;
        EnemyMaxConfusion = EnemyMaxConfusion;
        EnemyCurrentConfusion = EnemyMaxConfusion;
        EnemyMaxFatigue = EnemyMaxFatigue;
        EnemyCurrentFatigue = EnemyMaxFatigue;

        PlayerMaxAnger = PlayerMaxAnger;
        PlayerCurrentAnger = PlayerMaxAnger;
	}
	
	// Update is called once per frame
	void Update () {
	
	}



}
