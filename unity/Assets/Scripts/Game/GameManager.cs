using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public List<GameObject> InactiveObjects;

	// Use this for initialization
	void Start () {
        foreach (GameObject go in InactiveObjects)
        {
            go.SetActive(false);            
        }    
	}
	
	// Update is called once per frame
	void Update () {
	

	}
}
