using UnityEngine;
using System.Collections.Generic;

public class UpdateAttachedCardsPosition : MonoBehaviour
{

    public UIGrid Grid;
    // Use this for initialization
    void Start()
    {
        if (Grid == null)
        {
            Grid = gameObject.GetComponent<UIGrid>();
        }

        if (Grid != null)
        {
            Grid.onReposition += RepositionAttachedCards;
            Grid.Reposition();
            gameObject.GetComponentInParent<UIScrollView>().ResetPosition();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Recalculate the position of all the elements of the table, to change the pivot
    /// </summary>


    public void RepositionAttachedCards()
    {
        if (Grid != null)
        {
            BetterList<Transform> children = Grid.GetChildList();

            //CenterIfNoScrollNeeded(scrollView);

        }
    }
}