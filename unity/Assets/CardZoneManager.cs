using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardZoneManager : MonoBehaviour {

    public static CardZoneManager cardZoneManager;

    public GameObject handContainer;
    public GameObject playContainer;
    public GameObject discardContainer;
    public GameObject displayContainer;

	// Use this for initialization
	void Start () {
        cardZoneManager = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public List<CardController> GetCardsInZone(CardContainer.CardZone zone)
    {
        GameObject targetObject = null;

        switch (zone)
        {
            case CardContainer.CardZone.Hand:
                targetObject = handContainer;
                break;
            case CardContainer.CardZone.Play:
                targetObject = playContainer;
                break;
            case CardContainer.CardZone.Discard:
                targetObject = discardContainer;
                break;
            case CardContainer.CardZone.Display:
                targetObject = displayContainer;
                break;
            default:
                break;
        }

        List<CardController> cardList = new List<CardController>();

        if (targetObject != null)
        {
            foreach (CardController card in targetObject.transform.GetComponentsInChildren<CardController>())
            {
                if (card.CurrentZone == zone)
                {
                    cardList.Add(card);
                }
            }
        }

        return cardList;
    }

    public static CardContainer.CardZone FindObjectZone(GameObject go)
    {
        CardContainer cardContainer = go.GetComponentInParent<CardContainer>();

        CardContainer.CardZone returnZone = CardContainer.CardZone.None;

        if (cardContainer != null)
        {
            returnZone = cardContainer.cardZone;
        }

        return returnZone;

    }
}
