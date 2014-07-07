using UnityEngine;
using System.Collections.Generic;

public class OnRepositionListener : MonoBehaviour {

    public UIWidgetContainer Caller;

    public List<EventDelegate> OnReposition = new List<EventDelegate>();

	// Use this for initialization
	void Start () {
        
        if (Caller != null) {

            if (Caller is UIGrid)
            {
                ((UIGrid)Caller).onReposition += onReposition;
            }
            else if (Caller is UITable)
            {
                ((UITable)Caller).onReposition += onReposition;
            }
        }
            
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void onReposition()
    {
        EventDelegate.Execute(OnReposition);
    }


}
