using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Solemn.Audio;

public class ChangeMusic : MonoBehaviour
{
    public AudioClip music;
    //[SerializeField] BoxCollider2D triggerArea;
    private enum PlayType { playOnStart, trigger }
    [SerializeField] PlayType playType = PlayType.playOnStart;
    // Use this for initialization
    void Start()
    {
        if (playType == PlayType.playOnStart)
            AudioManager.instance.changeMusic(music);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (playType == PlayType.trigger)
            if (collision.tag == "Player")
            {
                AudioManager.instance.changeMusic(music);
            }
    }
}
