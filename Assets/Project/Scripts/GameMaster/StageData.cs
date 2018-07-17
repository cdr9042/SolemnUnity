using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SS.Scene;
public class StageData:MonoBehaviour
{
    public static StageData current;
    public Transform[] players;

    public enum GameEvent { playerDie, bigBossDefeat }
    public GameObject playerCamera;

    public GameObject m_EnemyMaster;

    float t_countDown;
    float timer;

    public StageData()
    {
        players = new Transform[4];

    }

    public void TriggerEvent(GameEvent ev)
    {
        switch (ev)
        {
            case GameEvent.playerDie:
                m_EnemyMaster.GetComponent<EnemyMasterScript>().resetSpawner();
                break;
            case GameEvent.bigBossDefeat:
                StartCoroutine(LevelComplete());
                break;
        }

    }

    IEnumerator LevelComplete()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.Popup(VictoryPopupController.VICTORYPOPUP_SCENE_NAME);
    }
}
