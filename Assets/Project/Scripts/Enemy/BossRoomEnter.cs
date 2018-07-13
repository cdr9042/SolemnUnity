using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
public class BossRoomEnter : MonoBehaviour {
    PlayableDirector timeline;
    private bool played = false;
    private void Start()
    {
        timeline = GetComponent<PlayableDirector>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !played)
        {
            timeline.Play();
            played = true;
        }
    }
}
