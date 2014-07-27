using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public List<GameObject> ActivateObjects;

	// Use this for initialization
	void Start () {
        foreach (GameObject go in ActivateObjects)
        {
            go.SetActive(true);            
        }    
	}
	
	// Update is called once per frame
	void Update () {
	

	}
}
