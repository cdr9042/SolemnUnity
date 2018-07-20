using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.Pooling;
public class EnemySpawner : MonoBehaviour
{
    public Transform enemyPrefab;
    private List<Transform> enemySpawned = new List<Transform>();
    public int m_PoolAmount;
    public float numberEnemySpawned;
    public float concurrentEnemySpawned;
    public float spawnArea;
    private bool isInitialSpawn = true;

    public GameObject player;
    private Transform cam;

    private float horzExtent, vertExtent, CamLeftPosX, CamRightPosX, CamTopPosY, CamBotPosY;
    private Rect activeBound;
    private float activeBoundMargin = 2f;

    public EnemyMasterScript enemyMaster;
    [SerializeField] private bool canDespawnOOV = true;

    private enum SpawnType { minion, boss }
    [SerializeField] private SpawnType m_spawnType = SpawnType.minion;


    //private string prefabKey;
    // Use this for initialization
    void Start()
    {
        cam = GeneralHelper.GetMainCamera().transform;

        vertExtent = Camera.main.orthographicSize * 2;
        horzExtent = vertExtent * Screen.width / Screen.height;
        //prefabKey = enemyPrefab.name;
        //if (!SpawningPool.ContainsPrefab(prefabKey))
        //{
        //    SpawningPool.AddPrefab(prefabKey, enemyPrefab.gameObject);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        CamLeftPosX = cam.position.x - horzExtent;
        CamRightPosX = cam.position.x + horzExtent;
        CamTopPosY = cam.position.y + vertExtent;
        CamBotPosY = cam.position.y - vertExtent;
        if (m_spawnType == SpawnType.minion && numberEnemySpawned <= m_PoolAmount)
        {
            // activeBound = new Rect(CamLeftPosX - activeBoundMargin, CamTopPosY + activeBoundMargin, horzExtent * 2 + activeBoundMargin, vertExtent * 2 + activeBoundMargin);
            if (isInitialSpawn)
            {
                if (isInSpawnBound(transform))
                {
                    Spawn();
                    isInitialSpawn = false;
                }
            }
            else if (!isInView(transform) && isInSpawnBound(transform))
            {
                Spawn();
            }

        }

        if (canDespawnOOV)
        {
            foreach (Transform spawn in enemySpawned.ToArray())
            {
                if (spawn == null)
                {
                    enemySpawned.Remove(spawn); break;
                }
                if (!isInSpawnBound(spawn))
                //if (spawn.position.x < CamLeftPosX - activeBoundMargin || spawn.position.x > CamRightPosX + activeBoundMargin ||
                //    spawn.position.y < CamBotPosY - activeBoundMargin || spawn.position.y > CamTopPosY + activeBoundMargin)
                {
                    //SpawningPool.ReturnToCache(spawn.gameObject);
                    Object.Destroy(spawn.gameObject);
                    numberEnemySpawned--;
                    enemySpawned.Remove(spawn);
                }
            }
        }

        // Debug.DrawLine(new Vector3(transform.position.x, transform.position.y, 0), new Vector3(CamLeftPosX, CamTopPosY, 0), Color.red);
        // Debug.DrawLine(new Vector3(transform.position.x, transform.position.y, 0), new Vector3(CamRightPosX, CamBotPosY, 0), Color.red);
        // Debug.Log(Vector2.Distance(activeBound.center, transform.position));
    }

    public void Spawn()
    {
        numberEnemySpawned++;
        //Transform spawned = SpawningPool.CreateFromCache(prefabKey).transform;
        Transform spawned = Instantiate(enemyPrefab, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
        enemySpawned.Add(spawned);
    }

    public void Reset()
    {
        resetSpawner();
        isInitialSpawn = true;
    }

    public void resetSpawner()
    {
        foreach (Transform enemy in enemySpawned)
        {
            Object.Destroy(enemy.gameObject);
        }
        enemySpawned.Clear();
        numberEnemySpawned = 0;
    }

    bool isInView(Transform transf)
    {
        return (transf.position.x > CamLeftPosX && transf.position.x < CamRightPosX && transf.position.y > CamBotPosY && transf.position.y < CamTopPosY);
    }

    bool isInSpawnBound(Transform transf)
    {
        return (transf.position.x > CamLeftPosX - activeBoundMargin && transf.position.x < CamRightPosX + activeBoundMargin &&
            transf.position.y > CamBotPosY - activeBoundMargin && transf.position.y < CamTopPosY + activeBoundMargin);
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
