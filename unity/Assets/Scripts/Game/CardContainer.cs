using UnityEngine;
using System.Collections;

public class CardContainer : MonoBehaviour {

    public CardZone cardZone;

    public enum CardZone
    {
        None,
        Hand,
        Play,
        Attached,
        Discard,
        Display,
        Selection
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
