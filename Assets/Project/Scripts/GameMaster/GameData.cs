using UnityEngine;
using System.Collections;
//using UnityEngine.SceneManagement;
using SS.Scene;

[System.Serializable]
public class GameData
{ //don't need ": Monobehaviour" because we are not attaching it to a game object

    public static GameData current;
    public Progress _Progress;
    public static string gameMode = "";
    public string modifiedTime = "";
    //public Transform[] players;
    //private int totalEnemyCount;

    public GameData()
    {
        _Progress = new Progress();        
        //players = new Transform[4];
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

    

    public void CreateNewGame()
    {
        SceneManager.LoadScene(Stage1SSController.STAGE1SS_SCENE_NAME);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void Load(int i)
    {
        

    }
}
