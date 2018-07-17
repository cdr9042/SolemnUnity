using System;
using UnityEngine;
using UnityEngine.UI;
using SS.Scene;
//using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPause = false;

    public GameObject pauseMenuUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPause)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }    
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f ;
        gameIsPause = true;
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.CurrentSceneName);
    } 
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPause = false;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(MainMenu2Controller.MAINMENU2_SCENE_NAME);
    }
    public void LoadOption()
    {

    }



}
