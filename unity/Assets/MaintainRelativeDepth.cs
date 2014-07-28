using UnityEngine;
using System.Collections;

public class MaintainRelativeDepth : MonoBehaviour {

    private UIWidget thisWidget;
    public UIWidget otherWidget;

    public int DepthOffset = 1;
	// Use this for initialization
	void Start () {
        thisWidget = gameObject.GetComponent<UIWidget>();
	
	}
	
	// Update is called once per frame
	void Update () {

        thisWidget.depth = otherWidget.depth + DepthOffset;
	
	}
}
