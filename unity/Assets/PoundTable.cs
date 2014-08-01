using UnityEngine;
using System.Collections;

public class PoundTable : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Pound()
    {
        StartCoroutine(DoPoundTable());

        return; 

        SFXManager.instance.PlaySound(SFXManager.instance.PlayCardSound, 1.0f);

        BattleManager.battleManager.CameraShaker.addShakeAmount *= 4;
        BattleManager.battleManager.CameraShaker.ShakeWithoutUpdatePosition();
        BattleManager.battleManager.CameraShaker.addShakeAmount *= 0.25f;
    }

    public IEnumerator DoPoundTable()
    {
        SFXManager.instance.PlaySound(SFXManager.instance.PlayCardSound, 1.0f);

        yield return new WaitForSeconds(0.1f);

        BattleManager.battleManager.CameraShaker.addShakeAmount *= 4;
        BattleManager.battleManager.CameraShaker.ShakeWithoutUpdatePosition();
        BattleManager.battleManager.CameraShaker.addShakeAmount *= 0.25f;
    }
}
