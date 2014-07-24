using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardSelectionController : MonoBehaviour {

    public static CardSelectionController cardSelectionController;

    public int SlotCount = 1;
    public int CardsRequired = 0;

    private int filledSlots = 0;
    public int FilledSlots
    {
        get
        {
            return filledSlots;
        }
        set
        {
            filledSlots = value;
            //if (Buttons == ButtonOption.okCancel)
            //{
                Debug.Log("Filled slots changed to " + filledSlots + ".");
                OKButton.isEnabled = (filledSlots > 0 && filledSlots >= CardsRequired);
                Debug.Log("OKButton.isEnabled should be " + (filledSlots > 0).ToString() + " and is " + OKButton.isEnabled + ".");
            //}
        }
    }

    private bool shouldShow = false;

    
    public bool Finished = false;

    public bool CanCancel = true;

    public List<CardSlotController> Slots = new List<CardSlotController>();
    public UIGrid SlotGrid;
    public UILabel PromptLabel;
    public UIButton OKButton;
    public UILabel OKButtonLabel;
    public UIButton CancelButton;
    public UILabel CancelButtonLabel;

    public delegate bool CanSlotCard(CardController card);

    public CanSlotCard canSlotCard;

    public enum SelectionResult
    {
        OK,
        Cancel
    }

    public SelectionResult Result = SelectionResult.Cancel;

    public enum ButtonOption
    {
        okOnly,
        okCancel
    }

    public ButtonOption Buttons = ButtonOption.okCancel;

    public CardContainer.CardZone SourceCardZone = CardContainer.CardZone.None;

	// Use this for initialization
	void Start () {
        cardSelectionController = this;
        NGUITools.SetActive(this.gameObject, false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public delegate void OnFinish();

    public OnFinish onFinish;

    public void Setup(string promptText, ButtonOption buttons, bool canCancel, int slotCount, int cardsRequired, CardContainer.CardZone cardSource)
    {
        PromptLabel.text = promptText;
        SlotCount = slotCount;
        CardsRequired = cardsRequired;

        SourceCardZone = cardSource;

        CanCancel = canCancel;

        int slotsActivated = 0;
        foreach (CardSlotController slot in Slots)
        {
            if (slotsActivated < SlotCount)
            {
                NGUITools.SetActive(slot.gameObject, true);
                slotsActivated++;
            }
            else
            {
                NGUITools.SetActive(slot.gameObject, false);
            }
        }

        if (buttons == ButtonOption.okCancel)
        {
            NGUITools.SetActive(CancelButton.gameObject, true);
            CancelButton.isEnabled = true;
            CancelButtonText = cancelButtonDefaultText;

            //we disable OK button if Cancel button exists
            //and enable it when cards are added
            OKButton.isEnabled = false;
        }
        else
        {
            NGUITools.SetActive(CancelButton.gameObject, false);
        }

        NGUITools.SetActive(OKButton.gameObject, true);
        OKButton.isEnabled = false;
        OKButtonText = okButtonDefaultText;

        if (CardsRequired > 0)
        {
            OKButton.isEnabled = false;
            CancelButton.isEnabled = CanCancel;
        }

        SlotGrid.repositionNow = true;
    }

    private static string okButtonDefaultText = "OK";
    public string OKButtonText
    {
        get
        {
            return OKButtonLabel.text;
        }
        set
        {
            OKButtonLabel.text = value;
        }
    }

    private static string cancelButtonDefaultText = "Cancel";
    public string CancelButtonText
    {
        get
        {
            return CancelButtonLabel.text;
        }
        set
        {
            CancelButtonLabel.text = value;
        }
    }

    public void Show()
    {
        shouldShow = true;
        NGUITools.SetActive(this.gameObject, true);

        TweenScale tweenScale = gameObject.GetComponent<TweenScale>();
        if (tweenScale != null)
        {
            tweenScale.PlayForward();
        }

        Finished = false;

        foreach (CardSlotController cardSlot in Slots)
        {
            if (cardSlot.gameObject.activeSelf)
            {
                NGUITools.BringForward(cardSlot.gameObject);
            }
        }

        if (OKButton.gameObject.activeSelf)
        {
            NGUITools.BringForward(OKButton.gameObject);
        }

        if (CancelButton.gameObject.activeSelf)
        {
            NGUITools.BringForward(CancelButton.gameObject);
        }

        //Globals.GetInstance().DebugWidgetDepths = true;

        //NGUITools.PushBack(backgroundWidget.gameObject);
    }

    public void Close()
    {
        TweenScale tweenScale = gameObject.GetComponent<TweenScale>();
        if (tweenScale != null)
        {
            tweenScale.PlayReverse();
        }

        SlotCount = 0;
        CardsRequired = 0;
        SourceCardZone = CardContainer.CardZone.None;
        FilledSlots = 0;

        CanCancel = true;

        shouldShow = false;
        StartCoroutine(SetInactiveAfterSeconds(1.0f));
        onFinish = null;
        canSlotCard = null;

        //Globals.GetInstance().DebugWidgetDepths = false;
    }

    private IEnumerator SetInactiveAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (!shouldShow)
        {
            NGUITools.SetActive(this.gameObject, false);
        }
    }

    private void Finish()
    {
        Finished = true;

        if (onFinish != null)
        {
            onFinish();
        }
        
    }

    public void DoOK()
    {
        //do stuff for OK
        Result = SelectionResult.OK;
        Finish();
    }

    public void DoCancel()
    {
        Result = SelectionResult.Cancel;
        Finish();
    }

    public List<CardController> GetCards()
    {
        List<CardController> slottedCards = CardZoneManager.cardZoneManager.GetCardsInZone(CardContainer.CardZone.Selection);

        return slottedCards;        
    }
}
