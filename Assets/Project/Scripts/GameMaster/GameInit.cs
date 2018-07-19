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
    [HideInInspector] public Transform m_Player;
    [SerializeField] private GameObject cameraPrefab;
    private PolygonCollider2D cameraConfiner;
    private GameObject postInit;
    private GameObject playerSpawnPoint;
    [SerializeField] private GameObject generalManager;
    [SerializeField] private GameObject stageManager;

    // Use this for initialization
    void Start()
    {
        //DontDestroyOnLoad(this);
        //Init game data (for saving progress)
        string lastCheckpointName = null;
        if (GameData.current != null)
        {
            if (GameData.current._Progress.getStage() != "")
            {
                lastCheckpointName = GameData.current._Progress.checkPoint;
                Debug.Log("current stage in data: " + GameData.current._Progress.stage);
            }
            else
            {
                GameData.current._Progress.setStage(SceneManager.GetActiveScene().name);
            }
        }
        else
        {
            GameData.current = new GameData();
            GameData.current._Progress.setStage(SceneManager.GetActiveScene().name);
            GameData.current.setGameMode("test");
            Debug.Log("GameData.current not set, test mode initiated");
        }

        //Init Stage data (for referencing player and handling events)
        StageData.current = stageManager.GetComponent<StageData>();

        if (lastCheckpointName != null && lastCheckpointName != "" && GameObject.Find(lastCheckpointName) != null)
        {
            Debug.Log(lastCheckpointName);
            playerSpawnPoint = GameObject.Find(lastCheckpointName);
            playerSpawnPoint.GetComponent<Checkpoint>().phase = 1;
        }
        else
        {
            playerSpawnPoint = GameObject.Find("SpawnPoint");
        }

        // GameObject _cam = Instantiate(cameraFab,new Vector3(0,0,0),Quaternion.identity);
        CreatePlayer();
        CreateCamera();
        CreateUI();
        SetupManagers();
        //enemyMaster.SetActive(true);
        // Debug.Break();
        // .setPlayer(player);
        // HBar.GetComponent<HealBarScript>().init();
        // vcam_confiner.m_BoundingShape2D = GameObject.Find("CameraConfiner").GetComponent<PolygonCollider2D>();
        // Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer ("PlayerLayer"), LayerMask.NameToLayer ("EnemyLayer"));

        // DebugInit();
        postInit = GameObject.Find("PostInit");
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
        //GameObject _cam = (GameObject)Instantiate(cameraPrefab);
        //_cam.name = "Camera";
        StageData.current.playerCamera = GameObject.FindGameObjectWithTag("MainCamera");
        CinemachineVirtualCamera vcam = cameraPrefab.GetComponentInChildren<CinemachineVirtualCamera>();
        CinemachineConfiner vcam_confiner = cameraPrefab.GetComponentInChildren<CinemachineConfiner>();
        cameraConfiner = GameObject.Find("CameraConfiner").GetComponent<PolygonCollider2D>();
        vcam_confiner.m_BoundingShape2D = cameraConfiner;
        vcam.Follow = m_Player.transform;
    }

    void SetupManagers()
    {
        if (!GameObject.Find("GeneralManager"))
        {
            Instantiate(generalManager);
        }
        StageData.current.m_EnemyMaster = m_enemyManager;
    }

    void CreatePlayer()
    {
        GameObject playerParent = Instantiate(m_PlayerPrefab, Vector3.zero, Quaternion.identity);
        m_Player = playerParent.transform.Find("PlayerBody");
        StageData.current.players[0] = m_Player;
        m_Player.name = "Player";
        m_Player.transform.localScale = Vector3.one;
        m_Player.transform.position = playerSpawnPoint.transform.position;
    }

    void DebugInit()
    {
        // GraphicDebug graphicDebug = new GraphicDebug();
    }
    // Update is called once per frame
    //void Update()
    //{

    //}
}
