using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class DiscardPileController : MonoBehaviour {

    public static DiscardPileController discardPileController;

    public float ShuffleIntoHandStrength = 5.0f;

	// Use this for initialization
	void Start () {
        discardPileController = this;
        //DeckManager.deckManager.onShuffleDiscardIntoDeck += ClearDiscardPile;
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
            GameObject child = this.transform.GetChild(i).gameObject;

            GameCard gameCard = child.GetComponent<GameCard>();

            //everything added to discard as gained cards should already have been gained
            gameCard.isGainedCard = false;

            //just destroy evertying for testing
            //Destroy(child.gameObject);

            //clear the discard list
            DeckManager.deckManager.discard = new List<CardDefinition>();

            CardZoneManager.cardZoneManager.SetZoneRepositionStrength(CardContainer.CardZone.Deck, ShuffleIntoHandStrength);
            //move card will handle adding the cards to our deck list
            CardZoneManager.cardZoneManager.MoveCardToZone(child, CardContainer.CardZone.Deck);

            //clear the discard list
            //DeckManager.deckManager.discard = new List<CardDefinition>();
                //Destroy(this.transform.GetChild(i).gameObject);        
        }            
    }
}
