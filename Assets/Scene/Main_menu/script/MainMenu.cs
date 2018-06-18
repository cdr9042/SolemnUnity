using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
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
                SceneManager.LoadScene(g._Progress.stage);

            }
        }
        else
        {
            GameData.current = new GameData();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void loadOptions()
    {

    }

    public void quitGame()
    {
        Application.Quit();
    }
}
