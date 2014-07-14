using UnityEngine;
using System.Collections;

public class DiscardPileController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        DeckManager.deckManager.onShuffleDiscardIntoDeck += ClearDiscardPile;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ClearDiscardPile()
    {
        int childCount = transform.childCount;

        for (int i = childCount-1; i >= 0; i--)
        {
            //should do a nicer destroy later
            Destroy(this.transform.GetChild(i).gameObject);        
        }            
    }
}
