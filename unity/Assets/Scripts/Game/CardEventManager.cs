﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CardEventManager : MonoBehaviour
{

    static public CardEventManager cardEventManager;

    public bool processEvents = false;

    private Queue<CardEvent> executionQueue = new Queue<CardEvent>();

    private CardEvent currentEvent;
    
    public void QueueEvents(List<CardEvent> events)
    {
        foreach (CardEvent cardEvent in events)
        {
            executionQueue.Enqueue(cardEvent);
        }
    }

    public void QueueEvents(GameObject cardObject)
    {
        GameCard gameCard = cardObject.GetComponent<GameCard>();

        if (gameCard != null)
        {
            foreach (CardEvent cardEvent in gameCard.cardEvents)
            {
                if (cardEvent != null)
                {
                    executionQueue.Enqueue(cardEvent);
                }
            }
        }
    }

    public void Start()
    {
        //processEvents = true;
        cardEventManager = this;
    }

    public void TurnOnProcessing()
    {
        processEvents = true;
    }

    public void TurnOffProcessing()
    {
        processEvents = false;
    }

    public void Update()
    {
        //if we are currently processing events, go ahead and do it what are you waiting for
        if (processEvents)
        {
            bool processNextEvent = true;

            while (processNextEvent)
            {

                //first check to see if we have an event we are processing already
                if (currentEvent != null)
                {
                    currentEvent.Update();

                    //check to see if the current event has completed
                    processNextEvent = currentEvent.eventFinished;

                    //if it has, remove it
                    if (processNextEvent)
                    {
                        currentEvent = null;
                    }
                }

                //now if we are not waiting on an event, get the next event in the queue and start it
                if (currentEvent == null)
                {
                    if (executionQueue.Count > 0)
                    {
                        //when we grab an event, set the state to inactive
                        TurnManager.turnManager.ChangeState(TurnManager.TurnState.PlayerInactive);

                        currentEvent = executionQueue.Dequeue();

                        //start the next event. if the event finishes immediately, we can loop again and grab another
                        processNextEvent = currentEvent.Execute();
                    }
                    else
                    {
                        //no event in process, and no events in queue
                        processNextEvent = false;

                        //when we have no events left, set the state to active
                        TurnManager.turnManager.ChangeState(TurnManager.TurnState.PlayerActive);                        
                    }
                }

            }

        }
    }
}
