using UnityEngine;
using System.Collections.Generic;

public class RepositionTablePivot : MonoBehaviour {

    public enum Pivot
    {
        Left,
        Center,
        Right
    }

    public Pivot pivot = Pivot.Left;

    public UITable Table;
	// Use this for initialization
	void Start () {
        Debug.Log(Time.time + "::RepositionTablePivot::Start() : Table = " + Table);
        if (Table == null)
        {
            Table = gameObject.GetComponent<UITable>();
        }

        if (Table != null)
        {
            Table.onReposition += RepositionPivot;
            Table.Reposition();
            gameObject.GetComponentInParent<UIScrollView>().ResetPosition();
        }        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// Recalculate the position of all the elements of the table, to change the pivot
    /// </summary>


    public void RepositionPivot()
    {
        if (Table != null)
        {
            List<Transform> children = Table.children;

            float Width = 0;
            if (pivot != Pivot.Left)
                Width = NGUIMath.CalculateRelativeWidgetBounds(transform).size.x;

            for (int i = 0, imax = children.Count; i < imax; ++i)
            {
                Transform t = children[i];
                Vector3 pos = t.localPosition;
                if (pivot == Pivot.Center)
                    pos.x -= Width / 2 + Table.padding.x;
                else if (pivot == Pivot.Right)
                    pos.x -= Width + Table.padding.x;
                else if (pivot == Pivot.Left)
                    pos.x -= Table.padding.x;

                t.localPosition = pos;
            }

            UIScrollView scrollView = gameObject.GetComponentInParent<UIScrollView>();

            if (scrollView != null)
            {
                scrollView.InvalidateBounds();
                scrollView.RestrictWithinBounds(false);
                scrollView.UpdateScrollbars(false);
                scrollView.onDragFinished();
            }
            //CenterIfNoScrollNeeded(scrollView);

        }
    }
}
