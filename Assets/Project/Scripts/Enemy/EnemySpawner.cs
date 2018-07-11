using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform enemySpawn;
    private List<Transform> enemySpawned = new List<Transform>();
    public int initialPoolCount;
    public float totalEnemiesSpawned;
    public float concurrentEnemySpawned;
    public float spawnArea;
    public GameObject player;
    private Transform cam;
    private float horzExtent, vertExtent, CamLeftPosX, CamRightPosX, CamTopPosY, CamBotPosY;
    private Rect activeBound;
    private float activeBoundMargin = 2f;
    public EnemyMasterScript enemyMaster;
    // Use this for initialization
    void Start()
    {
        cam = Camera.main.transform;

        vertExtent = Camera.main.orthographicSize * 2;
        horzExtent = vertExtent * Screen.width / Screen.height;
    }

    // Update is called once per frame
    void Update()
    {
        if (totalEnemiesSpawned <= initialPoolCount)
        {
            CamLeftPosX = cam.position.x - horzExtent;
            CamRightPosX = cam.position.x + horzExtent;
            CamTopPosY = cam.position.y + vertExtent;
            CamBotPosY = cam.position.y - vertExtent;
            // activeBound = new Rect(CamLeftPosX - activeBoundMargin, CamTopPosY + activeBoundMargin, horzExtent * 2 + activeBoundMargin, vertExtent * 2 + activeBoundMargin);
            if (transform.position.x > CamLeftPosX && transform.position.x < CamRightPosX && transform.position.y > CamBotPosY && transform.position.y < CamTopPosY)
            {
                Debug.Log(name + " is in spawn bound");
                totalEnemiesSpawned++;
                Transform spawned = Instantiate(enemySpawn, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
                //.Log(spawned);
                enemySpawned.Add(spawned);
                //.Log(spawned.GetComponent<EnemyScript>().thePlayer);
            }
        }
        // Debug.DrawLine(new Vector3(transform.position.x, transform.position.y, 0), new Vector3(CamLeftPosX, CamTopPosY, 0), Color.red);
        // Debug.DrawLine(new Vector3(transform.position.x, transform.position.y, 0), new Vector3(CamRightPosX, CamBotPosY, 0), Color.red);
        // Debug.Log(Vector2.Distance(activeBound.center, transform.position));
    }
    public void resetSpawner(){
        foreach(Transform enemy in enemySpawned)
        {
            Object.Destroy(enemy);
        }
        totalEnemiesSpawned = 0;
    }
    void OnGUI()
    {
        // DrawQuad(activeBound, Color.red);
    }
    void DrawQuad(Rect position, Color color)
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, color);
        texture.Apply();
        GUI.skin.box.normal.background = texture;
        GUI.Box(position, GUIContent.none);
    }
}
