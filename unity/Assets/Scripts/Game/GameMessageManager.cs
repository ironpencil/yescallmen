using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameMessageManager : MonoBehaviour {

    public static GameMessageManager gameMessageManager;

    public UILabel labelScript;
    public UIScrollView messageScrollview;

    public IPTypewriterEffect typewriter;

    public int charsPerSecond = 50;
    
    private bool isTypewriterFinished = true;
    public bool IsFinished
    {
        get
        {
            return (isTypewriterFinished && messageQueue.Count == 0);
        }
    }

    public bool PlayRandomMumbles = true;
    public bool VictoryMumblesOnly = false;
    public bool DefeatMumblesOnly = false;

    public List<AudioClip> RandomMumbles;

    public List<AudioClip> VictoryMumbles;
    public List<AudioClip> DefeatMumbles;

    public bool PlayIntroMumbles = false;

    public List<AudioClip> IntroMumbles;

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

        ProcessQueue();

            DoRandomMumbles();
	}

    
    //secret counter to turn on host mumble all the time
    private int MumbleTriggerCounter = 0;
    public void IncrementMumbleTriggerCounter()
    {
        MumbleTriggerCounter++;

        switch (MumbleTriggerCounter)
        {
            case 5:
                AlwaysDoHostMumble = !AlwaysDoHostMumble;
                break;
            case 6:
                MumbleDelayOffset = MumbleDelayOffset - 0.5f;
                break;
            case 7:
                MumbleDelayOffset = MumbleDelayOffset - 0.5f;
                break;
            case 8:
                MumbleDelayOffset = MumbleDelayOffset + 1.0f;
                AlwaysDoHostMumble = !AlwaysDoHostMumble;
                MumbleTriggerCounter = 0;
                break;
            default:
                break;
        }
    }

    public bool AlwaysDoHostMumble = false;    

    private void DoRandomMumbles()
    {
        if (RandomMumbles.Count == 0 ||
            !PlayRandomMumbles) { return; }

        if (!IsFinished || AlwaysDoHostMumble)
        {
                if (playNextMumble)
                {
                    if (currentSpeaker == Speaker.Host ||
                        AlwaysDoHostMumble)
                    {
                        playNextMumble = false;

                        AudioClip clip = GetNextMumble();
                        Globals.GetInstance().SFXSource.PlayOneShot(clip);
                        StartCoroutine(DelayMumbling(clip.length));
                    }
                }                
        }
    }

    private AudioClip GetNextMumble()
    {
        AudioClip clip = SelectMumble();

        int safety = 0;
        while (previousMumbles.Contains(clip.name) && safety < RandomMumbles.Count)
        {
            clip = SelectMumble();
            safety++; //only search for a new mumble so many times
        }

        previousMumbles.Enqueue(clip.name);

        if (previousMumbles.Count > MumbleRepeatLimit)
        {
            previousMumbles.Dequeue();
        }
        return clip;
    }

    private AudioClip SelectMumble()
    {
        int nextMumble = 0;
        AudioClip clip = null;

        if (VictoryMumblesOnly)
        {
            nextMumble = UnityEngine.Random.Range(0, VictoryMumbles.Count);
            clip = VictoryMumbles[nextMumble];
        }
        else if (DefeatMumblesOnly)
        {
            nextMumble = UnityEngine.Random.Range(0, DefeatMumbles.Count);
            clip = DefeatMumbles[nextMumble];
        }
        else
        {
            nextMumble = UnityEngine.Random.Range(0, RandomMumbles.Count);
            clip = RandomMumbles[nextMumble];
        }
        return clip;
    }

    public int MumbleRepeatLimit = 3;
    public float MumbleDelayOffset = 0.02f;

    private Queue<string> previousMumbles = new Queue<string>();

    private bool playNextMumble = true;    

    private IEnumerator DelayMumbling(float delayLength)
    {
        yield return new WaitForSeconds(delayLength + MumbleDelayOffset);
        playNextMumble = true;
    }

    public void ClearText()
    {
        isTypewriterFinished = false;
        typewriter.SetText("");        
        //typewriter.isActive = true;

        typewriter.Finish();

        //clear message queue
        messageQueue.Clear();
        colorTagExists = false;

        messageScrollview.ResetPosition();
    }

    public void FinishQueue()
    {
        while (messageQueue.Count > 0)
        {
            ProcessMessage(messageQueue.Dequeue());
        }
        typewriter.Finish();
        messageScrollview.ResetPosition();
    }

    private bool colorTagExists = false;

    private void ProcessQueue()
    {
        if (isTypewriterFinished && messageQueue.Count > 0)
        {
            ProcessMessage(messageQueue.Dequeue());
        }
    }

    private void ProcessMessage(MessageItem message)
    {
        string preText = "";

        if (message.Speaker != currentSpeaker)
        {
            //switch to new speaker
            currentSpeaker = message.Speaker;
            
            //clear mumble queue
            previousMumbles.Clear();

            //check for existing text to see if we need to cancel out previous color

            if (colorTagExists) { preText += "[-]"; }

            //change to new color
            switch (currentSpeaker)
            {
                case Speaker.Host:
                    preText += "[" + HostColorHex + "]";
                    break;
                case Speaker.Caller:
                    preText += "[" + CallerColorHex + "]";
                    break;
                case Speaker.System:
                    preText += "[" + SystemColorHex + "]";
                    break;
                default:
                    break;
            }
        }

        //process actual message
        typewriter.AddText(preText + message.Text);
        isTypewriterFinished = false;

        messageScrollview.ResetPosition();
    }

    public void AddLine(string text, bool instant, Speaker speaker)
    {
        //AddLine("[" + colorHex + "]" + text + "[-] ", instant);
        AddText("\r\n" + text, instant, speaker);
        
    }

    //obsolete
    private void AddLine(string text, bool instant)
    {
        isTypewriterFinished = false;
        typewriter.AddText("\r\n" + text);
        //typewriter.isActive = true;

        if (instant)
        {
            typewriter.Finish();
        }

        messageScrollview.ResetPosition();
    }

    public void AddText(string text, bool instant, Speaker speaker)
    {
        //AddText("[" + colorHex + "]" + text + "[-] ", instant);

        messageQueue.Enqueue(new MessageItem(speaker, text));

        if (instant)
        {
            FinishQueue();
        }
    }
    
    //obsolete
    private void AddText(string text, bool instant)
    {
        isTypewriterFinished = false;
        typewriter.AddText(text);
        //typewriter.isActive = true;
        
        if (instant)
        {
            typewriter.Finish();
        }

        messageScrollview.ResetPosition();
    }


    private Queue<MessageItem> messageQueue = new Queue<MessageItem>();

    private Speaker currentSpeaker = Speaker.Host;

    public Speaker CurrentSpeaker { get { return currentSpeaker; } }

    public enum Speaker
    {
        Host,
        Caller,
        System
    }

    private void OnFinished() {
        Debug.Log("Message Finished Displaying!");
        isTypewriterFinished = true;
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

public class MessageItem
{
    public string Text;

    public GameMessageManager.Speaker Speaker;

    public MessageItem(GameMessageManager.Speaker speaker, string text)
    {

        Speaker = speaker;
        Text = text;
    }

}
