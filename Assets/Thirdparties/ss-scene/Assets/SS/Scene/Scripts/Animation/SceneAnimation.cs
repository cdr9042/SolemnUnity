// This code is part of the SS-Scene library, released by Anh Pham (anhpt.csit@gmail.com).

using UnityEngine;
using System.Collections;

namespace SS.Scene
{
    [ExecuteInEditMode]
    public class SceneAnimation : Tweener
    {
        [SerializeField] Controller m_Controller;

        /// <summary>
        /// After a scene is loaded, its view will be put at center of screen. So you have to put it somewhere temporary before playing the show-animation.
        /// </summary>
        public virtual void HideBeforeShowing()
        {
        }

        /// <summary>
        /// Play the show-animation. Don't forget to call OnShown right after the animation finishes.
        /// </summary>
        public virtual void Show()
        {
            OnShown();
        }

        /// <summary>
        /// Play the hide-animation. Don't forget to call OnHidden right after the animation finishes.
        /// </summary>
        public virtual void Hide()
        {
            OnHidden();
        }

        public void OnShown()
        {
            SceneManager.OnShown(m_Controller);
        }

        public void OnHidden()
        {
            SceneManager.OnHidden(m_Controller);
        }

        #if UNITY_EDITOR
        protected override void Update()
        {
            base.Update();

            if (!Application.isPlaying)
            {
                AutoFind();
            }
        }

        void AutoFind()
        {
            if (!Application.isPlaying)
            {
                if (m_Controller == null)
                {
                    m_Controller = FindObjectOfType<Controller>();

                    if (m_Controller != null)
                    {
                        m_Controller.Animation = this;
                    }
                }
            }
        }
        #endif
    }
}

