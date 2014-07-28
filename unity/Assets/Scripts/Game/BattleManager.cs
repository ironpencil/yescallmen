using UnityEngine;
using System.Collections;

public class BattleManager : MonoBehaviour {

    public static BattleManager battleManager;

    //public int argumentsWon = 0;
    //public int ArgumentsWon
    //{
    //    get
    //    {
    //        return argumentsWon;
    //    }
    //    set
    //    {
    //        argumentsWon = value;

    //        //save the arguments won back to Globals
    //        Globals.GetInstance().PlayerBattlesWon = argumentsWon;
    //    }
    //}

    //public int argumentsLost = 0;
    //public int ArgumentsLost
    //{
    //    get
    //    {
    //        return argumentsLost;
    //    }
    //    set
    //    {
    //        argumentsLost = value;

    //        //save the arguments won back to Globals
    //        Globals.GetInstance().PlayerBattlesLost = argumentsLost;
    //    }
    //}

    public int BattlesPerShow = 2;

    public int battleNumber = 0;
    public int BattleNumber
    {
        get { return battleNumber; }
        set
        {
            battleNumber = value;
            callNumberLabel.text = BattleNumber.ToString();
        }
    }

    public int playerMaxAnger = 100;
    public int PlayerMaxAnger
    {
        get { return playerMaxAnger; }
        set
        {
            playerMaxAnger = Mathf.Max(1, value);
            playerAngerMaxLabel.text = PlayerMaxAnger.ToString();

            //save the max HP change back to Globals
            Globals.GetInstance().PlayerMaxHP = value;
        }
    }
    
    public int playerCurrentAnger = 100;
    public int PlayerCurrentAnger
    {
        get { return playerCurrentAnger; }
        set
        {
            playerCurrentAnger = Mathf.Max(0, value);
            playerAngerCurrentLabel.text = PlayerCurrentAnger.ToString();
        }
    }

    public int enemyMaxHP = 10;
    public int EnemyMaxHP
    {
        get { return enemyMaxHP; }
        set
        {
            enemyMaxHP = Mathf.Max(1, value);
            enemyHPMaxLabel.text = EnemyMaxHP.ToString();
        }
    }

    public int enemyCurrentHP = 10;
    public int EnemyCurrentHP
    {
        get { return enemyCurrentHP; }
        set
        {
            enemyCurrentHP = Mathf.Max(0, value);
            enemyHPCurrentLabel.text = EnemyCurrentHP.ToString();
        }            
    }

    /* old damage types
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
     * */

    public bool IsEnemyAlive()
    {

        return EnemyCurrentHP < EnemyMaxHP;

        //return (EnemyCurrentAnger > 0 &&
        //    EnemyCurrentConfusion > 0 &&
        //    EnemyCurrentFatigue > 0);
    }

    public bool IsPlayerAlive()
    {
        return PlayerCurrentAnger < PlayerMaxAnger;
    }

    public void StartNewBattle()
    {
        //EnemyCurrentAnger = EnemyMaxAnger;
        //EnemyCurrentFatigue = EnemyMaxFatigue;
        //EnemyCurrentConfusion = EnemyMaxConfusion;

        BattleNumber++;

        EnemyActionManager.enemyActionManager.NextEnemy(Globals.GetInstance().PlayerLevel, BattleNumber);

        EnemyMaxHP = EnemyActionManager.enemyActionManager.MaxHP;
        EnemyCurrentHP = 0;        
    }

    public UILabel playerAngerCurrentLabel;
    public UILabel playerAngerMaxLabel;

    public UILabel enemyHPCurrentLabel;
    public UILabel enemyHPMaxLabel;
    
    public UILabel callersPerShowLabel;
    public UILabel callNumberLabel;
    /*old damage typespublic UILabel enemyAngerLabel;
    public UILabel enemyConfusionLabel;
    public UILabel enemyFatigueLabel;
     * */

    public int DamageEnemy(GameCard.DamageType damageType, int value)
    {
        //int newValue = 

        EnemyCurrentHP = EnemyCurrentHP + value;

        /* old damage types
         * switch (damageType)
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
        }*/

        return EnemyCurrentHP;
    }

    public int DamagePlayer(int value)
    {
        PlayerCurrentAnger = PlayerCurrentAnger + value;

        return PlayerCurrentAnger;
    }

	// Use this for initialization
	void Start () {
        battleManager = this;

        /* old damage types
        EnemyMaxAnger = EnemyMaxAnger;
        EnemyCurrentAnger = EnemyMaxAnger;
        EnemyMaxConfusion = EnemyMaxConfusion;
        EnemyCurrentConfusion = EnemyMaxConfusion;
        EnemyMaxFatigue = EnemyMaxFatigue;
        EnemyCurrentFatigue = EnemyMaxFatigue;
         */

        EnemyMaxHP = 0;
        EnemyCurrentHP = 0;

        PlayerMaxAnger = Globals.GetInstance().PlayerMaxHP;
        PlayerCurrentAnger = 0;

        callersPerShowLabel.text = BattlesPerShow.ToString();

        //ArgumentsWon = Globals.GetInstance().PlayerBattlesWon;
        //ArgumentsLost = Globals.GetInstance().PlayerBattlesLost;
	}
	
	// Update is called once per frame
	void Update () {
	
	}



}
