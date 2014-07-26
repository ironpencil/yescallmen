using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class GameCard : MonoBehaviour
{
    #region Variables
    public UILabel titleLabel;
    public UILabel textLabel;
    public UILabel levelLabel;
    public UILabel damageLabel;

    public UISprite cardSprite;

    public CardDefinition cardDefinition;

    public bool isGainedCard = false;

    public enum CardType
    {
        None,
        Argument,
        Spite,
        Special,
        Action
    }

    public enum DamageType
    {
        None,
        Anger,
        Confusion,
        Fatigue
    }

    public CardType cardType = CardType.None;

    public DamageType damageType = DamageType.None;

    public int spiteUsed = 0;

    public int spiteAdded = 0;

    public int actionsAdded = 0;

    [SerializeField]
    private int levelPrvt = 1;
    public int Level
    {
        get { return levelPrvt; }
        set
        {
            levelPrvt = value;
            if (levelLabel != null)
            {
                if (value > 0)
                {
                    levelLabel.text = "Lvl " + levelPrvt;
                }
                else
                {
                    levelLabel.text = "";
                }
            }
        }
    }

    [SerializeField]
    private string titlePrvt = "";
    public string Title
    {
        get { return titlePrvt; }
        set
        {
            titlePrvt = value;
            if (titleLabel != null)
            {
                titleLabel.text = titlePrvt;
            }
        }
    }

    [SerializeField]
    private string textPrvt = "";
    public string AbilityText
    {
        get { return textPrvt; }
        set
        {
            textPrvt = value;
            if (textLabel != null)
            {
                textLabel.text = textPrvt;
            }
        }
    }

    [SerializeField]
    private int baseDamagePrvt = 0;
    public int BaseDamage
    {
        get { return baseDamagePrvt; }
        set
        {
            baseDamagePrvt = value;
        }
    }

    [SerializeField]
    public int currentDamagePrvt = 0;
    public int CurrentDamage
    {
        get { return currentDamagePrvt; }
        set
        {
            currentDamagePrvt = value;
            if (damageLabel != null)
            {
                if (BaseDamage > 0)
                {
                    damageLabel.text = currentDamagePrvt.ToString() + " Dmg";
                }
                else
                {
                    damageLabel.text = "";
                }
            }
        }
    }

    public Dictionary<string, string> eventVariables = new Dictionary<string, string>();

    public List<CardEvent> cardEvents = new List<CardEvent>();

    public List<CardDefinition> rememberedCards = new List<CardDefinition>();

    public void AddEvent(CardEvent cardEvent)
    {
        cardEvent.gameCard = this;
        cardEvents.Add(cardEvent);
    }

    [ContextMenu("Find Card Labels")]
    public void FindCardLabels()
    {
        //find title label
        UILabel[] labels = gameObject.GetComponentsInChildren<UILabel>();

        foreach (UILabel label in labels)
        {
            switch (label.gameObject.name)
            {
                case "TitleLabel":
                    titleLabel = label;
                    break;
                case "TextLabel":
                    textLabel = label;
                    break;
                case "LevelLabel":
                    levelLabel = label;
                    break;
                case "DamageLabel":
                    damageLabel = label;
                    break;
                default:
                    break;
            }
        }
    }

    [ContextMenu("Update Card Values")]
    public void UpdateCardValues()
    {
        if (titleLabel == null)
        {
            FindCardLabels();
        }

        Title = Title;
        AbilityText = AbilityText;
        CurrentDamage = CurrentDamage;
        Level = Level;
    }
    #endregion

    [ContextMenu("Level Up")]
    public bool LevelUp()
    {
        bool leveledCard = false;
        //can't level up a card that has a level of 0
        if (Level > 0)
        {
            DeckManager.deckManager.TrashCard(cardDefinition);
            ChangeLevel(Level + 1);
            DeckManager.deckManager.GainCard(cardDefinition);
            leveledCard = true;
        }
        return leveledCard;
    }

    private void ChangeLevel(int newLevel)
    {
        this.cardDefinition.Level = newLevel;

        CardFactory.cardFactory.SetupCard(this, this.cardDefinition);
    }

    public void Start () {
        FindCardLabels();
        UpdateCardValues();
    }
}

