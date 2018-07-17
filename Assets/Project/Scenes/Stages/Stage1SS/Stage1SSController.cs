using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SS.Scene;

public class Stage1SSController : Controller
{
    public const string STAGE1SS_SCENE_NAME = "Stage1SS";

    public override string SceneName()
    {
        return STAGE1SS_SCENE_NAME;
    }
    public void OnSaveButton()
    {
        SceneManager.Popup(SaveMenuController.SAVEMENU_SCENE_NAME);
    }
}