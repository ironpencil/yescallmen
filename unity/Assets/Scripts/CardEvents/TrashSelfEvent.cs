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
        Destroy(this.gameCard.gameObject);

        eventFinished = true;
        return eventFinished;
    }

}
