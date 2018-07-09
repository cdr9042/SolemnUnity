// This code is part of the SS-Scene library, released by Anh Pham (anhpt.csit@gmail.com).

using UnityEngine;
using System.Collections;

namespace SS.Scene
{
    public class SceneManagerSupporter : MonoBehaviour
    {
        void Awake()
        {
            gameObject.AddComponent<AudioListener>();
            DontDestroyOnLoad(gameObject);
        }

        void Update()
        {
            #if UNITY_EDITOR || UNITY_ANDROID || UNITY_STANDALONE
            UpdateInput();
            #endif
        }

        void UpdateInput()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!SceneManager.ActiveShield())
                {
                    Controller controller = SceneManager.Top();
                    if (controller != null)
                    {
                        controller.OnKeyBack();
                    }
                }
            }
        }
    }
}
