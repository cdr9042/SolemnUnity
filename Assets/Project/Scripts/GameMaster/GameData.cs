using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[System.Serializable]
public class GameData
{ //don't need ": Monobehaviour" because we are not attaching it to a game object

    public static GameData current;
    public Progress _Progress;
    public static string gameMode;
    public Transform[] players;
    public GameObject m_EnemyMaster;
    public enum GameEvent { playerDie }
    public GameObject playerCamera;
    //private int totalEnemyCount;

    public GameData()
    {
        _Progress = new Progress();
        gameMode = "";
        players = new Transform[4];
        //totalEnemyCount = 0;
    }
    public void setGameMode(string gM)
    {
        gameMode = gM;
    }
    public string getGameMode()
    {
        return gameMode;
    }

    public void TriggerEvent(GameEvent ev)
    {
        switch (ev)
        {
            case GameEvent.playerDie:
                m_EnemyMaster.GetComponent<EnemyMasterScript>().resetSpawner();
                break;
        }
        
    }

    public void CreateNewGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadGame()
    {
        if (SaveLoadGame.savedGames.Count > 0)
        {
            foreach (GameData g in SaveLoadGame.savedGames)
            {
                GameData.current = g;
                //Move on to game...
                // Application.LoadLevel(1);
                string savedStage = g._Progress.getStage();
                if (Application.CanStreamedLevelBeLoaded(savedStage))
                {
                    SceneManager.LoadScene(savedStage);
                }
                else
                {
                    Debug.Log("invalid save data");
                    CreateNewGame();
                }
                // Do something that can throw an exception

            }
        }
    }
}
