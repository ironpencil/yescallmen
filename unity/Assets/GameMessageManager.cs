using UnityEngine;
using System.Collections;

public class GameMessageManager : MonoBehaviour {

    public static GameMessageManager gameMessageManager;

    public GameObject labelObject;
    public UIScrollView messageScrollview;

    private IPTypewriterEffect typewriter;

    public int charsPerSecond = 50;

	// Use this for initialization
	void Start () {
        gameMessageManager = this;
        typewriter = labelObject.GetComponent<IPTypewriterEffect>();

	}
	
	// Update is called once per frame
	void Update () {
        typewriter.charsPerSecond = charsPerSecond;        
	}

    public void SetText(string text, bool instant)
    {
        typewriter.SetText(text);        
        typewriter.isActive = true;

        if (instant)
        {
            typewriter.Finish();
        }
        else
        {
            typewriter.ResetToBeginning();
        }

        messageScrollview.ResetPosition();
    }

    public void AddLine(string text, bool instant)
    {
        typewriter.AddText("\r\n" + text);
        typewriter.isActive = true;

        if (instant)
        {
            typewriter.Finish();
        }

        messageScrollview.ResetPosition();
    }
}
