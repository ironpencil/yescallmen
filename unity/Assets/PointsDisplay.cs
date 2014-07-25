using UnityEngine;
using System.Collections;

public class PointsDisplay : MonoBehaviour {

    public UILabel PointsLabel;

    public static PointsDisplay pointsDisplay;
	// Use this for initialization
	void Start () {
        pointsDisplay = this;
        PointsLabel.text = Globals.GetInstance().FeministsConvertedString;
	
	}
	
	// Update is called once per frame
	void Update () {
	
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
                    GameMessageManager.gameMessageManager.AddLine("Looks like you may have convinced a couple viewers...", false);
                    break;
                case 3:
                    GameMessageManager.gameMessageManager.AddLine("Looks like people in town are talking, and you have a few more fans...", false);
                    break;
                case 4:
                    GameMessageManager.gameMessageManager.AddLine("Looks like you've convinced quite a few people in town!", false);
                    break;
                case 5:
                    GameMessageManager.gameMessageManager.AddLine("Wow, your show is really picking up! The whole town is raving!", false);
                    break;
                case 6:
                    GameMessageManager.gameMessageManager.AddLine("People in nearby cities have begun watching your show!", false);
                    break;
                case 7:
                    GameMessageManager.gameMessageManager.AddLine("Clips from your show have gone viral online!", false);
                    break;
                case 8:
                    GameMessageManager.gameMessageManager.AddLine("You're getting fan email from all over the country!", false);
                    break;
                case 9:
                    GameMessageManager.gameMessageManager.AddLine("Your show is now one of the most popular shows in the world!", false);
                    break;
                case 10:
                    GameMessageManager.gameMessageManager.AddLine("Are there even that many people on the planet?!", false);
                    break;
                case 11:
                    GameMessageManager.gameMessageManager.AddLine("You have convinced every single living creature on the planet!", false);
                    GameMessageManager.gameMessageManager.AddLine("Congratulations, you are the biggest asshole ever!", false);
                    break;
                case 12:
                    GameMessageManager.gameMessageManager.AddLine("First contact. Aliens have come to Earth, and they want more of your show!", false);
                    break;
                case 21:
                    GameMessageManager.gameMessageManager.AddLine("You have converted the entire galaxy!", false);
                    break;
                case 31:
                    GameMessageManager.gameMessageManager.AddLine("Every living being in the universe watches your show constantly.", false);
                    break;
                case 32:
                    GameMessageManager.gameMessageManager.AddLine("Your message travels the known universe and into neighboring universes.", false);
                    break;
                case 41:
                    GameMessageManager.gameMessageManager.AddLine("Your message spans across all of reality.", false);
                    break;
                case 42:
                    GameMessageManager.gameMessageManager.AddLine("Your message transcends time and space into alternate dimensions and realities.", false);
                    break;
                default:
                    if (Globals.GetInstance().PlayerLevel > 50)
                    {
                        GameMessageManager.gameMessageManager.AddLine("The reach of your message is literally unfathomable.", false);
                        GameMessageManager.gameMessageManager.AddLine("All feminists that were, are, or ever will be now understand your truth.", false);
                        GameMessageManager.gameMessageManager.AddLine("", false);
                        GameMessageManager.gameMessageManager.AddLine("You are truly the worst.", false);
                    }
                    else
                    {
                        GameMessageManager.gameMessageManager.AddLine("Looks like you have more fans!", false);
                    }
                    break;
            }
        }
        PointsDisplay.pointsDisplay.RefreshPointsDisplay();
    }

}
