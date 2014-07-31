using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardController : MonoBehaviour {

    public CardContainer.CardZone CurrentZone;
    public CardContainer CurrentContainer;

    public GameCard gameCard;

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

    public List<UITweener> onTrashTweens = new List<UITweener>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private bool isTrashing = false;

    public bool IsTrashing { get { return isTrashing; } }

    public float TrashDelay = 1.0f;

    public bool doPlayCardEffects = false;

    public void DoTrashAnimation()
    {
        if (!isTrashing)
        {
            isTrashing = true;

            //UIWidget cardWidget = gameObject.GetComponent<UIWidget>();
            //cardWidget.pivot = UIWidget.Pivot.Top;
            //gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, 0, gameObject.transform.localPosition.y);
            //foreach (UITweener tween in onTrashTweens)
            //{
            //    tween.PlayForward();
            //}

            StartCoroutine(TrashCardAfterDelay());
        }
    }

    private IEnumerator TrashCardAfterDelay()
    {
        yield return new WaitForSeconds(TrashDelay);
        foreach (UITweener tween in onTrashTweens)
        {
            tween.PlayForward();
        }
    }

    void DestroyCard()
    {
        CardZoneManager.cardZoneManager.RepositionCardGridNextUpdate(gameObject);
        UnityEngine.Object.Destroy(gameObject);        
    }

    public void DoScaleToNormal()
    {
        if (!isTrashing)
        {
            EventDelegate.Execute(ScaleToNormal);            
        }
    }

    public void OnScaleComplete()
    {
        if (Mathf.Approximately(transform.localScale.x, 1.0f))
        {
            //scaled to 1
            if (CurrentZone == CardContainer.CardZone.Play &&
                doPlayCardEffects)
            {
                doPlayCardEffects = false;                
                StartCoroutine(DoPlayCardEffects());
                //SFXManager.instance.PlaySound(SFXManager.instance.PlayCardSound, 1.0f);
                //float shakeIntensity = Mathf.Min(0.01 + gameCard.Level * 
                //BattleManager.battleManager.CameraShaker.Shake(
            }
        }
    }
    

    public void DoScaleToLarge()
    {
        if (!isTrashing)
        {
            EventDelegate.Execute(ScaleToLarge);
        }
    }

    void OnHover(bool isOver)
    {
        Debug.Log(Time.time + ":OnHover(" + isOver + "): current " + (current == null ? "=" : "!") + "=null");
        if (current != null) return;
        current = this;
        if (isOver) HoverOver();
        else HoverOut();
        current = null;
    }

    bool playRollOver = true;

    void HoverOver()
    {
        if (CurrentZone == CardContainer.CardZone.Hand ||
            //CurrentZone == CardContainer.CardZone.Play ||
            //(CurrentZone == CardContainer.CardZone.Display &&
            //CardDisplayController.cardDisplayController.CurrentDisplayMode == CardDisplayController.DisplayMode.Selection))
            CurrentZone == CardContainer.CardZone.Display)
        {
            NGUITools.BringForward(gameObject);
            //EventDelegate.Execute(ScaleToLarge);
            DoScaleToLarge();
            if (playRollOver)
            {
                SFXManager.instance.PlaySound(SFXManager.instance.RolloverCardSound, 0.6f);
            }
            playRollOver = false;
        }
        EventDelegate.Execute(onHoverOver);
    }

    private IEnumerator DoPlayCardEffects()
    {
        yield return new WaitForSeconds(0.01f);

        BattleManager.battleManager.CameraShaker.StartShaking();
        SFXManager.instance.PlaySound(SFXManager.instance.PlayCardSound, 1.0f);
    }

    void HoverOut()
    {
        if (CurrentZone == CardContainer.CardZone.Hand ||
            CurrentZone == CardContainer.CardZone.Play ||
            //(CurrentZone == CardContainer.CardZone.Display &&
            //CardDisplayController.cardDisplayController.CurrentDisplayMode == CardDisplayController.DisplayMode.Selection))
            CurrentZone == CardContainer.CardZone.Display)
        {
            //EventDelegate.Execute(ScaleToNormal);
            DoScaleToNormal();
        }        
        EventDelegate.Execute(onHoverOut);
        playRollOver = true;
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
        if (CurrentZone == CardContainer.CardZone.Hand ||
            //CurrentZone == CardContainer.CardZone.Play ||
            //(CurrentZone == CardContainer.CardZone.Display &&
            //CardDisplayController.cardDisplayController.CurrentDisplayMode == CardDisplayController.DisplayMode.Selection))
            CurrentZone == CardContainer.CardZone.Display)
        {
            NGUITools.BringForward(gameObject);
            //EventDelegate.Execute(ScaleToLarge);
            DoScaleToLarge();
        }

        EventDelegate.Execute(onPress);
    }

    void Release()
    {
        //collider.enabled = true;
        //if (CurrentZone != CardContainer.CardZone.Discard &&
        //    CurrentZone != CardContainer.CardZone.Attached)
        //{
        //if (CurrentZone != CardContainer.CardZone.Display)
        //{
            //EventDelegate.Execute(ScaleToNormal);
        DoScaleToNormal();
        //}
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

        CardZoneManager.cardZoneManager.DoClickCard(this);
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
        CurrentContainer = GetComponentInParent<CardContainer>();

        if (CurrentContainer != null)
        {
            CurrentZone = CurrentContainer.cardZone;
        }
        else
        {
            CurrentZone = CardContainer.CardZone.None;
        }

        DragDropCard ddCard = gameObject.GetComponent<DragDropCard>();

        if (ddCard != null)
        {
            switch (CurrentZone)
            {
                case CardContainer.CardZone.None:
                    break;
                case CardContainer.CardZone.Hand:
                    ddCard.restriction = UIDragDropItem.Restriction.Vertical;
                    break;
                case CardContainer.CardZone.Play:
                    ddCard.restriction = UIDragDropItem.Restriction.Vertical;
                    break;
                case CardContainer.CardZone.Attached:
                    break;
                case CardContainer.CardZone.Discard:
                    ddCard.restriction = UIDragDropItem.Restriction.None;
                    break;
                case CardContainer.CardZone.Display:
                    ddCard.restriction = UIDragDropItem.Restriction.None;
                    break;
                case CardContainer.CardZone.Selection:
                    ddCard.restriction = UIDragDropItem.Restriction.None;
                    break;
                default:
                    break;
            }
        }
    }

    public bool CanAttach(CardController cardController)
    {
        //return CurrentZone == CardContainer.CardZone.Play;
        return false;
    }
}
