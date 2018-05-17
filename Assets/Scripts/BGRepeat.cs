using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGRepeat : MonoBehaviour
{
    private Transform cam;
    // public Transform[] myClone;
    // Use this for initialization
	public GameObject posMarker;
	GameObject go;
    void Start()
    {
        cam = Camera.main.transform;
		if (posMarker!=null) go = Instantiate(posMarker) as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        float horzExtent = Camera.main.orthographicSize * Screen.width / Screen.height;
		float BGsizeX = transform.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        float BGRightPosX = transform.position.x + BGsizeX*0.5f;
		float BGLeftPosX = transform.position.x - BGsizeX*0.5f;
        float CamLeftPosX = cam.position.x - horzExtent;
		float CamRightPosX = cam.position.x + horzExtent;
        float camToB = cam.position.x - (transform.position.x + BGsizeX);
        if (posMarker!=null)  {go.transform.position = new Vector2(BGRightPosX, go.transform.position.y);
		Debug.DrawLine(
			new Vector3(transform.position.x,go.transform.position.y,go.transform.position.z-0.5f),
			new Vector3(BGRightPosX,go.transform.position.y,go.transform.position.z-0.5f),
			Color.blue);
		}
		// Debug.Log(go);
        // go.transform.position = new Vector2(transform.position.x + transform.GetComponent<SpriteRenderer>().size.x/2, go.transform.position.y);
        // Debug.Log(transform.position);
        // Debug.Log("bTP:");
        // Debug.Log(transform.GetComponent<SpriteRenderer>().size.x);
        // Debug.Log(camToB);
        // Debug.Log(horzExtent);


        if (CamLeftPosX > BGRightPosX)
        {
			Debug.Log(transform);
            Debug.Log("MOVE TO FRONT");
			transform.position = new Vector2(transform.position.x + BGsizeX*2f, transform.position.y);
        }

		if (CamLeftPosX < BGLeftPosX - BGsizeX)
        {
            Debug.Log(transform);
            Debug.Log("MOVE TO LEFT");
			transform.position = new Vector2(transform.position.x - BGsizeX*2f, transform.position.y);
        }
    }
}
