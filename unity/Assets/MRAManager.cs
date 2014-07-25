using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MRAManager : MonoBehaviour
{

    public static MRAManager instance;

    // Use this for initialization
    void Start()
    {

        instance = this;

        InitializeQuotes(Globals.GetInstance().PlayerBattlesWon + Globals.GetInstance().PlayerBattlesLost);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private int lastCallerArgIndex = -1;
    private int lastHostArgIndex = -1;
    private int lastFemaleVerbIndex = -1;
    private int lastMaleVerbIndex = -1;
    private int lastFeministArgIndex = -1;

    public string GetCallerArgument()
    {
        string quote = "";

        quote = "says something about women just wanting " + GetFeministArgument() + ". How ridiculous!";

        return quote;
    }

    public string GetHostArgument()
    {
        int randomIndex = UnityEngine.Random.Range(0, 5);

        //prevent duplicates
        while (randomIndex == lastHostArgIndex)
        {
            randomIndex = UnityEngine.Random.Range(0, 5);
        }

        lastHostArgIndex = randomIndex;

        string quote = "";

        switch (randomIndex)
        {
            case 0: quote = "If it weren't for women who " + GetFemaleVerb() + ", maybe men wouldn't feel forced to " + GetMaleVerb() + "!";
                break;
            case 1: quote = "I dare you to try to explain why women " + GetFemaleVerb() + ". You can't. You just can't!";
                break;
            case 2: quote = "Just because men " + GetMaleVerb() + " doesn't mean women should " + GetFemaleVerb() + "!";
                break;
            default:
                quote = "Well, if women didn't " + GetFemaleVerb() + ", then men wouldn't have to " + GetMaleVerb() + "!";
                break;
        }

        return quote;
    }

    private string GetFeministArgument()
    {
        int randomIndex = UnityEngine.Random.Range(0, feministArguments.Count);

        //prevent duplicates
        while (randomIndex == lastFeministArgIndex)
        {
            randomIndex = UnityEngine.Random.Range(0, 5);
        }

        lastFeministArgIndex = randomIndex;

        return feministArguments[randomIndex];
    }

    private string GetFemaleVerb()
    {
        int randomIndex = UnityEngine.Random.Range(0, femaleMRAVerbs.Count);

        //prevent duplicates
        while (randomIndex == lastFemaleVerbIndex)
        {
            randomIndex = UnityEngine.Random.Range(0, 5);
        }

        lastFemaleVerbIndex = randomIndex;

        return femaleMRAVerbs[randomIndex];
    }

    private string GetMaleVerb()
    {
        int randomIndex = UnityEngine.Random.Range(0, maleMRAVerbs.Count);

        //prevent duplicates
        while (randomIndex == lastMaleVerbIndex)
        {
            randomIndex = UnityEngine.Random.Range(0, 5);
        }

        lastMaleVerbIndex = randomIndex;

        return maleMRAVerbs[randomIndex];
    }

    public void AddAnotherStrangeQuote()
    {
        if (femaleStrangeVerbs.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, femaleStrangeVerbs.Count);
            femaleMRAVerbs.Add(femaleStrangeVerbs[randomIndex]);
            femaleStrangeVerbs.RemoveAt(randomIndex);
        }

        if (maleStrangeVerbs.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, maleStrangeVerbs.Count);
            maleMRAVerbs.Add(maleStrangeVerbs[randomIndex]);
            maleStrangeVerbs.RemoveAt(randomIndex);
        }

        if (feministArgumentsStrange.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, feministArgumentsStrange.Count);
            feministArguments.Add(feministArgumentsStrange[randomIndex]);
            feministArgumentsStrange.RemoveAt(randomIndex);
        }
    }

    public void InitializeQuotes(int loadAdditional)
    {
        for (int i = 0; i < loadAdditional; i++)
        {
            AddAnotherStrangeQuote();
        }
    }

    #region quotes

    List<string> feministArguments = new List<string>() {
        "to be treated like human beings",
        "equal pay for equal work",
        "to be respected in the workplace",
        "to not be stared at by creepy guys constantly"
    };

    List<string> feministArgumentsStrange = new List<string>() {
        "to not be shrunk down and kept in jars as trophies",
        "less than 30-hour work days",
        "to be allowed to eat at the grown-ups table during family dinners"
    };

    List<string> femaleMRAVerbs = new List<string>() 
    {
        "hate fedoras",
        "bleed for 5 days without dying",
        "change their mind after sex",
        "complain about a supposed 'glass ceiling'",
        "dress slutty to get attention",
        "fake paternity tests",
        "friendzone men",
        "get excluded from the draft",
        "get mad when men ask innocent questions",
        "get rich off child support",
        "get sex whenever they want",
        "get so much maternity leave",
        "hate nice guys",
        "hit men without consequences",
        "make men look stupid in sitcoms",
        "make more money than men",
        "only date assholes",
        "rape men's wallets",
        "say 'No' when they mean 'Yes'",
        "secretly like being 'objectified'",
        "steal men's sperm",
        "suck at video games",
        "want money for sex",
        "want rough sex",
        "want to be superior to men",
        "think penises are evil",
        "lie about rape"
    };

    List<string> maleMRAVerbs = new List<string>()
    {
        "own a fedora for every occassion",
        "avoid females during their periods",
        "get falsely accused of rape",
        "mistrust paternity tests",
        "pay so much child support",
        "get drafted",
        "objectify females",
        "get attacked when they ask innocent questions",
        "beg for sex",
        "get no paternity leave",
        "pay attention to scantily clad females",
        "defend themselves physically",
        "spend all their money",
        "look stupid in sitcoms",
        "make less money than females",
        "act like assholes",
        "get stuck in the friendzone",
        "hoard their sperm",
        "excel at video games",
        "pay for sex",
        "think 'No' means 'Yes'",
        "end up being inferior to females"
    };

    List<string> femaleStrangeVerbs = new List<string>()
    {
        "under-appreciate My Little Pony",
        "forget that it's Adam and Eve not Adam and Steve",
        "have more bones than men",
        "keep on harping about global warming",
        "misunderstand bitcoins",
        "perform false flag operations",
        "attack the present from 10 years in the future",
        "bite the male's head off during mating",
        "build doomsday devices",
        "choose",
        "control gas prices",
        "control the media",
        "feast upon the brains of the living",
        "fiercely guard the secret to flight",
        "get confused about the ending to Bioshock Infinite",
        "get distracted during important movie scenes",
        "get involved in a land war with Asia",
        "have a three-foot-long tusk used for hunting and dominance rituals",
        "have cold feet",
        "have handbrakes to help them around tight corners",
        "have highly acidic blood",
        "have retractable razor-sharp claws",
        "have teeth in their vaginas",
        "have the powers of a puma",
        "hog the covers",
        "hold in their farts",
        "just want to cuddle",
        "kick ass and chew bubblegum",
        "literally drain the life force of men during sex",
        "move their eyes independently",
        "pull their victims hearts from their chests using only their bare hands",
        "refuse to acknowledge the brilliance of the A-Team",
        "refuse to share the gift of fire",
        "run faster than horses",
        "sap my sentry",
        "seriously just creep me out",
        "shed their skin once a month",
        "smell blood from up to 3 miles away",
        "speak the words of Zardoz",
        "stay a while, and listen",
        "tell me the odds",
        "transform by the light of the full moon",
        "unhinge their jaws to devour prey whole",
        "combine with other females to form a giant fighting robot"
    };

    List<string> maleStrangeVerbs = new List<string>()
    {
        "age twice as fast as women",
        "remain seated with their seatbelts fastened and their seat backs in the upright and locked position",
        "say, 'fuzzy pickles'",
        "obey",
        "devote more time to picking ice cream flavors",
        "sit on the bench during the big game",
        "live their lives never knowing the feel of the wind on their faces",
        "bring home the bacon",
        "shuffle everyday",
        "eat pack after pack of ramen noodles cooked over trash barrels",
        "mine for bitcoins",
        "store dismembered body parts in the freezer",
        "inflate themselves to three times their size to ward off predators",
        "get their fins stuck in fishing nets",
        "ALWAYS TYPE IN FULL CAPS",
        "love monster trucks",
        "change their coloration to blend in with their surroundings",
        "honk their horns at pretty ladies",
        "remind women they're ACTION FIGURES, not DOLLS",
        "use echolocation to find their way in the dark",
        "crumble under pressure",
        "go in against a Sicilian when death is on the line",
        "hibernate during the harsh winter months",
        "have no time for love, Dr. Jones",
        "get sick of these motherfuckin' snakes on this motherfuckin' plane",
        "harass female gamers on voice chat",
        "rage for a while",
        "duel to the death… no, to the pain",
        "eat larger portions",
        "rescue the president",
        "find themselves all out of gum",
        "never carry cash",
        "buy anti-werewolf talismans",
        "get their brains transplanted into robots",
        "put Baby in a corner",
        "stay a while… stay FOREVER",
        "become the Pope",
        "remain ever vigilant",
        "find their princess in a different castle",
        "go on hunting trips",
        "get hunted to extinction for their valuable pelts",
        "ride upon a steed, perchance to spy a lady",
        "stay frosty",
        "fight crime in spandex body armor",
        "eat hamburgers out of dumpsters",
        "find out that their sworn enemy is actually their father",
        "fight through an army of robots",
        "die in real life if they die in the game",
        "prefer the leading brand"
    };

    #endregion
}
