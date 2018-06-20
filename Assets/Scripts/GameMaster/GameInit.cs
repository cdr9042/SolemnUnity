using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class GameInit : MonoBehaviour
{
    public GameData _GameData;
    // Use this for initialization
    void Start()
    {
        _GameData = new GameData();
        GameObject playerFab = (GameObject)(Resources.Load("Prefab/PlayerSogetsu"));
        // GameObject cameraFab = (GameObject)Instantiate(Resources.Load("Prefab/Camera"));
        Debug.Log(GameData.current);
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
        GameObject lastCheckpoint;
        if (lastCheckpointName != null && lastCheckpointName != "")
        {
            Debug.Log(lastCheckpointName);
            lastCheckpoint = GameObject.Find(lastCheckpointName);
			lastCheckpoint.GetComponent<Checkpoint>().phase = 1;
        }
        else
        {
            lastCheckpoint = GameObject.Find("SpawnPoint");
        }
        Debug.Log(lastCheckpoint);
        GameObject player = Instantiate(playerFab, new Vector3(0, 0, 0), Quaternion.identity);
        player.transform.position = lastCheckpoint.transform.position;
        // GameObject _cam = Instantiate(cameraFab,new Vector3(0,0,0),Quaternion.identity);
        GameObject _cam = GameObject.Find("CM vcam1");
        CinemachineVirtualCamera vcam = _cam.GetComponent<CinemachineVirtualCamera>();
        CinemachineConfiner vcam_confiner = _cam.GetComponent<CinemachineConfiner>();
        vcam.Follow = player.transform;
        // vcam_confiner.m_BoundingShape2D = GameObject.Find("CameraConfiner").GetComponent<PolygonCollider2D>();
        // Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer ("PlayerLayer"), LayerMask.NameToLayer ("EnemyLayer"));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
