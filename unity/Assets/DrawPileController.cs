using UnityEngine;
using System.Collections;

public class DrawPileController : MonoBehaviour {

    public UIWidgetContainer HandTable;
    public GameObject CardPrefab;

    public CardDisplayController DisplayController;

    //private GameObject NewCard;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void DrawCard()
    {

        //NGUITools.AddChild(HandTable.gameObject, CardPrefab);
        
        GameObject newCard = NGUITools.AddChild(gameObject, CardPrefab);

        newCard.transform.localPosition = CardPrefab.transform.localPosition;

        if (newCard == null) { return; }

        //NGUITools.BringForward(NewCard);

        //if (TweenDrawCard != null)
        //{
        //    TweenDrawCard.from = NewCard.transform.position;
        //    TweenDrawCard.to = DisplayPosition;
        //    TweenDrawCard.PlayForward();
        //}

        //ReParentNewCard(newCard);

        DisplayCard(newCard);

        //HandTable.repositionNow = true;
    }

    public void DisplayCard(GameObject cardObject)
    {
        if (DisplayController != null)
        {
            DisplayController.DisplayCard(cardObject);
        }
    }

    public void ReParentNewCard(GameObject cardObject)
    {
        if (cardObject != null && HandTable != null)
        {
            cardObject.transform.parent = HandTable.transform;

            if (HandTable is UIGrid)
            {
                ((UIGrid)HandTable).repositionNow = true;
            }
            else if (HandTable is UITable)
            {
                ((UITable)HandTable).repositionNow = true;
            }
        }
    }
}
