using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class GainCardEvent : CardEvent
{
    public CardDefinition cardToGain = null;

    public CardContainer.CardZone addToZone = CardContainer.CardZone.Hand;

    public override bool Execute()
    {
        GameObject newCard = null;
        GameObject parent = CardZoneManager.cardZoneManager.displayContainer;

        if (cardToGain != null)
        {
            newCard = CardFactory.cardFactory.CreateCard(cardToGain, parent);

            GameCard gameCard = newCard.GetComponent<GameCard>();

            //gameCard.isGainedCard = true;

            DeckManager.deckManager.GainCard(gameCard.cardDefinition);

            UIWidget cardWidget = gameCard.gameObject.GetComponent<UIWidget>();
            cardWidget.alpha = 0.0f;

            TweenAlpha.Begin(newCard.gameObject, 0.5f, 1.0f);

            //GameMessageManager.gameMessageManager.AddLine("Drew card: " + gameCard.Title, false);
            CardDisplayController.cardDisplayController.DisplayCard(newCard, addToZone, 1.0f);

        }

        eventFinished = true;
        return eventFinished;
    }

}
