using System;
using UnityEngine;
using UnityEngine.UI;
using SS.Scene;
//using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPause = false;

    public GameObject pauseMenuUI;
    public Stage1SSController SceneController;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (StageData.gameIsPause)
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
        //pauseMenuUI.SetActive(true);

        Time.timeScale = 0f ;
        StageData.gameIsPause = true;
        SceneController.OpenPauseMenu();
        //SceneManager.Popup(PauseMenuController.PAUSEMENU_SCENE_NAME);
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.CurrentSceneName);
    } 
    public static void Resume()
    {
        //pauseMenuUI.SetActive(false);
        SceneManager.Close();
        Time.timeScale = 1f;
        StageData.gameIsPause = false;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(MainMenu2Controller.MAINMENU2_SCENE_NAME);
        //SceneManager.LoadScene("MainMenu2");
    }
    public void LoadOption()
    {

    }

    public void OnSaveButton()
    {
        SceneManager.Popup(SaveMenuController.SAVEMENU_SCENE_NAME);
    }


}
