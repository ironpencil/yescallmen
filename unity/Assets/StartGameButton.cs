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
        StartCoroutine(DoStartGame());
    }

    public IEnumerator DoStartGame()
    {
        BarWipe.instance.DoWipe(false);

        yield return new WaitForSeconds(1.25f);    

        Globals.GetInstance().LastScene = Globals.GameScene.Title;
        Application.LoadLevel("outofbattle");
    }
}