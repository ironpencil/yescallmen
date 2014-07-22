using UnityEngine;
using System.Collections;

public class OutOfBattleManager : MonoBehaviour {

    public static OutOfBattleManager outOfBattleManager;

	// Use this for initialization
    void Start()
    {
        outOfBattleManager = this;
    }
	
	// Update is called once per frame
	void Update () {
        PointsLabel.text = Globals.GetInstance().FeministsConvertedString;
	
	}

    public UILabel PointsLabel;
}
