// This code is part of the SS-Scene library, released by Anh Pham (anhpt.csit@gmail.com).

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace SS.Scene
{
    public class SceneTransition : Tweener
    {
        enum State
        {
            SHIELD_OFF,
            SHIELD_ON,
            SHIELD_FADE_IN,
            SHIELD_FADE_OUT,
            SCENE_LOADING
        }

        [SerializeField] Image m_Shield;
        [SerializeField] EaseType m_FadeInEaseType = EaseType.easeInOutExpo;
        [SerializeField] EaseType m_FadeOutEaseType = EaseType.easeInOutExpo;
        [SerializeField] Color m_ShieldColor = Color.black;

        string m_SceneName;
        bool m_ClearAll;
        bool m_Active;

        State m_State;
        EasingFunction m_FadeInEase;
        EasingFunction m_FadeOutEase;
        float m_StartAlpha;
        float m_EndAlpha;

        void Awake()
        {
            DontDestroyOnLoad(gameObject);

            m_FadeInEase = GetEasingFunction(m_FadeInEaseType);
            m_FadeOutEase = GetEasingFunction(m_FadeOutEaseType);
        }

		public void ShieldOff()
		{
            if (m_State == State.SHIELD_ON)
            {
                m_State = State.SHIELD_OFF;
                Active = false;
            }
		}

		public void ShieldOn()
		{
            if (m_State == State.SHIELD_OFF)
            {
                m_State = State.SHIELD_ON;
                Active = true;

                m_Shield.color = Color.clear;
            }
		}

        // Scene gradually appear
        public void FadeInScene()
        {
            if (this != null)
            {
                if (SceneManager.SceneFadeDuration == 0)
                {
                    ShieldOff();
                }
                else
                {
                    Active = true;

                    m_StartAlpha = 1;
                    m_EndAlpha = 0;

                    this.m_AnimationDuration = SceneManager.SceneFadeDuration;
                    this.Play();

                    m_State = State.SHIELD_FADE_IN;
                }
            }
        }

        // Scene gradually disappear
        void FadeOutScene()
        {
            if (this != null)
            {
                if (SceneManager.SceneFadeDuration == 0)
                {
                    OnFadedOut();
                    ShieldOn();
                }
                else
                {
                    Active = true;

                    m_StartAlpha = 0;
                    m_EndAlpha = 1;

                    this.m_AnimationDuration = SceneManager.SceneFadeDuration;
                    this.Play();

                    m_State = State.SHIELD_FADE_OUT;
                }
            }
        }

        public void LoadScene(string sceneName, bool clearAll)
        {
            m_SceneName = sceneName;
            m_ClearAll = clearAll;

            FadeOutScene();
        }

        public void OnFadedIn()
        {
            if (this != null)
            {
                m_State = State.SHIELD_OFF;
                Active = false;
                SceneManager.OnFadedIn(m_SceneName);
            }
        }

        public void OnFadedOut()
        {
            m_State = State.SCENE_LOADING;
            SceneManager.OnFadedOut(m_SceneName, m_ClearAll);
        }

        public bool Active
        {
            get
            {
                return m_Active;
            }
            protected set
            {
                m_Active = value;
                m_Shield.gameObject.SetActive(m_Active);
            }
        }

        protected override void ApplyProgress(float progress)
        {
            EasingFunction ease = (m_State == State.SHIELD_FADE_IN) ? m_FadeInEase : m_FadeOutEase;

            Color color = m_ShieldColor;
            color.a = ease(m_StartAlpha, m_EndAlpha, progress);

            m_Shield.color = color;
        }

        protected override void OnEndAnimation()
        {
            switch (m_State)
            {
                case State.SHIELD_FADE_IN:
                    OnFadedIn();
                    break;
                case State.SHIELD_FADE_OUT:
                    OnFadedOut();
                    break;
            }
        }
    }
}