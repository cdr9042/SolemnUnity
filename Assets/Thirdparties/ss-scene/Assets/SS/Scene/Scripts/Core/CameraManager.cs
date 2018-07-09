// This code is part of the SS-Scene library, released by Anh Pham (anhpt.csit@gmail.com).

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] Camera m_BgCamera;
    [SerializeField] Camera m_UiCamera;

    public void ActivateBackgroundCamera(bool active)
    {
        if (m_BgCamera != null && m_BgCamera.gameObject != null)
        {
            m_BgCamera.gameObject.SetActive(active);
        }
    }

    public Camera uiCamera
    {
        get { return m_UiCamera; }
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}