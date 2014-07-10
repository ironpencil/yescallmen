using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CardEvent : MonoBehaviour
{

    public string EventName = "";

    public GameObject cardObject = null;

    public bool eventFinished = false;

    public virtual bool Execute()
    {        
        eventFinished = true;
        return eventFinished;
    }

    //public List<CardEvent> OnFinished = new List<CardEvent>();
}
