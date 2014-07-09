using UnityEngine;
using System.Collections.Generic;

public class CardController : MonoBehaviour {

    public CardContainer.CardZone CurrentZone;

    static public CardController current;

    public List<EventDelegate> ScaleToLarge = new List<EventDelegate>();
    public List<EventDelegate> ScaleToNormal = new List<EventDelegate>();

    public List<EventDelegate> GoToHand = new List<EventDelegate>();

    public List<EventDelegate> onHoverOver = new List<EventDelegate>();
    public List<EventDelegate> onHoverOut = new List<EventDelegate>();
    public List<EventDelegate> onPress = new List<EventDelegate>();
    public List<EventDelegate> onRelease = new List<EventDelegate>();
    public List<EventDelegate> onSelect = new List<EventDelegate>();
    public List<EventDelegate> onDeselect = new List<EventDelegate>();
    public List<EventDelegate> onClick = new List<EventDelegate>();
    public List<EventDelegate> onDoubleClick = new List<EventDelegate>();
    public List<EventDelegate> onDragOver = new List<EventDelegate>();
    public List<EventDelegate> onDragOut = new List<EventDelegate>();    

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnHover(bool isOver)
    {
        //Debug.Log(Time.time + ":OnHover(" + isOver + "): current " + (current == null ? "=" : "!") + "=null");
        if (current != null) return;
        current = this;
        if (isOver) HoverOver();
        else HoverOut();
        current = null;
    }

    void HoverOver()
    {
        if (CurrentZone != CardContainer.CardZone.Discard &&
            CurrentZone != CardContainer.CardZone.Attached)
        {
            NGUITools.BringForward(gameObject);
            EventDelegate.Execute(ScaleToLarge);
        }
        EventDelegate.Execute(onHoverOver);
    }

    void HoverOut()
    {
        if (CurrentZone != CardContainer.CardZone.Discard &&
            CurrentZone != CardContainer.CardZone.Attached)
        {
            EventDelegate.Execute(ScaleToNormal);
        }        
        EventDelegate.Execute(onHoverOut);
    }

    void OnPress(bool pressed)
    {
        if (current != null) return;
        current = this;
        if (pressed) Press();
        else Release();
        current = null;
    }

    void Press()
    {
        //collider.enabled = false;
        UpdateCurrentZone();
        if (CurrentZone != CardContainer.CardZone.Discard &&
            CurrentZone != CardContainer.CardZone.Attached)
        {
            NGUITools.BringForward(gameObject);
            EventDelegate.Execute(ScaleToLarge);
        }

        EventDelegate.Execute(onPress);
    }

    void Release()
    {
        //collider.enabled = true;
        //if (CurrentZone != CardContainer.CardZone.Discard &&
        //    CurrentZone != CardContainer.CardZone.Attached)
        //{
            EventDelegate.Execute(ScaleToNormal);
       // }
        EventDelegate.Execute(onRelease);
    }

    void OnSelect(bool selected)
    {
        if (current != null) return;
        current = this;
        if (selected) Select();
        else Deselect();
        current = null;
    }

    void Select()
    {
        EventDelegate.Execute(onSelect);
    }

    void Deselect()
    {
        EventDelegate.Execute(onDeselect);
    }

    void OnClick()
    {
        if (current != null) return;
        current = this;
        Click();
        current = null;
    }

    void Click()
    {
        EventDelegate.Execute(onClick);
    }

    void OnDoubleClick()
    {
        if (current != null) return;
        current = this;
        DoubleClick();
        current = null;
    }

    void DoubleClick()
    {
        EventDelegate.Execute(onDoubleClick);
    }

    void OnDragOver(GameObject go)
    {
        if (current != null) return;
        current = this;
        DragOver(go);
        current = null;
    }

    void DragOver(GameObject go)
    {

        EventDelegate.Execute(onDragOver);
    }

    void OnDragOut(GameObject go)
    {
        if (current != null) return;
        current = this;
        DragOut(go);
        current = null;
    }

    void DragOut(GameObject go)
    {
        EventDelegate.Execute(onDragOut);
    }

    public void UpdateCurrentZone()
    {
        CardContainer currentContainer = GetComponentInParent<CardContainer>();

        if (currentContainer != null)
        {
            CurrentZone = currentContainer.cardZone;
        }
    }

    public bool CanAttach(CardController cardController)
    {
        return CurrentZone == CardContainer.CardZone.Play;
    }
}
