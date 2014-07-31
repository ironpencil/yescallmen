using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PointsDisplay : MonoBehaviour
{

    public UILabel PointsLabel;

    public static PointsDisplay pointsDisplay;
    // Use this for initialization
    void Start()
    {
        pointsDisplay = this;
        PointsLabel.text = Globals.GetInstance().FeministsConvertedString;

    }

    // Update is called once per frame
    void Update()
    {

    }

    [ContextMenu("Refresh Display")]
    public void RefreshPointsDisplay()
    {
        Globals.GetInstance().CalculateFeministsConverted();
        PointsLabel.text = Globals.GetInstance().FeministsConvertedString;
    }

    public void UpdatePointTotal()
    {
        if (Globals.GetInstance().PlayerFinishedLastShow)
        {
            //if we finished the last show, we just leveled up, so display a message about how many people we've converted
            switch (Globals.GetInstance().PlayerLevel)
            {                    
                case 2:
                    GameMessageManager.gameMessageManager.AddLine(">> Looks like you may have convinced a couple viewers...", false, GameMessageManager.Speaker.System);
                    break;
                case 3:
                    GameMessageManager.gameMessageManager.AddLine(">> Looks like people in town are talking, and you have a few more fans...", false, GameMessageManager.Speaker.System);
                    break;
                case 4:
                    GameMessageManager.gameMessageManager.AddLine(">> Looks like you've convinced quite a few people in town!", false, GameMessageManager.Speaker.System);
                    break;
                case 5:
                    GameMessageManager.gameMessageManager.AddLine(">> Wow, your show is really picking up! The whole town is raving!", false, GameMessageManager.Speaker.System);
                    break;
                case 6:
                    GameMessageManager.gameMessageManager.AddLine(">> People in nearby cities have begun watching your show!", false, GameMessageManager.Speaker.System);
                    break;
                case 7:
                    GameMessageManager.gameMessageManager.AddLine(">> Clips from your show have gone viral online!", false, GameMessageManager.Speaker.System);
                    break;
                case 8:
                    GameMessageManager.gameMessageManager.AddLine(">> You're getting fan email from all over the country!", false, GameMessageManager.Speaker.System);
                    break;
                case 9:
                    GameMessageManager.gameMessageManager.AddLine(">> Your show is now one of the most popular shows in the world!", false, GameMessageManager.Speaker.System);
                    break;
                case 10:
                    GameMessageManager.gameMessageManager.AddLine(">> Are there even that many people on the planet?!", false, GameMessageManager.Speaker.System);
                    break;
                case 11:
                    GameMessageManager.gameMessageManager.AddLine(">> You have convinced every single living creature on the planet! Congratulations, you are the biggest asshole ever!", false, GameMessageManager.Speaker.System);
                    //GameMessageManager.gameMessageManager.AddLine("Congratulations, you are the biggest asshole ever!", false, GameMessageManager.Speaker.System);
                    break;
                case 12:
                    GameMessageManager.gameMessageManager.AddLine(">> First contact. Aliens have come to Earth, and they want more of your show!", false, GameMessageManager.Speaker.System);
                    break;
                case 21:
                    GameMessageManager.gameMessageManager.AddLine(">> You have converted the entire galaxy!", false, GameMessageManager.Speaker.System);
                    break;
                case 31:
                    GameMessageManager.gameMessageManager.AddLine(">> Every living being in the universe watches your show constantly.", false, GameMessageManager.Speaker.System);
                    break;
                case 32:
                    GameMessageManager.gameMessageManager.AddLine(">> Your message travels the known universe and into neighboring universes.", false, GameMessageManager.Speaker.System);
                    break;
                case 41:
                    GameMessageManager.gameMessageManager.AddLine(">> Your message spans across all of reality.", false, GameMessageManager.Speaker.System);
                    break;
                case 42:
                    GameMessageManager.gameMessageManager.AddLine(">> Your message transcends time and space into alternate dimensions and realities.", false, GameMessageManager.Speaker.System);
                    break;
                default:
                    if (Globals.GetInstance().PlayerLevel > 50)
                    {
                        GameMessageManager.gameMessageManager.AddLine(">> The reach of your message is literally unfathomable. All feminists that were, are, or ever will be now understand your truth.", false, GameMessageManager.Speaker.System);
                        GameMessageManager.gameMessageManager.AddLine(">> You are truly the worst.", false, GameMessageManager.Speaker.System);
                        //GameMessageManager.gameMessageManager.AddLine("All feminists that were, are, or ever will be now understand your truth.", false, GameMessageManager.Speaker.System);
                        //GameMessageManager.gameMessageManager.AddLine("", false);
                        //GameMessageManager.gameMessageManager.AddLine("You are truly the worst.", false, GameMessageManager.Speaker.System);
                    }
                    else
                    {
                        DoRandomPointsMessage();
                    }
                    break;
            }
        }
        PointsDisplay.pointsDisplay.RefreshPointsDisplay();
    }

    private void DoRandomPointsMessage()
    {
        if (Globals.GetInstance().PlayerLevel > 40)
        {
            GameMessageManager.gameMessageManager.AddLine(">> Looks like you have more fans!", false, GameMessageManager.Speaker.System);
        }
        else if (Globals.GetInstance().PlayerLevel > 30)
        {
            GameMessageManager.gameMessageManager.AddLine(">> Looks like you have more fans!", false, GameMessageManager.Speaker.System);
        }
        else if (Globals.GetInstance().PlayerLevel > 10)
        {
            GameMessageManager.gameMessageManager.AddLine(">> Looks like you have more fans!", false, GameMessageManager.Speaker.System);
        }
        else if (Globals.GetInstance().PlayerLevel > 10)
        {
            GameMessageManager.gameMessageManager.AddLine(">> Looks like you have more fans!", false, GameMessageManager.Speaker.System);
        }
        else
        {
            GameMessageManager.gameMessageManager.AddLine(">> Looks like you have more fans!", false, GameMessageManager.Speaker.System);
        }
    }

    private List<string> GalaxyMessages = new List<string>()
    {
        ">> Everyone in Uvshillon Sector 7124 is now required to wear a fedora on all 3 of their heads at all times.",
        ">> The Circini star system has been destroyed because it interfered with your show's reception on 9239 Tauri Prime."
    };
    private List<string> UniverseMessages = new List<string>()
    {
        ">> PlanetsMessage"
    };
    private List<string> MultiverseMessages = new List<string>()
    {
        ">> Somewhere, a baby has been born wearing a fedora."
    };

    private List<string> PointsDisplayMessages = new List<string>() {
        // case 1: - just in case, but shouldn't ever display this
        ">> Nothing yet... ",
        //case 2:
        ">> Looks like you may have convinced a couple viewers...",
        //case 3:
        ">> Looks like people in town are talking, and you have a few more fans...",
        //case 4:
        ">> Looks like you've convinced quite a few people in town!",
        //case 5:
        ">> Wow, your show is really picking up! The whole town is raving!",
        //case 6:
        ">> People in nearby cities have begun watching your show!",
        //case 7:
        ">> Clips from your show have gone viral online!",
        //case 8:
        ">> You're getting fan email from all over the country!",
        // case 9:
        ">> Your show is now one of the most popular shows in the world!",
        //case 10:
        ">> Are there even that many people on the planet?!",
        //case 11:
        ">> You have convinced every single living creature on the planet! Congratulations, you are the biggest asshole ever!",
        //case 12:
        ">> First contact. Aliens have come to Earth, and they want more of your show!",
        //13
        ">> Interstellar traffic around Earth has increased dramatically.",        
        //14
        ">> A museum for alien tourists has been set up in your honor. On the moon.",
        //15
        ">> The ruler of the Daniea Industrial Complex on Tarandi Prime has commissioned a statue to be built in your honor.",
        //16
        ">> Psyri IV has been demolished to build a hyperspace bypass to Earth.",
        //17
        ">> You've been awarded the Key to the Crystalline City of Z'Supa on 8421 Persei IIIa.",
        //18
        ">> It turns out Qleaweo hipsters on Striavius Theta liked you before you were cool.",
        //19
        ">> Xruchaks from Gheeda VII have been seen cruising up and down Psori's Belt blaring your show's theme music.",
        //20
        ">> An Ueksig from the Shattered Moon of Gullveig V claims to be carrying your love child.",
        //case 21:
        ">> You have converted the entire galaxy!",
        //22
        ">> Everyone in Uvshillon Sector 7124 is now required to wear a fedora on all 3 of their heads at all times.",
        //23
        ">> The Circini star system has been destroyed because it interfered with your show's reception in 9239 Tauri Prime.",
        //24
        ">> A war has broken out in blah because blah thought you said BLALAHAHL",
        //receiving secret messages
        //25
        //26
        //27
        //28
        //29
        //30
        //case 31:
        ">> Every living being in the universe watches your show constantly.",
        //case 32:
        ">> Your message travels the known universe and into neighboring universes.",
        //33
        //34
        //35
        //36
        //37
        //38
        //39
        //40
        //case 41:
        ">> Your message spans across all of reality.",
        //case 42:
        ">> Your message transcends time and space into alternate dimensions and realities."
        //43
        //44
        //45
        //46
        //47
        //48
        //49
        //50
    };
}
