using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.Pooling;

public class removeSelfAfterAnimation : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void returnToCache()
    {
        SpawningPool.ReturnToCache(gameObject);
    }

    public void OnParticleSystemStopped()
    {
        Debug.Log("callback");
        returnToCache();
    }
}
