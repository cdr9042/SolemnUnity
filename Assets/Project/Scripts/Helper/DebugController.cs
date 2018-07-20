using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DebugController : MonoBehaviour
{
    public static DebugController instance=null; 
    public Transform trackVelocity;
    enum DebugToggle { velocity }
    List<DebugToggle> debugToggles;
    Text m_Text;
    public string DebugText { get;set;
    }
    string GUIstring;
    // Use this for initialization

    private void Awake()
    {
        //Check if there is already an instance of SoundManager
        if (instance == null)
            //if not, set it to this.
            instance = this;
        //If instance already exists:
        else if (instance != this)
            //Destroy this, this enforces our singleton pattern so there can only be one instance of SoundManager.
            Destroy(gameObject);

        //Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        debugToggles = new List<DebugToggle>();
        m_Text = GetComponent<Text>();
    }

    public void TrackVelocity(bool track)
    {
        if (track)
            debugToggles.Add(DebugToggle.velocity);
        else
        {
            debugToggles.Remove(DebugToggle.velocity);
        }
    }
    
    void Update()
    {
        GUIstring = DebugText;
        foreach (DebugToggle dT in debugToggles)
        {
            switch (dT)
            {
                case DebugToggle.velocity: 
                    if (trackVelocity != null)
                    {
                        Vector2 vel = trackVelocity.GetComponent<Rigidbody2D>().velocity;
                        GUIstring += ("\nVelocity: " + GeneralHelper.RoundUpDecimalString(vel.x));
                        GUIstring += ("," + GeneralHelper.RoundUpDecimalString(vel.y));
                    }
                    
                    break;
            }
        }
        m_Text.text = GUIstring;
    }

    public void TrackVelocitySetTarget(string name)
    {
        Debug.Log("param:"+name);
        trackVelocity = GameObject.Find(name).transform;
        Debug.Log("target change to " + trackVelocity.gameObject.name);
    }

    //private void OnGUI()
    //{
    //    GUILayout.Label(GUIstring);
    //}
}
