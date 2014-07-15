using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CardEvent : ScriptableObject
{

    public string EventName = "";

    public bool eventFinished = false;

    public GameCard gameCard = null;

    public virtual bool Execute()
    {        
        eventFinished = true;
        return eventFinished;
    }

    public virtual void Update()
    {

    }

    //public List<CardEvent> OnFinished = new List<CardEvent>();
}
