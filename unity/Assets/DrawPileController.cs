using UnityEngine;
using System.Collections;

public class DrawPileController : MonoBehaviour {

    public UITable HandTable;
    public GameObject CardPrefab;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void DrawCard()
    {
        NGUITools.AddChild(HandTable.gameObject, CardPrefab);
        HandTable.repositionNow = true;
    }
}
