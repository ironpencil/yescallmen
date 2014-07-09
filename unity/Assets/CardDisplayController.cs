using UnityEngine;
using System.Collections.Generic;

public class CardDisplayController : MonoBehaviour {

    public UIGrid displayGrid;
    public UIGrid handGrid;

    //public UIScrollView handScrollView;

    private List<DisplayedCard> displayedCards = new List<DisplayedCard>();
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        List<DisplayedCard> removeCards = new List<DisplayedCard>();

        foreach (DisplayedCard card in displayedCards)
        {
            card.Update();
            if (card.timeUp)
            {
                MoveCardToHand(card);
                removeCards.Add(card);
            }
        }

        foreach (DisplayedCard card in removeCards)
        {
            displayedCards.Remove(card);
        }

	}

    public void DisplayCard(GameObject cardObject)
    {
        
        TweenScale.Begin(cardObject, 0.5f, new Vector3(1.5f, 1.5f, 0.0f)).PlayForward(); 
        cardObject.transform.parent = displayGrid.transform;
        NGUITools.BringForward(cardObject);
        displayGrid.repositionNow = true;

        NGUITools.MarkParentAsChanged(cardObject);
               

        displayedCards.Add(new DisplayedCard(cardObject, 3.0f, true));
    }

    public void MoveCardToHand(DisplayedCard displayedCard)
    {
        GameObject cardObject = displayedCard.cardObject;

        TweenScale.Begin(cardObject, 0.5f, new Vector3(1.0f, 1.0f, 0.0f)).PlayForward();

        cardObject.transform.parent = handGrid.transform;
        handGrid.repositionNow = true;
        displayGrid.repositionNow = true;

        //have to enable/re-enable the scroll view to get it to update to the new parent
        UIDragScrollView dragScrollView = cardObject.GetComponent<UIDragScrollView>();

        if (dragScrollView != null)
        {
            //dragScrollView.scrollView = handScrollView;
            dragScrollView.enabled = false;
            dragScrollView.enabled = true;
        }

        NGUITools.MarkParentAsChanged(cardObject);
        

        //displayedCards.Remove(displayedCard);
    }
}

public class DisplayedCard
{
    public GameObject cardObject = null;
    public float displayTime = 0.0f;
    public bool timeUp = false;
    
    private float currentTime = 0.0f;

    private bool countTime = false;

    public DisplayedCard(GameObject cardObject, float displayTime, bool start)
    {
        this.cardObject = cardObject;
        this.displayTime = displayTime;

        if (start) { StartTimer(); }
    }

    public void ResetTimer()
    {
        currentTime = 0.0f;
    }

    public void StartTimer()
    {
        StartTimer(true);
    }

    public void StartTimer(bool reset)
    {
        if (reset) { ResetTimer(); }

        countTime = true;
        timeUp = false;
    }

    public void StopTimer()
    {
        countTime = false;
    }

    public void Update()
    {
        if (countTime)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= displayTime)
            {
                timeUp = true;
                StopTimer();
            }
        }
    }
}
