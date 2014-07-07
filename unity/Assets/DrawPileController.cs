using UnityEngine;
using System.Collections;

public class DrawPileController : MonoBehaviour {

    public UIWidgetContainer HandTable;
    public GameObject CardPrefab;

    public TweenPosition TweenDrawCard;

    public Vector3 DisplayPosition;

    private GameObject NewCard;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void DrawCard()
    {

        //NGUITools.AddChild(HandTable.gameObject, CardPrefab);
        
        NewCard = NGUITools.AddChild(gameObject, CardPrefab);

        NewCard.transform.localPosition = CardPrefab.transform.localPosition;

        if (NewCard == null) { return; }

        NGUITools.BringForward(NewCard);

        //if (TweenDrawCard != null)
        //{
        //    TweenDrawCard.from = NewCard.transform.position;
        //    TweenDrawCard.to = DisplayPosition;
        //    TweenDrawCard.PlayForward();
        //}

        ReParentNewCard();
        
        //HandTable.repositionNow = true;
    }

    public void ReParentNewCard()
    {
        if (NewCard != null && HandTable != null)
        {
            NewCard.transform.parent = HandTable.transform;

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
