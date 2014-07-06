using UnityEngine;
using System.Collections.Generic;

public class RepositionTableScrollView : MonoBehaviour {

    public UITable Table;
	// Use this for initialization
	void Start () {
        if (Table == null)
        {
            Table = gameObject.GetComponent<UITable>();
        }

        if (Table != null)
        {
            Table.onReposition += RepositionUpdateScrollView;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// Recalculate the position of all the elements of the table, to change the pivot
    /// </summary>


    public void RepositionUpdateScrollView()
    {
        if (Table != null)
        {
            //gameObject.GetComponentInParent<UIScrollView>().InvalidateBounds();
           // gameObject.GetComponentInParent<UIScrollView>().RestrictWithinBounds(false);
            //gameObject.GetComponentInParent<UIScrollView>().UpdateScrollbars();
        }
    }
}
