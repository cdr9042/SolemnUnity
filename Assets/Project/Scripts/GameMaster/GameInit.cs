using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;
using UnityStandardAssets._2D;

public class GameInit : MonoBehaviour
{
    // public GameData _GameData;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private GameObject m_enemyManager;
    [SerializeField] private GameObject m_PlayerPrefab;
    public Transform m_Player;
    [SerializeField] private GameObject cameraPrefab;
    [SerializeField] private PolygonCollider2D cameraConfiner;
    [SerializeField] private GameObject postInit;
    [SerializeField] private GameObject playerSpawnPoint;
    // Use this for initialization
    void Start()
    {
        //DontDestroyOnLoad(this);
        string lastCheckpointName = null;
        if (GameData.current != null)
        {
            if (GameData.current._Progress.getStage() != "")
            {
                lastCheckpointName = GameData.current._Progress.checkPoint;
                Debug.Log("current stage in data: " + GameData.current._Progress.stage);
            } else {
				GameData.current._Progress.setStage(SceneManager.GetActiveScene().name);
			}
        }
        else
        {
            GameData.current = new GameData();
            GameData.current.setGameMode("test");
            Debug.Log("GameData.current not set, test mode initiated");
        }
        if (lastCheckpointName != null && lastCheckpointName != "" && GameObject.Find(lastCheckpointName)!=null)
        {
            Debug.Log(lastCheckpointName);
            playerSpawnPoint = GameObject.Find(lastCheckpointName);
			playerSpawnPoint.GetComponent<Checkpoint>().phase = 1;
        }
        else
        {
            //lastCheckpoint = GameObject.Find("SpawnPoint");
        }

        // GameObject _cam = Instantiate(cameraFab,new Vector3(0,0,0),Quaternion.identity);
        CreatePlayer();
        CreateCamera();
        CreateUI();
        SetupEnemyManager();
        //enemyMaster.SetActive(true);
        // Debug.Break();
        // .setPlayer(player);
        // HBar.GetComponent<HealBarScript>().init();
        // vcam_confiner.m_BoundingShape2D = GameObject.Find("CameraConfiner").GetComponent<PolygonCollider2D>();
        // Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer ("PlayerLayer"), LayerMask.NameToLayer ("EnemyLayer"));

        // DebugInit();
        postInit.SetActive(true);
    }

    void CreateUI()
    {
        //healthBar = Instantiate(healthBar);
        healthBar.gameObject.GetComponent<HealBarScript>().setPlayer(m_Player);
        healthBar.gameObject.GetComponent<HealBarScript>().init();
        healthBar.transform.parent.parent.gameObject.SetActive(true);
    }

    void CreateCamera()
    {
        GameObject _cam = (GameObject)Instantiate(cameraPrefab);
        _cam.name = "Camera";
        GameData.current.playerCamera = _cam;
        CinemachineVirtualCamera vcam = _cam.GetComponentInChildren<CinemachineVirtualCamera>();
        CinemachineConfiner vcam_confiner = _cam.GetComponentInChildren<CinemachineConfiner>();
        vcam_confiner.m_BoundingShape2D = cameraConfiner;
        vcam.Follow = m_Player.transform;
    }

    void SetupEnemyManager()
    {
        GameData.current.m_EnemyMaster = m_enemyManager;
    }

    void CreatePlayer()
    {
        GameObject playerParent = Instantiate(m_PlayerPrefab, Vector3.zero, Quaternion.identity);
        m_Player = playerParent.transform.Find("PlayerBody");
        GameData.current.players[0] = m_Player;
        m_Player.name = "Player";
        m_Player.transform.localScale = Vector3.one;
        m_Player.transform.position = playerSpawnPoint.transform.position;
    }

    void DebugInit(){
        // GraphicDebug graphicDebug = new GraphicDebug();
    }
    // Update is called once per frame
    //void Update()
    //{

    //}
}
