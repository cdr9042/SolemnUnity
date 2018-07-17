using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SS.Scene;

public class FinishGameController : Controller
{
    public const string FINISHGAME_SCENE_NAME = "FinishGame";

    public override string SceneName()
    {
        return FINISHGAME_SCENE_NAME;
    }

    public void OnMainmenuButton()
    {
        SceneManager.LoadScene(MainMenu2Controller.MAINMENU2_SCENE_NAME);
    }
}