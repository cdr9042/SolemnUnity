// This code is part of the SS-Scene library, released by Anh Pham (anhpt.csit@gmail.com).

using UnityEngine;
using System.Collections.Generic;

namespace SS.Scene
{
    public class UnmanagedCanvas : MonoBehaviour
    {
        void Awake()
        {
            DontDestroyOnLoad(gameObject);

            GetComponent<Canvas>().worldCamera = SceneManager.CameraUI;
        }
    }
}
