using UnityEngine;
using System.Collections;

public class StartShowButton : MonoBehaviour {

    private UIButton buttonScript;

	// Use this for initialization
	void Start () {
        buttonScript = gameObject.GetComponent<UIButton>();
	}
	
	// Update is called once per frame
	void Update () {
        if (OutOfBattleManager.outOfBattleManager.CanStartShow != buttonScript.isEnabled)
        {
            buttonScript.isEnabled = OutOfBattleManager.outOfBattleManager.CanStartShow;
        }	
	}

    public void StartNextShow()
    {
        //GameMessageManager.gameMessageManager.SetText("", true);
        //Globals.GetInstance().PlayerFinishedLastShow = true;
        //Globals.GetInstance().PlayerLevel++;

        GameMessageManager.gameMessageManager.AddLine(">> Alright, here's our first caller of the evening...", false, GameMessageManager.gameMessageManager.HostColorHex);

        OutOfBattleManager.outOfBattleManager.CanStartShow = false;

        StartCoroutine(StartShow());
    }

    private IEnumerator StartShow()
    {
        while (!GameMessageManager.gameMessageManager.isFinished)
        {
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(1.0f);

        Globals.GetInstance().LastScene = Globals.GameScene.Studio;
        Application.LoadLevel("battle");
    }
}
