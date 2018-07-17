using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SS.Scene;

public class LoadMenuController : Controller
{
    public const string LOADMENU_SCENE_NAME = "LoadMenu";

    public override string SceneName()
    {
        return LOADMENU_SCENE_NAME;
    }
}