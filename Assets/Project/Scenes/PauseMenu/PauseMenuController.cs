using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SS.Scene;

public class PauseMenuController : Controller
{
    public const string PAUSEMENU_SCENE_NAME = "PauseMenu";

    public override string SceneName()
    {
        return PAUSEMENU_SCENE_NAME;
    }

    public override void OnKeyBack()
    {
        Time.timeScale = 1f;
        StageData.gameIsPause = false;
        SceneManager.Close();
    }

    public void OnSaveButton()
    {
        SceneManager.Popup(SaveMenuController.SAVEMENU_SCENE_NAME);
    }

    public void OnOptionButton()
    {

    }

    public void OnMainMenuButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(MainMenu2Controller.MAINMENU2_SCENE_NAME);
    }
}