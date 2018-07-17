using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextStageCheck : MonoBehaviour {
	public bool PlayerEnter;
    public Transform _StageMaster;
	// Use this for initialization
	void Start () {
		
	}

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag == "Player")
        {
            PlayerEnter = true;
            _StageMaster.GetComponent<GotoNextStage>().goToNextStage();
        }
	}
}
