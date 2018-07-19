using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int phase;
    public string checkPoint = "1-1";
    private Animator m_Anim;
    private float[] clipLength = new float[5];
	public Transform dataMaster;

    public void UpdateAnimClipTimes()
    {
        AnimationClip[] clips = m_Anim.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            switch (clip.name)
            {
                case "shrineBeginActive":
                    clipLength[0] = clip.length + 0.1f;
                    break;
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        phase = 0;
        m_Anim = GetComponent<Animator>();
		UpdateAnimClipTimes();
    }

    // Update is called once per frame
    void Update()
    {
        m_Anim.SetInteger("phase", phase);
    }


    IEnumerator startAnimation()
    {
        phase = 1;
        yield return new WaitForSeconds(clipLength[0]);
		phase = 2;
		GameData.current._Progress.checkPoint = name;
        if (GameData.current.getGameMode() != "test") {
            //Debug.Log(GameData.current._Progress.checkPoint);
            //GameData.current._Progress.getStage();
		    //SaveLoadGame.Save();
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.tag == "Player")
        {
            switch (phase)
            {
                case 0:
                    StartCoroutine(startAnimation());
                    break;
            }
        }
    }
}
