﻿using UnityEngine;
using System.Collections;

public class GameMessageManager : MonoBehaviour {

    public static GameMessageManager gameMessageManager;

    public GameObject labelObject;
    public UIScrollView messageScrollview;

    private UILabel uiLabel;
    private IPTypewriterEffect typewriter;

    public int charsPerSecond = 50;

	// Use this for initialization
	void Start () {
        gameMessageManager = this;
        uiLabel = labelObject.GetComponent<UILabel>();
        typewriter = labelObject.GetComponent<IPTypewriterEffect>();

	}
	
	// Update is called once per frame
	void Update () {
        typewriter.charsPerSecond = charsPerSecond;        
	}

    public void SetText(string text, bool instant)
    {
        uiLabel.text = text;
        typewriter.ResetToBeginning();
        typewriter.isActive = true;

        if (instant)
        {
            typewriter.Finish();
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
