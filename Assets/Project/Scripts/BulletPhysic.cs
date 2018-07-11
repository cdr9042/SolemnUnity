using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPhysic : MonoBehaviour {
    public Vector3 velocity;
	// Use this for initialization
	void Start () {
        //GetComponent<GunScript>().SetState(GunScript.State.active);
    }
	
	// Update is called once per frame
	void Update () {
        transform.position += velocity;
	}
}
