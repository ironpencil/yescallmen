using UnityEngine;
using System.Collections;

public class NextBattle : MonoBehaviour {

    private UIButton buttonScript;

	// Use this for initialization
	void Start () {
        buttonScript = gameObject.GetComponent<UIButton>();
	}
	
	// Update is called once per frame
	void Update () {

        if (TurnManager.turnManager.CurrentState == TurnManager.TurnState.OutOfBattle)
        {
            if (!buttonScript.isEnabled)
            {
                buttonScript.isEnabled = true;
            }
        }
        else
        {
            if (buttonScript.isEnabled)
            {
                buttonScript.isEnabled = false;
            }
        }	
	}

    public void StartNextBattle()
    {
        if (TurnManager.turnManager.CurrentState == TurnManager.TurnState.OutOfBattle)
        {
            TurnManager.turnManager.ChangeState(TurnManager.TurnState.StartBattle);
        }
    }
}
