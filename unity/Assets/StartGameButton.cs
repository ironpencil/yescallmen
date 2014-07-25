using UnityEngine;
using System.Collections;

public class StartGameButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void StartGame()
    {
        Globals.GetInstance().LastScene = Globals.GameScene.Title;
        Application.LoadLevel("outofbattle");
    }
}
