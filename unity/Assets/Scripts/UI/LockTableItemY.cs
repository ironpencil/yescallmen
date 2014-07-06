using UnityEngine;
using System.Collections.Generic;

public class LockTableItemY : MonoBehaviour {

    public float LockedY = 0.0f;

    public UITable Table;

	// Use this for initialization
	void Start () {
        Table.onReposition += ResetYPosition;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void ResetYPosition()
    {
        if (Table != null)
        {
            List<Transform> children = Table.children;

            foreach (Transform child in children)
            {
                child.localPosition = new Vector3(child.localPosition.x, LockedY, child.localPosition.z);
            }
        }
    }
}
