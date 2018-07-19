using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SS.Scene;

public class StageController : Controller {

    public override string SceneName()
    {
        return string.Empty;
    }

    public void OpenPauseMenu()
    {
        SceneManager.Popup(PauseMenuController.PAUSEMENU_SCENE_NAME);
    }

    public override void OnKeyBack()
    {
        Time.timeScale = 0f;
        StageData.gameIsPause = true;
        SceneManager.Popup(PauseMenuController.PAUSEMENU_SCENE_NAME);
    }
}
