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
            int playerLevel = Globals.GetInstance().PlayerLevel;

            if (playerLevel > PointsDisplayMessages.Count)
            {
                GameMessageManager.gameMessageManager.AddLine(">> The reach of your message is literally unfathomable. All feminists that are, were, or ever will be now understand your truth.", false, GameMessageManager.Speaker.System);
                GameMessageManager.gameMessageManager.AddLine(">> You are truly the worst.", false, GameMessageManager.Speaker.System);
            }
            else
            {
                GameMessageManager.gameMessageManager.AddLine(PointsDisplayMessages[playerLevel - 1], false, GameMessageManager.Speaker.System);
            }


            //switch (Globals.GetInstance().PlayerLevel)
            //{                    
            //    case 2:
            //        GameMessageManager.gameMessageManager.AddLine(">> Looks like you may have convinced a couple viewers...", false, GameMessageManager.Speaker.System);
            //        break;
            //    case 3:
            //        GameMessageManager.gameMessageManager.AddLine(">> Looks like people in town are talking, and you have a few more fans...", false, GameMessageManager.Speaker.System);
            //        break;
            //    case 4:
            //        GameMessageManager.gameMessageManager.AddLine(">> Looks like you've convinced quite a few people in town!", false, GameMessageManager.Speaker.System);
            //        break;
            //    case 5:
            //        GameMessageManager.gameMessageManager.AddLine(">> Wow, your show is really picking up! The whole town is raving!", false, GameMessageManager.Speaker.System);
            //        break;
            //    case 6:
            //        GameMessageManager.gameMessageManager.AddLine(">> People in nearby cities have begun watching your show!", false, GameMessageManager.Speaker.System);
            //        break;
            //    case 7:
            //        GameMessageManager.gameMessageManager.AddLine(">> Clips from your show have gone viral online!", false, GameMessageManager.Speaker.System);
            //        break;
            //    case 8:
            //        GameMessageManager.gameMessageManager.AddLine(">> You're getting fan email from all over the country!", false, GameMessageManager.Speaker.System);
            //        break;
            //    case 9:
            //        GameMessageManager.gameMessageManager.AddLine(">> Your show is now one of the most popular shows in the world!", false, GameMessageManager.Speaker.System);
            //        break;
            //    case 10:
            //        GameMessageManager.gameMessageManager.AddLine(">> Are there even that many people on the planet?!", false, GameMessageManager.Speaker.System);
            //        break;
            //    case 11:
            //        GameMessageManager.gameMessageManager.AddLine(">> You have convinced every single living creature on the planet! Congratulations, you are the biggest asshole ever!", false, GameMessageManager.Speaker.System);
            //        //GameMessageManager.gameMessageManager.AddLine("Congratulations, you are the biggest asshole ever!", false, GameMessageManager.Speaker.System);
            //        break;
            //    case 12:
            //        GameMessageManager.gameMessageManager.AddLine(">> First contact. Aliens have come to Earth, and they want more of your show!", false, GameMessageManager.Speaker.System);
            //        break;
            //    case 21:
            //        GameMessageManager.gameMessageManager.AddLine(">> You have converted the entire galaxy!", false, GameMessageManager.Speaker.System);
            //        break;
            //    case 31:
            //        GameMessageManager.gameMessageManager.AddLine(">> Every living being in the universe watches your show constantly.", false, GameMessageManager.Speaker.System);
            //        break;
            //    case 32:
            //        GameMessageManager.gameMessageManager.AddLine(">> Your message travels the known universe and into neighboring universes.", false, GameMessageManager.Speaker.System);
            //        break;
            //    case 41:
            //        GameMessageManager.gameMessageManager.AddLine(">> Your message spans across all of reality.", false, GameMessageManager.Speaker.System);
            //        break;
            //    case 42:
            //        GameMessageManager.gameMessageManager.AddLine(">> Your message transcends time and space into alternate dimensions and realities.", false, GameMessageManager.Speaker.System);
            //        break;
            //    default:
            //        if (Globals.GetInstance().PlayerLevel > 50)
            //        {
            //            GameMessageManager.gameMessageManager.AddLine(">> The reach of your message is literally unfathomable. All feminists that were, are, or ever will be now understand your truth.", false, GameMessageManager.Speaker.System);
            //            GameMessageManager.gameMessageManager.AddLine(">> You are truly the worst.", false, GameMessageManager.Speaker.System);
            //            //GameMessageManager.gameMessageManager.AddLine("All feminists that were, are, or ever will be now understand your truth.", false, GameMessageManager.Speaker.System);
            //            //GameMessageManager.gameMessageManager.AddLine("", false);
            //            //GameMessageManager.gameMessageManager.AddLine("You are truly the worst.", false, GameMessageManager.Speaker.System);
            //        }
            //        else
            //        {
            //            DoRandomPointsMessage();
            //        }
            //        break;
            //}



        }
        PointsDisplay.pointsDisplay.RefreshPointsDisplay();
    }

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
        ">> There was a hover-pile-up on the hover-highways of Isop 1138 due to a Qieddash watching your show on xits holotuner and losing control.",
        //25
        ">> A war has broken out in the Geirskokul Colony over your comments on how Knofluhs spluxionate like this, but Raksyims spluxionate like THIS.",
        //26
        ">> A cult has sprung up around a Zilphylst male who claims to be receiving secret messages from you encoded in your blinking patterns.",
        //27
        ">> In a far away galaxy, after watching your show, a young moisture farmer goes outside and looks out across the desert with hope in his eyes.",
        //28
        ">> The tabloids in the Maenali Cluster are printing doctored holophotos of you sneaking into a hotel with Eccentrica Gallumbits.",
        //29       
        ">> A new law has been enacted in the Pegasi arm of 6045 Vespae requiring all females to pay monetary reparations.",
        //30
        ">> A species of shapeshifters from the Aubrurl sector have all decided to permanently assume a facsimile of your form.", 
        //case 31:
        ">> Every living being in the universe watches your show constantly.",
        //case 32:
        ">> Your message travels the known universe and into neighboring universes.",
        //33
        ">> A man hearing your show on the radio in a parallel universe picks up his phone and says 'Anita! Anita! It's Marvin! Your cousin, Marvin SARKEESIAN! You know that new " + 
        "sound you're looking for? Well listen to THIS!'",
        //34
        ">> A group of man-apes banging on bones around a black monolith sense the sub-molecular vibrations caused by your show's transmission and stop what they are doing for a moment to look skyward.",
        //35
        ">> A race of hermaphroditic space lobsters in a nearby universe has decided to stop reproducing due to disgust with their own female sex organs.",
        //36
        ">> The first annual convention of parallel versions of you was held at a Sizzler attached to a Best Western in Spokane.",
        //37
        ">> You have been selected as a celebrity guest judge on this year's much-anticipated season of Jeremy's Dance Magic.",
        //38
        ">> The denizens of the underside of the world have just heard about your show, and they can't get enough.",
        //39
        ">> Theoretical beings from a perpendicular universe think your show is the best thing since unsliced bread.",
        //40
        ">> A female version of you from a parallel universe has offered her sincerest apologies to your fans.",
        //case 41:
        ">> Your message spans across all of reality.",
        //case 42:
        ">> Your message transcends time and space into alternate dimensions and realities.",
        //43
        ">> Babies are now born already wearing fedoras, and nobody thinks it is weird.",
        //44
        ">> A group of plucky young kids from the past have attempted to stop transmissions of your show. They have been dealt with.",
        //45
        ">> A creature that manifests only as nostalgia about the 80s was caught trying to sneak into your dressing room. You have graciously decided not to press charges.",
        //46
        ">> The Black Gate has opened, and from it spills terribles creatures imagined only in forgotten nightmares... and they just LOVE your show!",
        //47
        ">> The very fabric of reality has begun to flex and deform, such is the popularity of your show.",
        //48
        ">> horse horse horse horse horse horse horse horse horse horse horse horse horse horse horse horse horse horse horse horse horse horse horse horse horse " +
        "horse horse horse horse horse horse horse horse horse horse horse horse horse horse horse horse horse horse horse horse horse horse horse horse horse " +
        "horse horse horse horse horse horse horse horse horse horse horse horse horse hor",
        //49
        ">> Daisy, Daisy give me your answer do.\r\nI'm half crazy all for the love of you.\r\nIt won't be a stylish marriage,\r\nI can't afford a carriage.\r\n" +
        "But you'll look sweet,\r\nUpon the seat,\r\nOf a bicycle built for two.",
        //50
        ">> The show's broadcast has reached the reality where you, the player, currently live. You now believe everything in this game is literally true.",
    };
}
