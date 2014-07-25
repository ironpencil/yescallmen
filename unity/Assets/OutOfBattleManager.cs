using UnityEngine;
using System.Collections;

public class OutOfBattleManager : MonoBehaviour {

    public static OutOfBattleManager outOfBattleManager;

    public bool CanStartShow = false;

	// Use this for initialization
    void Start()
    {
        outOfBattleManager = this;

        StartCoroutine(DoStartScene());
    }
	
	// Update is called once per frame
	void Update () {
        PointsLabel.text = Globals.GetInstance().FeministsConvertedString;
	
	}

    public UILabel PointsLabel;

    public IEnumerator DoStartScene() {

        yield return new WaitForEndOfFrame();

        Debug.Log("Last Scene = " + Globals.GetInstance().LastScene);

        switch (Globals.GetInstance().LastScene)
        {
            case Globals.GameScene.Title:
                //coming from the title scene
                GameMessageManager.gameMessageManager.AddLine("\"Welcome to #YesCallMen, the only show that gives TRUE FACTS about the struggle for Men's Rights and exposes the blatant lies SPREAD by the Feminist Regime.\"", false);
                break;
            case Globals.GameScene.Battle:
                //coming from the battle scene

                GameMessageManager.gameMessageManager.AddLine("Numbers from your last show are coming in", false);

                while (!GameMessageManager.gameMessageManager.isFinished)
                {
                    yield return new WaitForSeconds(0.1f);
                }

                for (int i = 0; i < 6; i++)
                {
                    yield return new WaitForSeconds(0.20f);
                    GameMessageManager.gameMessageManager.AddText("   .", false);                    
                }                

                if (Globals.GetInstance().PlayerFinishedLastShow)
                {
                    //player won last show
                    PointsDisplay.pointsDisplay.UpdatePointTotal();
                    GameMessageManager.gameMessageManager.AddLine("\"Welcome to another episode of #YesCallMen! We had a great show last time and hope to deliver more of the same tonight.\"", false);
                }
                else
                {
                    GameMessageManager.gameMessageManager.AddLine("Looks like your outburst didn't get you any new fans...", false);
                    GameMessageManager.gameMessageManager.AddLine("\"OK well, welcome back to another episode of #YesCallMen. Hopefully things go more smoothly tonight and I don't have to deal with such incredible stupidity.\"", false);
                }

                break;
            default:
                break;
        }

        GameMessageManager.gameMessageManager.AddLine("\"Let's get right to it and take some calls.\"", false);

        while (!GameMessageManager.gameMessageManager.isFinished)
        {
            yield return new WaitForSeconds(0.1f);
        }

        CanStartShow = true;
    }
}
