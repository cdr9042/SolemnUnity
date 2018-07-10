using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.SceneManagement;
using SS.Scene;

public class MainMenu2Controller : Controller
{
    public const string MAINMENU2_SCENE_NAME = "MainMenu2";

    public override string SceneName()
    {
        return MAINMENU2_SCENE_NAME;
    }

    public void StartGame()
    {
        SaveLoadGame.Load();
        Debug.Log(SaveLoadGame.savedGames);
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
        else
        {
            CreateNewGame();
        }
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void CreateNewGame()
    {
        GameData.current = new GameData();
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Debug.Log("starting new game");
    }
    public void loadOptions()
    {

    }

    public void quitGame()
    {
        Application.Quit();
    }
}