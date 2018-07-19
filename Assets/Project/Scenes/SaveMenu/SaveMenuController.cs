using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SS.Scene;

public class SaveMenuController : Controller
{
    public const string SAVEMENU_SCENE_NAME = "SaveMenu";

    public override string SceneName()
    {
        return SAVEMENU_SCENE_NAME;
    }

    //public static void OnSaveButton(int saveSlot)
    //{

    //}

    public void OnBackButton()
    {
        SceneManager.Close();
    }
}