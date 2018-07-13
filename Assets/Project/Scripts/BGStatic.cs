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
        if (cam != null)
            transform.position = new Vector3(cam.position.x, cam.position.y, transform.position.z);
        else
        {
            cam = Camera.main.transform;
        }
    }
}
