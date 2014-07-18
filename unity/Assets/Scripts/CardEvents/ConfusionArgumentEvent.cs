using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ConfusionArgumentEvent : CardEvent
{

    bool waitForCard = false;

    GameObject topCard = null;

    bool playCard = false;

    public override bool Execute()
    {
        topCard = DrawPileController.drawPile.DrawCard();

        if (topCard != null)
        {
            GameCard gameCard = topCard.GetComponent<GameCard>();

            if (gameCard != null)
            {
                waitForCard = true;
                if (gameCard.cardType == GameCard.CardType.Argument)
                {
                    CardDisplayController.cardDisplayController.DisplayCard(topCard, CardContainer.CardZone.Play, 1.0f);
                    gameCard.spiteUsed = 0;
                    playCard = true;
                }
                else
                {
                    CardDisplayController.cardDisplayController.DisplayCard(topCard, CardContainer.CardZone.Discard, 1.0f);
                }
            }

        }

        eventFinished = !waitForCard;
        return eventFinished;
    }

    public override void Update()
    {
        if (waitForCard)
        {
            if (CardDisplayController.cardDisplayController.displayedCards.Count == 0)
            {
                waitForCard = false;
                eventFinished = true;
                
                if (playCard)
                {
                    RulesManager.rulesManager.PlayCard(topCard);
                }
            }
        }
    }
}
