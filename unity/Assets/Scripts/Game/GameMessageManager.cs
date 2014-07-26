using UnityEngine;
using System.Collections;

public class GameMessageManager : MonoBehaviour {

    public static GameMessageManager gameMessageManager;

    public UILabel labelScript;
    public UIScrollView messageScrollview;

    public IPTypewriterEffect typewriter;

    public int charsPerSecond = 50;

    public bool isFinished = true;

	// Use this for initialization
	void Awake () {
        gameMessageManager = this;
        //typewriter = labelScript.GetComponent<IPTypewriterEffect>();

        typewriter.onFinished.Add(new EventDelegate(OnFinished));

        HostColorHex = ColorToHex(HostColor);
        CallerColorHex = ColorToHex(CallerColor);
        SystemColorHex = ColorToHex(SystemColor);

	}
	
	// Update is called once per frame
	void Update () {
        typewriter.charsPerSecond = charsPerSecond;        

	}

    public void SetText(string text, bool instant, string colorHex)
    {
        SetText("[" + colorHex + "]" + text + "[-] ", instant);
    }

    public void SetText(string text, bool instant)
    {
        isFinished = false;
        typewriter.SetText(text);        
        //typewriter.isActive = true;

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

    public void AddLine(string text, bool instant, string colorHex)
    {
        AddLine("[" + colorHex + "]" + text + "[-] ", instant);
    }

    public void AddLine(string text, bool instant)
    {
        isFinished = false;
        typewriter.AddText("\r\n" + text);
        //typewriter.isActive = true;

        if (instant)
        {
            typewriter.Finish();
        }

        messageScrollview.ResetPosition();
    }

    public void AddText(string text, bool instant, string colorHex)
    {
        AddText("[" + colorHex + "]" + text + "[-] ", instant);
    }

    public void AddText(string text, bool instant)
    {
        isFinished = false;
        typewriter.AddText(text);
        //typewriter.isActive = true;
        
        if (instant)
        {
            typewriter.Finish();
        }

        messageScrollview.ResetPosition();
    }

    private void OnFinished() {
        Debug.Log("Message Finished Displaying!");
        isFinished = true;
    }

    public Color HostColor = Color.white;
    public Color CallerColor = Color.magenta;
    public Color SystemColor = Color.yellow;

    public string HostColorHex;
    public string CallerColorHex;
    public string SystemColorHex;

    string ColorToHex(Color32 color)
    {
        string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
        return hex;
    }

    Color HexToColor(string hex)
    {
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        return new Color32(r, g, b, 255);
    }
}
