using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Solemn.Enemy
{

    public class GreenDragonScript : MonoBehaviour
    {
        private Animator m_Anim;
        private EnemyShooter m_Shooter;
        // Use this for initialization
        void Start()
        {
            m_Anim = GetComponent<Animator>();
            m_Shooter = GetComponent<EnemyShooter>();
            //UpdateAnimClipTimes();
            m_Anim.SetBool("Apear", true);
        }

        //void UpdateAnimClipTimes()
        //{
        //    AnimationClip[] clips = m_Anim.runtimeAnimatorController.animationClips;
        //    foreach (AnimationClip clip in clips)
        //    {
        //        switch (clip.name)
        //        {
        //            case "Apear":
        //                apearLength = clip.length + 0.1f;
        //                // Debug.Log("attack CD " + attackCD);
        //                break;

        //        }
        //    }
        //}
        // Update is called once per frame
        void Update()
        {

        }

        public void ActiveGun()
        {
            m_Shooter.enabled = true;
        }
    }

}
