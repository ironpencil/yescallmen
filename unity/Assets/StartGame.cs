using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour {

    public GameObject go;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Play()
    {
        if (go != null)
        {
            NGUITools.SetActive(go, true);
            NGUITools.SetActive(gameObject, false);
        }
    }
}
