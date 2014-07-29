using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class Globals : MonoBehaviour
{
    #region globalInstance
    protected static Globals _instance;

    public static Globals GetInstance()
    {
        if (_instance == null)
        {
            _instance = FindObjectOfType<Globals>();

            if (_instance == null)
            {
                _instance = (Instantiate(Resources.Load("Globals")) as GameObject).GetComponent<Globals>();
            }

            if (_instance != null)
            {
                _instance.Initialize();
            }
        }

        return _instance;
    }

    //destroy this object if a Globals instance already exists
    public void Start()
    {

        if (this != GetInstance())
        {
            gameObject.SetActive(false);
            DestroyImmediate(gameObject);
            return;
        }

        UpdateAudioBalance(true);
    }

    public void Update()
    {
        UpdateAudioBalance(false);
    }


    public void Initialize()
    {
        DontDestroyOnLoad(gameObject);

        DEFAULT_PLAYER_MAX_HP = PlayerMaxHP;

        if (DoLoadPlayerPrefs)
        {
            Debug.Log("DoLoadPlayerPrefs = true");
            LoadSession();
        }
    }

    #endregion    

    #region persistence
    public void LoadSession()
    {
        PlayerMaxHP = PlayerPrefs.GetInt(KEY_PLAYER_MAX_HP, PlayerMaxHP);
        PlayerLevel = PlayerPrefs.GetInt(KEY_PLAYER_LEVEL, PlayerLevel);
        PlayerBattlesWon = PlayerPrefs.GetInt(KEY_PLAYER_BATTLES_WON, PlayerBattlesWon);
        PlayerBattlesLost = PlayerPrefs.GetInt(KEY_PLAYER_BATTLES_LOST, PlayerBattlesLost);

        Debug.Log("HasKey(" + KEY_DECK_CONTENTS + ") = " + PlayerPrefs.HasKey(KEY_DECK_CONTENTS).ToString());
        string deckString = PlayerPrefs.GetString(KEY_DECK_CONTENTS);
        Debug.Log("deckString = " + deckString);
        

        if (!deckString.Equals(""))
        {
            Debug.Log("Load DeckString: " + deckString);

            List<CardDefinition> loadedDeck = LoadDeckFromString(deckString);

            if (loadedDeck != null) { DeckContents = loadedDeck; }
            //byte[] bytes = System.Text.Encoding.UTF8.GetBytes(deckString);

            //using (MemoryStream stream = new MemoryStream(bytes))
            //{
            //    BinaryFormatter bin = new BinaryFormatter();

            //    DeckContents = (List<CardDefinition>)bin.Deserialize(stream);
            //}


        }

        Debug.Log("Deck Size = " + DeckContents.Count);

        CalculateFeministsConverted();
    }

    private List<CardDefinition> LoadDeckFromString(string deckString)
    {
        List<CardDefinition> deck = null;
        try
        {
            if (!deckString.Equals(""))
            {
                byte[] bytes = System.Convert.FromBase64String(deckString);
                using (MemoryStream stream = new MemoryStream(bytes))
                {
                    BinaryFormatter bin = new BinaryFormatter();

                    deck = (List<CardDefinition>)bin.Deserialize(stream);
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log("Error Loading Deck: " + e.Message);
        }

        return deck;
    }

    
    private string SaveDeckToString(List<CardDefinition> deckContents)
    {
        string deckString = "";
        try
        {

            //Debug.Log("Current Directory = " + Directory.GetCurrentDirectory());
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter bin = new BinaryFormatter();
                bin.Serialize(stream, deckContents);

                deckString = System.Convert.ToBase64String(stream.ToArray());
            }
        }
        catch (Exception e)
        {
            Debug.Log("Error Saving Deck: " + e.Message);
        }

        return deckString;
    }

    [ContextMenu("Save Session")]
    public void SaveSession()
    {
        PlayerPrefs.SetInt(KEY_PLAYER_MAX_HP, PlayerMaxHP);
        PlayerPrefs.SetInt(KEY_PLAYER_LEVEL, PlayerLevel);
        PlayerPrefs.SetInt(KEY_PLAYER_BATTLES_WON, PlayerBattlesWon);
        PlayerPrefs.SetInt(KEY_PLAYER_BATTLES_LOST, PlayerBattlesLost);

        string deckString = SaveDeckToString(DeckContents);

        PlayerPrefs.SetString(KEY_DECK_CONTENTS, deckString);    
        
        PlayerPrefs.Save();
    }

    public bool DoesSaveDataExist()
    {
        return PlayerPrefs.HasKey(KEY_PLAYER_LEVEL);
    }

    public void ClearSaveData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        ResetAllValuesToDefault();
    }

    public void ResetAllValuesToDefault()
    {
        PlayerMaxHP = DEFAULT_PLAYER_MAX_HP;
        DeckContents = new List<CardDefinition>();
        PlayerLevel = 1;
        PlayerBattlesLost = 0;
        PlayerBattlesWon = 0;

        CalculateFeministsConverted();
    }
    #endregion

    #region audio

    public float InitialAudioVolume = 0.5f;

    public AudioSource AudioSource1;
    public AudioSource AudioSource2;

    private float audioBalance = 1.0f;
    public float TargetAudioBalance = 1.0f;

    public static float FULL_LINES_BALANCE = 1.0F;
    public static float SIMPLE_LINES_BALANCE = 0.0f;

    public float AudioBalanceSpeed = 10.0f;

    private void UpdateAudioBalance(bool force)
    {
        if (force || !Mathf.Approximately(audioBalance, TargetAudioBalance))
        {
            audioBalance = Mathf.MoveTowards(audioBalance, TargetAudioBalance,
                AudioBalanceSpeed * Time.deltaTime);

            AudioSource1.volume = audioBalance;
            AudioSource2.volume = 1.0f - audioBalance;
        }
    }

    [ContextMenu("Start Music")]
    public void StartMusic()
    {
        AudioListener.volume = Globals.GetInstance().InitialAudioVolume;
        Globals.GetInstance().AudioSource1.Play();
        Globals.GetInstance().AudioSource2.Play();
    }


    #endregion

    

    private const string KEY_PLAYER_MAX_HP = "playerMaxHP";
    private const string KEY_DECK_CONTENTS = "deckContents";
    private const string KEY_PLAYER_LEVEL = "playerLevel";
    private const string KEY_PLAYER_BATTLES_WON = "playerBattlesWon";
    private const string KEY_PLAYER_BATTLES_LOST = "playerBattlesLost";
    private const string KEY_PLAYER_TOTAL_SHOWS = "playerTotalShows";

    public float SHORT_DISPLAY_TIME = 0.2f;
    public float LONG_DISPLAY_TIME = 1.0f;


    public int DEFAULT_PLAYER_MAX_HP = 100;
    
    public int PlayerMaxHP = 100;
    public int PlayerLevel = 1;
    public int PlayerBattlesWon = 0;
    public int PlayerBattlesLost = 0;

    public int PlayerShowsCompleted { get { return PlayerLevel + PlayerBattlesLost - 1; } }

    public long PlayerFeministsConverted = 0;

    public enum FeministsConvertedUnit
    {
        Feminists,
        Planets,
        Galaxies,
        Universes,
        Multiverses,
        Beyond
    }

    public FeministsConvertedUnit PlayerConvertedUnit = FeministsConvertedUnit.Feminists;

    public bool PlayerFinishedLastShow = false;
    public bool FirstShowOfSession = true;

    public List<CardDefinition> DeckContents = new List<CardDefinition>();

    public bool DoLoadPlayerPrefs = true;

    public void CalculateFeministsConverted()
    {        
        string unitString = "";

        if (PlayerLevel <= 10)
        {
            PlayerFeministsConverted = PlayerLevel;
            PlayerConvertedUnit = FeministsConvertedUnit.Feminists;            
            unitString = PlayerFeministsConverted == 1 ? "Feminist" : "Feminists";
        }
        else if (PlayerLevel <= 20)
        {
            PlayerFeministsConverted = PlayerLevel - 10;
            PlayerConvertedUnit = FeministsConvertedUnit.Planets;
            unitString = PlayerFeministsConverted == 1 ? "Inhabited Planet" : "Inhabited Planets";
        }
        else if (PlayerLevel <= 30)
        {
            PlayerFeministsConverted = PlayerLevel - 20;
            PlayerConvertedUnit = FeministsConvertedUnit.Galaxies;
            unitString = PlayerFeministsConverted == 1 ? "Galaxy" : "Galaxies";
        }
        else if (PlayerLevel <= 40)
        {
            PlayerFeministsConverted = PlayerLevel - 30;
            PlayerConvertedUnit = FeministsConvertedUnit.Universes;
            unitString = PlayerFeministsConverted == 1 ? "Universe" : "Universes";
        }
        else if (PlayerLevel <= 50)
        {
            PlayerFeministsConverted = PlayerLevel - 40;
            PlayerConvertedUnit = FeministsConvertedUnit.Multiverses;
            unitString = PlayerFeministsConverted == 1 ? "Multiverse" : "Multiverses";
        }

        if (PlayerLevel == 1)
        {
            //player just starting has converted no feminists
            PlayerFeministsConverted = 0;
            FeministsConvertedString = "None";
        } 
        else if (PlayerLevel > 50)
        {
            PlayerFeministsConverted = PlayerLevel - 50;
            PlayerConvertedUnit = FeministsConvertedUnit.Beyond;
            FeministsConvertedString = "Beyond All Comprehension";            
        }
        else
        {
            PlayerFeministsConverted = (long)Mathf.Pow((float)PlayerFeministsConverted, (float)PlayerFeministsConverted);
            FeministsConvertedString = PlayerFeministsConverted + " " + unitString;
        }

    }

    public string FeministsConvertedString = "None";
    public bool DebugWidgetDepths = false;

    public enum GameScene
    {
        Title,
        Studio,
        Battle
    }

    public GameScene LastScene = GameScene.Title;

    public bool DoIntroTutorial = false;

}
