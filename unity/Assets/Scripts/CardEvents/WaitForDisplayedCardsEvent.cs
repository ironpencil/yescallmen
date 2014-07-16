using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class WaitForDisplayedCardsEvent : CardEvent
{

    public override bool Execute()
    {
        eventFinished = CardDisplayController.cardDisplayController.displayedCards.Count == 0;
        return eventFinished;
    }

    public override void Update()
    {
        eventFinished = CardDisplayController.cardDisplayController.displayedCards.Count == 0;
    }
    
}
