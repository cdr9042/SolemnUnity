using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.Pooling;

public class DespawnOutOfView : MonoBehaviour
{
    //public GameObject player;
    private Transform cam;
    private float horzExtent, vertExtent, CamLeftPosX, CamRightPosX, CamTopPosY, CamBotPosY;
    private Rect activeBound;
    private float activeBoundMargin = 2f;
    //public EnemyMasterScript enemyMaster;
    //[SerializeField] private Transform spawnZone;

    private bool enteredView = false;
    void Start()
    {
        cam = Camera.main.transform;

        vertExtent = Camera.main.orthographicSize * 2;
        horzExtent = vertExtent * Screen.width / Screen.height; ;
    }

    // Update is called once per frame
    void Update()
    {

        CamLeftPosX = cam.position.x - horzExtent;
        CamRightPosX = cam.position.x + horzExtent;
        CamTopPosY = cam.position.y + vertExtent;
        CamBotPosY = cam.position.y - vertExtent;
        // activeBound = new Rect(CamLeftPosX - activeBoundMargin, CamTopPosY + activeBoundMargin, horzExtent * 2 + activeBoundMargin, vertExtent * 2 + activeBoundMargin);
        if ((transform.position.x < CamLeftPosX || transform.position.x > CamRightPosX)
            || (transform.position.y < CamBotPosY || transform.position.y > CamTopPosY))
        {
            if (enteredView)
            {
                SpawningPool.ReturnToCache(gameObject);
                //Object.Destroy(gameObject);
            }
        }
        else
        {
            enteredView = true;
        }
        //.DrawLine(new Vector3(transform.position.x, transform.position.y, 0), new Vector3(CamLeftPosX, CamTopPosY, 0), Color.red);
        //.DrawLine(new Vector3(transform.position.x, transform.position.y, 0), new Vector3(CamRightPosX, CamBotPosY, 0), Color.red);
        //.Log(Vector2.Distance(activeBound.center, transform.position));
    }

    //private void OnTriggerExit2D(Collider2D collision)
    //{

    //}
}
