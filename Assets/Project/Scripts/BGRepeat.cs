using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGRepeat : MonoBehaviour
{
    private Transform cam;
    // public Transform[] myClone;
    // Use this for initialization
    public Transform posMarker;
    GameObject go;
    void Start()
    {
        if (GameData.current != null)
            cam = StageData.current.playerCamera.transform;
        if (posMarker != null) go = Instantiate(posMarker).gameObject;
        //transform.position = new Vector3(cam.position.x, cam.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (cam != null)
        {
            float horzExtent = Camera.main.orthographicSize * Screen.width / Screen.height;
            float BGsizeX = transform.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
            float BGRightPosX = transform.position.x + BGsizeX * 0.5f;
            float BGLeftPosX = transform.position.x - BGsizeX * 0.5f;
            float CamLeftPosX = cam.position.x - horzExtent;
            float CamRightPosX = cam.position.x + horzExtent;
            float camToB = cam.position.x - (transform.position.x + BGsizeX);
            if (posMarker != null)
            {
                go.transform.position = new Vector2(BGRightPosX, go.transform.position.y); //đánh dấu ô vuông đỏ ở rìa phải BG
                //Debug.DrawLine( //vẽ nét từ trung tâm đến rìa phải BG
                //    new Vector3(transform.position.x, go.transform.position.y, go.transform.position.z - 0.5f),
                //    new Vector3(BGRightPosX, go.transform.position.y, go.transform.position.z - 0.5f),
                //    Color.red);
            }

            //.DrawLine( //vẽ nét từ trung tâm đến rìa phải BG
            //new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.5f),
            //new Vector3(CamRightPosX, cam.position.y, cam.position.z - 0.5f),
            //Color.red);
            //.DrawLine( //vẽ nét từ trung tâm đến rìa phải BG
            //new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.5f),
            //new Vector3(CamLeftPosX, cam.position.y, cam.position.z - 0.5f),
            //Color.red);

            if (CamLeftPosX > BGRightPosX)
            {
                //Debug.Log(transform);
                //Debug.Log("MOVE TO FRONT");
                transform.position = new Vector2(transform.position.x + BGsizeX * 2f, transform.position.y);
            }

            if (CamLeftPosX < BGLeftPosX - BGsizeX)
            {
                //Debug.Log(transform);
                //Debug.Log("MOVE TO LEFT");
                transform.position = new Vector2(transform.position.x - BGsizeX * 2f, transform.position.y);
            }
        }
        else
        {
            if (GameData.current != null)
                cam = StageData.current.playerCamera.transform;
        }

        // .Log(go);
        // go.transform.position = new Vector2(transform.position.x + transform.GetComponent<SpriteRenderer>().size.x/2, go.transform.position.y);
        // .Log(transform.position);
        // .Log("bTP:");
        // .Log(transform.GetComponent<SpriteRenderer>().size.x);
        // .Log(camToB);
        // .Log(horzExtent);



    }
}
