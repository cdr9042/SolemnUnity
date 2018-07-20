using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class GeneralHelper
{
    public static Vector2 AsVector2(this Vector3 _v)
    {
        return new Vector2(_v.x, _v.y);
    }

    public static GameObject GetMainCamera()
    {
        return (GameObject.FindGameObjectWithTag("MainCamera"));
    }

    public static string RoundUpDecimalString(float value)
    {
        return (Mathf.Round(value * 100) / 100).ToString();
    }

    //public static bool isInView(Transform transf)
    //{
    //    return !(transf.position.x > CamLeftPosX && transf.position.x < CamRightPosX &&
    //        transf.position.y > CamBotPosY && transf.position.y < CamTopPosY);
    //}

    //public static bool isInSpawnBound(Transform transf)
    //{
    //    return (transf.position.x > CamLeftPosX - activeBoundMargin && transf.position.x < CamRightPosX + activeBoundMargin &&
    //        transf.position.y > CamBotPosY - activeBoundMargin && transf.position.y < CamTopPosY + activeBoundMargin);
    //}
}
