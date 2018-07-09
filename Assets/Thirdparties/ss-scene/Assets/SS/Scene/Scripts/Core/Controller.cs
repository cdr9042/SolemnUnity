// This code is part of the SS-Scene library, released by Anh Pham (anhpt.csit@gmail.com).

using UnityEngine;
using System.Collections;

namespace SS.Scene
{
    public interface IController
    {
        string SceneName();
    }

    public class Data
    {
        public delegate void CallbackDelegate(Controller controller);

        /// <summary>
        /// This event is raised when the view is shown.
        /// </summary>
        public CallbackDelegate OnShown;

        /// <summary>
        /// This event is raised when the view is hidden.
        /// </summary>
        public CallbackDelegate OnHidden;

        public Data()
        {
        }

        public Data(CallbackDelegate onShown, CallbackDelegate onHidden)
        {
            OnShown = onShown;
            OnHidden = onHidden;
        }
    }

    [RequireComponent(typeof(ControllerSupporter))]
    public abstract class Controller : MonoBehaviour, IController
    {
        /// <summary>
        /// When you popup a fullscreen view using SceneManager.Popup(), the system will automatically deactivate the view under it ( for better performance ).
        /// When you close it using SceneManager.Close(), the view which was under it will be activated.
        /// </summary>
        [SerializeField]
        public bool FullScreen;

        /// <summary>
        /// SS-SceneManager has a default background camera and a default UI camera.
        /// When you load a scene contains its own camera, the default background camera will be deactivated automatically.
        /// </summary>
        [SerializeField]
        public bool OwnCamera;

        [SerializeField]
        public ControllerSupporter Supporter;

        [SerializeField]
        public SceneAnimation Animation;

        /// <summary>
        /// Each scene must has an unique scene name.
        /// </summary>
        /// <returns>The name.</returns>
        public abstract string SceneName();

        /// <summary>
        /// This event is raised right after this view becomes active after a call of LoadScene() or ReloadScene() or Popup() of SceneManager.
        /// Same OnEnable but included the data which is transfered from the previous view.
        /// </summary>
        /// <param name="data">Data.</param>
        public virtual void OnActive(Data data)
        {
        }

        /// <summary>
        /// This event is raised right after this view gets or losts focus.
        /// </summary>
        /// <param name="isFocus">If set to <c>true</c> is focus.</param>
        public virtual void OnFocus(bool isFocus)
        {
        }

        /// <summary>
        /// This event is raised right after the above view is hidden.
        /// </summary>
        public virtual void OnReFocus()
        {
        }

        /// <summary>
        /// This event is raised right after this view appears and finishes its show-animation.
        /// </summary>
        public virtual void OnShown()
        {
        }

        /// <summary>
        /// This event is raised right after this view finishes its hide-animation and disappears.
        /// </summary>
        public virtual void OnHidden()
        {
        }

        /// <summary>
        /// This event is raised right after player pushs the ESC button on keyboard or Back button on android devices.
        /// You should assign this method to OnClick event of your Close Buttons.
        /// </summary>
        public virtual void OnKeyBack()
        {
            SceneManager.Close();
        }

        /// <summary>
        /// This event is raised right after any scene is loaded.
        /// </summary>
        /// <param name="controller">Controller.</param>
        public virtual void OnAnySceneLoaded(Controller controller)
        {
        }

        /// <summary>
        /// This event is raised right after any scene is activated.
        /// </summary>
        /// <param name="controller">Controller.</param>
        public virtual void OnAnySceneActivated(Controller controller)
        {
        }

        public SceneData SceneData
        {
            get;
            set;
        }
    }
}
