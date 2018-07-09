using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGClone : MonoBehaviour
{
    public Transform[] backgrounds;
    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            Vector3 pos;
            for (int j = 0; j < 10; j++)
            {
                pos = new Vector3(
                                backgrounds[i].transform.position.x + backgrounds[i].GetComponent<SpriteRenderer>().size.x * j / 2,
                                backgrounds[i].transform.position.y,
                                backgrounds[i].transform.position.z);
                // Vector3 pos = new Vector3(backgrounds[i].Transform.position.x);
                Instantiate(backgrounds[i], pos, Quaternion.identity);
                // backgrounds[i].
            }

        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
