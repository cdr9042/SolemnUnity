// This code is part of the SS-Scene library, released by Anh Pham (anhpt.csit@gmail.com).

using UnityEngine;
using System.Collections.Generic;

namespace SS.Scene
{
    public enum SceneType
    {
        DEFAULT,
        SCENE,
        SCENE_CLEAR_ALL,
        POPUP,
        LOADING,
        TAB
    }

    public struct SceneData
    {
        public Vector3 Position;
        public Data Data;
        public bool HasAnimation;
        public SceneType SceneType;
        public int MinDepth;
        public bool InStack;

        public SceneData(SceneType sceneType, Vector3 position, Data data, bool hasAnimation, int minDepth, bool inStack)
        {
            SceneType = sceneType;
            Position = position;
            Data = data;
            HasAnimation = hasAnimation;
            MinDepth = minDepth;
            InStack = inStack;
        }
    }

    public class SceneManager
    {
        static Dictionary<string, Controller> m_Scenes = new Dictionary<string, Controller>();
        static Dictionary<string, SceneData> m_Command = new Dictionary<string, SceneData>();
        static Stack<Controller> m_Stack = new Stack<Controller>();
        static List<string> m_IgnorePrefixList = new List<string>();

        static SceneTransition m_SceneTransition;
        static SceneManagerSupporter m_SceneSupporter;
        static CameraManager m_CameraManager;

        static Controller m_CurrentSceneController;

        static Controller m_LoadingController;
        static bool m_LoadingActive;

        static Controller m_TabController;
        static bool m_TabActive;

        static string m_CurrentSceneName;
        static Camera m_CameraUI;

        static SceneManager()
        {
            Application.targetFrameRate = 60;

            Object cameraManager = Resources.Load("CameraManager");
            m_CameraManager = ((GameObject)GameObject.Instantiate(cameraManager)).GetComponent<CameraManager>();
            m_CameraManager.gameObject.name = "CameraManager";

            Object sceneTransition = Resources.Load("SceneTransition");
            m_SceneTransition = ((GameObject)GameObject.Instantiate(sceneTransition)).GetComponent<SceneTransition>();
            m_SceneTransition.gameObject.name = "SceneTransition";
            m_SceneTransition.GetComponentInChildren<Canvas>().worldCamera = m_CameraManager.uiCamera;

            GameObject sceneSupporter = new GameObject("SceneManagerSupporter");
            m_SceneSupporter = sceneSupporter.AddComponent<SceneManagerSupporter>();

            ShieldColor = new Color(0.235f, 0.235f, 0.235f, 0.5f);

            SceneFadeDuration = 0.5f;
            SceneAnimationDuration = 0.283f;
            CameraUI = m_CameraManager.uiCamera;

            RemoveAudioListener();
        }

        /// <summary>
        /// Gets or sets color of shield.
        /// When you popup a non-fullscreen view-B from view-A, a shield is activated and put between view-A and view-B.
        /// </summary>
        /// <value>Color of shield.</value>
        public static Color ShieldColor
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the duration of all show / hide scene animations.
        /// </summary>
        /// <value>The duration of all show / hide scene animations.</value>
        public static float SceneAnimationDuration
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the fade duration of scene transition.
        /// When you call LoadScene() or ReloadScene() of SceneManager, screen will be faded out, then the scene will be loaded, then faded in.
        /// </summary>
        /// <value>The fade duration of scene transition.</value>
        public static float SceneFadeDuration
        {
            get;
            set;
        }

        /// <summary>
        /// 'Loading Scene' is a scene which contains a loading animation. Make your own 'Loading Scene' then use this method to assign it.
        /// Then use SceneManager.LoadingAnimation(bool) to show or hide your loading animation on top.
        /// </summary>
        /// <value>Set the name of the loading scene.</value>
        public static string LoadingSceneName
        {
            set
            {
                SetFixController(m_LoadingController, value, SceneType.LOADING, LoadingPosition, 99);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="SS.Scene.SceneManager"/> prevent loading animation.
        /// </summary>
        /// <value><c>true</c> if prevent loading animation; otherwise, <c>false</c>.</value>
        public static bool PreventLoadingAnimation
        {
            get;
            set;
        }

        public static string ScenePrefix
        {
            get;
            set;
        }

        /// <summary>
        /// Clear all game objects then load the scene (useful for loading a game scene).
        /// </summary>
        /// <param name="sceneName">Scene name.</param>
        /// <param name="data">Data.</param>
        public static void LoadScene(string sceneName, Data data = null)
        {
            sceneName = AddPrefixToSceneName(sceneName);

            LoadScene(sceneName, true, data);
        }

        /// <summary>
        /// Load or Activate a canvas (in a scene) then put it on top of all the others.
        /// </summary>
        /// <param name="sceneName">Scene name.</param>
        /// <param name="data">Data.</param>
        public static void Popup(string sceneName, Data data = null)
        {
            AddScene(sceneName, SceneType.POPUP, ViewPosition, m_Stack.Count * 10, true, data);
        }

        /// <summary>
        /// Reloads the current scene.
        /// </summary>
        /// <param name="data">Data.</param>
        public static void ReloadScene(Data data = null)
        {
            if (!string.IsNullOrEmpty(m_CurrentSceneName))
            {
                LoadScene(m_CurrentSceneName, true, data);
            }
        }

        /// <summary>
        /// Close the canvas which is at top.
        /// </summary>
        public static void Close()
        {
            if (m_Stack.Count > 0)
            {
                m_SceneTransition.ShieldOn();

                Controller controller = m_Stack.Pop();
                controller.Supporter.Hide();

                if (controller.FullScreen && m_Stack.Count > 0)
                {
                    Controller topController = m_Stack.Peek();
                    topController.gameObject.SetActive(true);
                }
            }
        }

        /// <summary>
        /// Show or Hide the loading animation on top.
        /// </summary>
        /// <param name="active">If set to <c>true</c>, show the loading animation on top, and you can not click or tap anything until you hide it.</param>
        public static void LoadingAnimation(bool active)
        {
            if (!PreventLoadingAnimation)
            {
                ActiveFixController(m_LoadingController, ref m_LoadingActive, active);
            }
        }

        public static void SetIgnorePrefixScenes(params string[] sceneNames)
        {
            m_IgnorePrefixList = new List<string>(sceneNames);
        }

        public static void OnFadedIn(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                sceneName = m_CurrentSceneName;
            }
            Controller controller = m_Scenes[sceneName];
            controller.Supporter.OnShown();
        }

        public static void OnFadedOut(string sceneName, bool clearAll)
        {
            if (clearAll)
            {
                HideAll();
                ClearScenes();
                #if UNITY_5_2 || UNITY_5_1 || UNITY_5_0 || UNITY_4_6
                Application.LoadLevel(sceneName);
                #else
                UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
                #endif
            }
            else
            {
                if (m_Scenes.ContainsKey(sceneName))
                {
                    ExcuteCommand(sceneName);
                }
                else
                {
                    #if UNITY_5_2 || UNITY_5_1 || UNITY_5_0 || UNITY_4_6
                    Application.LoadLevelAdditive(sceneName);
                    #else
                    UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
                    #endif
                }
            }
        }

        public static string CurrentSceneName
        {
            get
            {
                return m_CurrentSceneName;
            }
        }

        public static string TabSceneName
        {
            set
            {
                SetFixController(m_TabController, value, SceneType.TAB, TabPosition, 50);
            }
        }

        public static void Tab(bool active)
        {
            ActiveFixController(m_TabController, ref m_TabActive, active);
        }

        public static Camera CameraUI
        {
            get
            {
                return m_CameraUI;
            }

            set
            {
                m_CameraUI = value;
            }
        }

        public static void BackToScene()
        {
            if (m_Stack.Count > 1)
            {
                Controller topController = m_Stack.Pop();

                while (m_Stack.Count > 1)
                {
                    Controller controller = m_Stack.Pop();
                    LostFocusAndRaiseHidden(controller);
                }

                m_Stack.Push(topController);

                Close();
            }
        }

        public static void HideAll()
        {
            while (m_Stack.Count > 0)
            {
                Controller controller = m_Stack.Pop();
                LostFocusAndRaiseHidden(controller);
            }
        }

        public static Controller Top()
        {
            if (m_Stack.Count > 0)
            {
                return m_Stack.Peek();
            }
            return null;
        }

        public static void OnLoaded(Controller controller)
        {
            m_Scenes.Add(controller.SceneName(), controller);
            foreach (var scene in m_Scenes)
            {
                scene.Value.OnAnySceneLoaded(controller);
            }

            Resources.UnloadUnusedAssets();
            System.GC.Collect();

            ExcuteCommand(controller.SceneName());
        }

        public static void OnShown(Controller controller)
        {
            switch (controller.SceneData.SceneType)
            {
                case SceneType.POPUP:
                    m_SceneTransition.ShieldOff();
                    break;
            }

            switch (controller.SceneData.SceneType)
            {
                case SceneType.DEFAULT:
                case SceneType.SCENE:
                case SceneType.SCENE_CLEAR_ALL:
                    break;
                default:
                    RaiseShownAndDeactivePrevScenes(controller);
                    break;
            }
        }

        public static void OnHidden(Controller controller)
        {
            m_SceneTransition.ShieldOff();

            LostFocusAndRaiseHidden(controller);
            ActiveTopSceneOnHidden(controller);
        }

        public static void RemoveAudioListener()
        {
            if (m_SceneSupporter == null)
                return;
            
            AudioListener al = m_SceneSupporter.GetComponent<AudioListener>();
            if (al != null)
            {
                Component.Destroy(al);
            }
        }

        public static void AddAudioListener()
        {
            if (m_SceneSupporter == null)
                return;
            
            AudioListener al = m_SceneSupporter.GetComponent<AudioListener>();
            if (al == null)
            {
                m_SceneSupporter.gameObject.AddComponent<AudioListener>();
            }
        }

        public static bool ActiveShield()
        {
            return m_SceneTransition.Active || m_LoadingActive;
        }

        public static void ActivateBackgroundCamera(bool active)
        {
            if (m_CameraManager != null)
            {
                m_CameraManager.ActivateBackgroundCamera(active);
            }
        }

        public static bool ContainScene(string sceneName)
        {
            foreach (var controller in m_Stack)
            {
                if (string.Compare(controller.SceneName(), sceneName) == 0)
                {
                    return true;
                }
            }

            return false;
        }

        static void LoadScene(string sceneName, bool clearAll, Data data = null)
        {
            if (clearAll)
            {
                ClearCommands();
                AddCommand(sceneName, new SceneData(SceneType.SCENE_CLEAR_ALL, ScenePosition, data, false, 0, true));
                m_SceneTransition.LoadScene(sceneName, clearAll);
            }
            else
            {
                if (m_CurrentSceneController == null || sceneName.CompareTo(m_CurrentSceneController.SceneName()) != 0)
                {
                    AddCommand(sceneName, new SceneData(SceneType.SCENE, ScenePosition, data, false, 0, true));
                    m_SceneTransition.LoadScene(sceneName, clearAll);
                }
                else
                {
                    BackToScene();
                }
            }
        }

        static void RaiseShownAndDeactivePrevScenes(Controller controller)
        {
            DeactivePrevSceneOnShown(controller);
            controller.Supporter.OnShown();
        }

        static void LostFocusAndRaiseHidden(Controller controller)
        {
            controller.OnFocus(false);
            controller.gameObject.SetActive(false);
            controller.Supporter.OnHidden();
        }

        static void SetFixController(Controller controller, string sceneName, SceneType sceneType, Vector3 position, int minDepth)
        {
            if (controller == null)
            {
                AddCommand(sceneName, new SceneData(sceneType, position, null, false, minDepth, false));
                #if UNITY_5_2 || UNITY_5_1 || UNITY_5_0 || UNITY_4_6
                Application.LoadLevelAdditive(sceneName);
                #else
                UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
                #endif
            }
        }

        static void ActiveFixController(Controller controller, ref bool controllerActive, bool active)
        {
            controllerActive = active;

            if (controller != null)
            {
                controller.gameObject.SetActive(active);
            }
        }

        static void ClearCommands()
        {
            m_Command.Clear();
        }

        static void ClearScenes()
        {
            m_Scenes.Clear();
            m_Stack.Clear();
        }

        static void AddScene(string sceneName, SceneType sceneType, Vector3 position, int minDepth, bool hasAnimation = false, Data data = null)
        {
            sceneName = AddPrefixToSceneName(sceneName);

            m_SceneTransition.ShieldOn();

            AddCommand(sceneName, new SceneData(sceneType, position, data, hasAnimation, minDepth, true));

            if (m_Scenes.ContainsKey(sceneName))
            {
                ExcuteCommand(sceneName);
            }
            else
            {
                #if UNITY_5_2 || UNITY_5_1 || UNITY_5_0 || UNITY_4_6
                Application.LoadLevelAdditive(sceneName);
                #else
                UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
                #endif
            }
        }

        static string AddPrefixToSceneName(string sceneName)
        {
            return m_IgnorePrefixList.Contains(sceneName) ? sceneName : ScenePrefix + sceneName;
        }

        static Vector3 ScenePosition
        {
            get { return Vector3.zero; }
        }

        static Vector3 PopupPosition
        {
            get { return Vector3.zero; }
        }

        static Vector3 ViewPosition
        {
            get { return Vector3.zero; }
        }

        static Vector3 LoadingPosition
        {
            get { return Vector3.zero; }
        }

        static Vector3 TabPosition
        {
            get { return Vector3.zero; }
        }

        static void AddCommand(string sceneName, SceneData sceneData)
        {
            m_Command.Add(sceneName, sceneData);
        }

        static void ExcuteCommand(string sceneName)
        {
            Controller controller = m_Scenes[sceneName];
            controller.gameObject.SetActive(true);

            if (m_Command.ContainsKey(sceneName))
            {
                SceneData sceneData = m_Command[sceneName];

                ExcutePushStack(controller, sceneData.InStack);
                ExcuteController(controller, sceneData);
                ExcuteSceneType(controller, sceneData.SceneType);

                m_Command.Remove(sceneName);
            }
            else
            {
                ExcutePushStack(controller, true);
                ExcuteController(controller, new SceneData());
                ExcuteSceneType(controller, SceneType.DEFAULT);
            }

            foreach (var scene in m_Scenes)
            {
                scene.Value.OnAnySceneActivated(controller);
            }
        }

        static void ExcutePushStack(Controller controller, bool inStack)
        {
            if (inStack)
            {
                if (m_Stack.Count > 0)
                {
                    Controller prevController = m_Stack.Peek();
                    prevController.OnFocus(false);
                }
                m_Stack.Push(controller);
            }
        }

        static void ExcuteController(Controller controller, SceneData sceneData)
        {
            controller.SceneData = sceneData;
            controller.transform.position = sceneData.Position;
            controller.Supporter.OnActive(sceneData.Data);
            controller.OnFocus(true);
            controller.Supporter.ResortDepth(sceneData.MinDepth);
            controller.Supporter.AssignCameraUI(m_CameraUI);
            controller.Supporter.Show();
        }

        static void ExcuteSceneType(Controller controller, SceneType sceneType)
        {
            switch (sceneType)
            {
                case SceneType.DEFAULT:
                    m_SceneTransition.FadeInScene();
                    controller.Supporter.ActiveEventSystem(true);
                    m_CurrentSceneController = controller;
                    m_CurrentSceneName = controller.SceneName();
                    break;
                case SceneType.SCENE:
                    m_SceneTransition.FadeInScene();
                    controller.Supporter.DestroyEventSystem();
                    controller.Supporter.ReplaceEventSystem(m_CurrentSceneController);
                    m_CurrentSceneController = controller;
                    m_CurrentSceneName = controller.SceneName();
                    DeactivePrevSceneOnShown(controller);
                    break;
                case SceneType.SCENE_CLEAR_ALL:
                    m_SceneTransition.FadeInScene();
                    m_CurrentSceneController = controller;
                    m_CurrentSceneName = controller.SceneName();
                    controller.Supporter.ActiveEventSystem(true);
                    break;
                case SceneType.POPUP:
                    controller.Supporter.CreateShields();
                    controller.Supporter.DestroyEventSystem();
                    break;
                case SceneType.LOADING:
                    ExcuteFixController(ref m_LoadingController, controller, m_LoadingActive);
                    break;
                case SceneType.TAB:
                    ExcuteFixController(ref m_TabController, controller, m_TabActive);
                    break;
            }
        }

        static void ExcuteFixController(ref Controller fixController, Controller controller, bool active)
        {
            fixController = controller;
            fixController.gameObject.SetActive(active);
            fixController.Supporter.DestroyEventSystem();
            GameObject.DontDestroyOnLoad(fixController.gameObject);
        }

        static void DeactivePrevSceneOnShown(Controller controller)
        {
            if (controller.FullScreen && m_Stack.Count > 1)
            {
                m_Stack.Pop();

                Controller prevController = m_Stack.Peek();
                DeactiveRaiseHidden(prevController);

                m_Stack.Push(controller);
            }
        }

        static void DeactiveRaiseHidden(Controller controller)
        {
            controller.gameObject.SetActive(false);
            controller.Supporter.OnHidden();
        }

        static void ActiveTopSceneOnHidden(Controller controller)
        {
            if (m_Stack.Count > 0)
            {
                Controller topController = m_Stack.Peek();
                topController.OnFocus(true);
                topController.OnReFocus();
            }
        }
    }
}