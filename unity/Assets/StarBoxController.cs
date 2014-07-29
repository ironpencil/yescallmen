using UnityEngine;
using System.Collections;

public class StarBoxController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public float delay = 0.0f;

    public void TurnOn()
    {
        NGUITools.SetActiveSelf(gameObject, true);
        //StartCoroutine(LetThereBeLight());
    }

    private IEnumerator LetThereBeLight()
    {
        yield return new WaitForSeconds(delay);

        
    }
}
