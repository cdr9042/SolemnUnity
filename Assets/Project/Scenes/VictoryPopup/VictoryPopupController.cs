using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SS.Scene;

public class VictoryPopupController : Controller
{
    public const string VICTORYPOPUP_SCENE_NAME = "VictoryPopup";

    public override string SceneName()
    {
        return VICTORYPOPUP_SCENE_NAME;
    }

    public void OnContinueButton()
    {
        SceneManager.LoadScene(FinishGameController.FINISHGAME_SCENE_NAME);
    }
}