using UnityEngine;
using System.Collections;

public class PointsDisplay : MonoBehaviour {

    public UILabel PointsLabel;

    public static PointsDisplay pointsDisplay;
	// Use this for initialization
	void Start () {
        pointsDisplay = this;

        PointsLabel.text = Globals.GetInstance().FeministsConvertedString;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    [ContextMenu("Refresh Display")]
    public void RefreshPointsDisplay()
    {
        Globals.GetInstance().CalculateFeministsConverted();
        PointsLabel.text = Globals.GetInstance().FeministsConvertedString;
    }

}
