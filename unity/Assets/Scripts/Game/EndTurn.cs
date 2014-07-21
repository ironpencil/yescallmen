using UnityEngine;
using System.Collections;

public class EndTurn : MonoBehaviour {

    private UIButton buttonScript;

    // Use this for initialization
    void Start()
    {
        buttonScript = gameObject.GetComponent<UIButton>();
	}
	
	// Update is called once per frame
	void Update () {

        if (TurnManager.turnManager.CurrentState == TurnManager.TurnState.PlayerActive)
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

    public void EndPlayerTurn()
    {
        TurnManager.turnManager.ChangeState(TurnManager.TurnState.PlayerCleanup);
    }
}
