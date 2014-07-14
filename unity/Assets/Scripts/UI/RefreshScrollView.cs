using UnityEngine;
using System.Collections;

public class RefreshScrollView : MonoBehaviour {

    public UIScrollView scrollView;
    
    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Refresh()
    {
        if (scrollView != null)
        {
            //Debug.Log("Refreshing Scroll View");
            scrollView.InvalidateBounds();
            //scrollView.RestrictWithinBounds(false);
            scrollView.UpdateScrollbars(false);
            scrollView.onDragFinished();
        }
    }


}
