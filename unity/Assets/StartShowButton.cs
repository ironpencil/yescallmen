using UnityEngine;
using System.Collections;

public class StartShowButton : MonoBehaviour {

    //private UIButton buttonScript;

    public bool debugPointsDisplay = false;
	// Use this for initialization
	void Start () {
        //buttonScript = gameObject.GetComponent<UIButton>();
	}
	
	// Update is called once per frame
	void Update () {
        //if (OutOfBattleManager.outOfBattleManager.CanStartShow != buttonScript.isEnabled)
        //{
        //    buttonScript.isEnabled = OutOfBattleManager.outOfBattleManager.CanStartShow;
        //}	
	}

    bool showStarted = false;

    public void OnClick()
    {
        if (!showStarted)
        {
            StartNextShow();
        }
    }

    public void StartNextShow()
    {
        if (debugPointsDisplay)
        {
            GameMessageManager.gameMessageManager.ClearText();
            Globals.GetInstance().PlayerFinishedLastShow = true;
            Globals.GetInstance().PlayerLevel++;
            PointsDisplay.pointsDisplay.UpdatePointTotal();

            return;
        }

        showStarted = true;

        if (OutOfBattleManager.outOfBattleManager.CanStartShow)
        {
            GameMessageManager.gameMessageManager.AddLine(">> Alright, here's our first caller of the evening...", false, GameMessageManager.Speaker.Host);
        }
        else
        {
            OutOfBattleManager.outOfBattleManager.FastStart = true;
            GameMessageManager.gameMessageManager.ClearText();
            GameMessageManager.gameMessageManager.AddLine(">> Enough waiting around, let's take our first caller...", false, GameMessageManager.Speaker.Host);
        }

        StartCoroutine(StartShow());
    }

    private IEnumerator StartShow()
    {
        while (!GameMessageManager.gameMessageManager.IsFinished)
        {
            yield return new WaitForSeconds(0.1f);
        }

        if (OutOfBattleManager.outOfBattleManager.FastStart)
        {
            BarWipe.instance.DoWipe(false, 0.5f);
            yield return new WaitForSeconds(0.5f);
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
            BarWipe.instance.DoWipe(false);
            yield return new WaitForSeconds(1.25f);
        }

        Globals.GetInstance().LastScene = Globals.GameScene.Studio;
        Application.LoadLevel("battle");
    }
}
