using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TrashSelfEvent : CardEvent
{

    public override bool Execute()
    {

        Destroy(this.gameCard.gameObject);

        eventFinished = true;
        return eventFinished;
    }

}
