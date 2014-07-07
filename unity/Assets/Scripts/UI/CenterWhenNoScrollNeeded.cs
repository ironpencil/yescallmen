using UnityEngine;
using System.Collections;

public class CenterWhenNoScrollNeeded : MonoBehaviour {

    public UIScrollView ScrollView;
    //public UIScrollBar[] ScrollBars;
	// Use this for initialization
	void Start () {
        Debug.Log(Time.time + "::CenterWhenNoScrollNeeded::Start() : ScrollView = " + ScrollView);
        if (ScrollView != null)
        {
            ScrollView.onDragFinished += CenterIfNoScrollNeeded;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void CenterIfNoScrollNeeded()
    {
        if (ScrollView != null)
        {
            //foreach (UIScrollBar scrollBar in ScrollBars)
            //{
                //if (scrollBar.fillDirection == UIScrollBar.FillDirection.LeftToRight ||
                //    scrollBar.fillDirection == UIScrollBar.FillDirection.RightToLeft)
                //{
                    //horizontal scrollbar
            if (!ScrollView.shouldMoveHorizontally)
            {
                bool restrictWithinPanel = ScrollView.restrictWithinPanel;
                Debug.Log("Spring Scrollview back to center");
                ScrollView.restrictWithinPanel = false;
                SpringPanel.Begin(ScrollView.gameObject, Vector3.zero, 13f);
                ScrollView.restrictWithinPanel = restrictWithinPanel;
            }
            else
            {
                //ScrollView.restrictWithinPanel = true;
            }
                //}
                //else
                //{
                    //vertical scrollbar
                    //if (!ScrollView.shouldMoveVertically)
                    //{
                   //     scrollBar.value = 0.5f;
                    //}
                //}
            
        }
    }
}
