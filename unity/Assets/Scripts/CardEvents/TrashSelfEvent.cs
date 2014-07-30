using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TrashSelfEvent : CardEvent
{

    public override bool Execute()
    {
        DeckManager.deckManager.TrashCard(gameCard.cardDefinition);

        CardController card = gameCard.gameObject.GetComponent<CardController>();

        CardZoneManager.cardZoneManager.MoveCardToZone(card.gameObject, CardContainer.CardZone.None, false);
        //card.DoTrashAnimation();

        //UnityEngine.Object.Destroy(this.gameCard.gameObject);

        eventFinished = true;
        return eventFinished;
    }

}
