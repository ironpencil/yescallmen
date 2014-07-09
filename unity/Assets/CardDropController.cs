using UnityEngine;
using System.Collections;

public class CardDropController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnDrop(GameObject go)
    {
        Debug.Log("OnDrop() of " + go.name + " detected by " + gameObject.name);
    }
}
