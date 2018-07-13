using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Solemn.Enemy
{
    [RequireComponent(typeof(EnemyScript))]
    public class BossScript : MonoBehaviour
    {
        public enum BossState { wait, appear, active }
        public BossState m_State = BossState.wait;
        //private Vector3 startPos;
        private EnemyScript m_eScript;
        [SerializeField] List<MonoBehaviour> componentList;
        void Start()
        {
            //startPos = transform.position;
            m_eScript = GetComponent<EnemyScript>();
        }

        void Update()
        {
            switch (m_State)
            {
                case BossState.active:
                    componentSetActive(true);
                    break;
            }
        }

        public void Respawn()
        {
            m_eScript.Respawn();
            m_State = BossState.wait;
            componentSetActive(false);
        }

        void componentSetActive(bool value)
        {
            foreach (MonoBehaviour comp in componentList)
            {
                comp.enabled = value;
            }
        }
    }
}