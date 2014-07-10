using System;
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

    public void Start()
    {
        processEvents = true;
        cardEventManager = this;
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
                        currentEvent = executionQueue.Dequeue();

                        //start the next event. if the event finishes immediately, we can loop again and grab another
                        processNextEvent = currentEvent.Execute();
                    }
                    else
                    {
                        //no event in process, and no events in queue
                        processNextEvent = false;
                    }
                }

            }

        }
    }
}
