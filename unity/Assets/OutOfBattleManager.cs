using UnityEngine;
using System.Collections;

public class OutOfBattleManager : MonoBehaviour {

    public static OutOfBattleManager outOfBattleManager;

    public bool CanStartShow = false;

	// Use this for initialization
    void Start()
    {
        outOfBattleManager = this;

        
    }
	
	// Update is called once per frame
	void Update () {
        //PointsLabel.text = Globals.GetInstance().FeministsConvertedString;
	
	}

    bool isStarted = false;

    public void StartScene()
    {
        if (!isStarted)
        {
            isStarted = true;
            StartCoroutine(DoStartScene());
        }
    }

    public UILabel PointsLabel;
    public UITweener PhoneLabelTween;

    public bool FastStart = false;

    public IEnumerator DoStartScene() {

        yield return new WaitForSeconds(0.2f);

        Debug.Log("Last Scene = " + Globals.GetInstance().LastScene);

        if (FastStart) { yield break; }

        switch (Globals.GetInstance().LastScene)
        {
            case Globals.GameScene.Title:
                //coming from the title scene
                GameMessageManager.gameMessageManager.AddLine(">> Welcome to #YesCallMen, the only show that gives TRUE FACTS about the struggle for Men's Rights. I'm your host, Richard Powers.", false, GameMessageManager.Speaker.Host);
                break;
            case Globals.GameScene.Battle:
                //coming from the battle scene
                GameMessageManager.gameMessageManager.AddLine(">> Numbers from your last show are coming in", false, GameMessageManager.Speaker.System);

                //GameMessageManager.gameMessageManager.AddLine("[" + GameMessageManager.Speaker.System + "]>> Numbers from your last show are coming in", false);

                while (!GameMessageManager.gameMessageManager.IsFinished)
                {
                    yield return new WaitForSeconds(0.1f);                    
                }

                if (FastStart) { yield break; }
                PointsDisplay.pointsDisplay.PointsLabel.text = "Receiving";

                for (int i = 0; i < 5; i++)
                {
                    yield return new WaitForSeconds(0.20f);
                    if (FastStart) { yield break; }
                    GameMessageManager.gameMessageManager.AddText(".", false, GameMessageManager.Speaker.System);
                    //GameMessageManager.gameMessageManager.AddText(".", false);
                    PointsDisplay.pointsDisplay.PointsLabel.text += " .";
                }

                if (FastStart) { yield break; }
                //GameMessageManager.gameMessageManager.AddText("[-] ", false);
                PointsDisplay.pointsDisplay.UpdatePointTotal();

                if (Globals.GetInstance().PlayerFinishedLastShow)
                {
                    //player won last show                    
                    GameMessageManager.gameMessageManager.AddLine(">> Welcome to another episode of #YesCallMen! We had a great show last time and hope to deliver more of the same tonight.", false, GameMessageManager.Speaker.Host);
                }
                else
                {
                    GameMessageManager.gameMessageManager.AddLine(">> Looks like your outburst last time didn't get you any new fans...", false, GameMessageManager.Speaker.System);
                    GameMessageManager.gameMessageManager.AddLine(">> OK well, welcome back to another episode of #YesCallMen. Hopefully things go more smoothly tonight and I don't have to deal with such incredible stupidity.", false, GameMessageManager.Speaker.Host);
                }

                break;
            default:
                break;
        }

        GameMessageManager.gameMessageManager.AddLine(">> Let's get right to it and take some calls.", false, GameMessageManager.Speaker.Host);

        while (!GameMessageManager.gameMessageManager.IsFinished)
        {
            yield return new WaitForSeconds(0.1f);
            if (FastStart) { yield break; }
        }

        CanStartShow = true;

        PhoneLabelTween.PlayForward();
    }
}
