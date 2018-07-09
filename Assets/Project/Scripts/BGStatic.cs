using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGStatic : MonoBehaviour
{
    private Transform cam;
    // public Transform[] myClone;
    // Use this for initialization
    void Start()
    {
        cam = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
		transform.position = new Vector3(cam.position.x,cam.position.y,transform.position.z);
    }
}
