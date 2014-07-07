using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour {

    public GameObject HandPanel;
    public GameObject GamePanel;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Play()
    {
        if (HandPanel != null && GamePanel != null)
        {
            NGUITools.SetActive(HandPanel, true);
            NGUITools.SetActive(GamePanel, true);

            GetComponent<TweenAlpha>().PlayForward();

            //NGUITools.SetActive(gameObject, false);
        }
    }

    public void Disable()
    {
        NGUITools.SetActive(gameObject, false);
    }
}
