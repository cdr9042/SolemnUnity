using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SS.Scene;

namespace Solemn.Enemy
{
    [RequireComponent(typeof(EnemyScript))]
    public class BossScript : MonoBehaviour
    {
        public enum BossState { wait, appear, active, dying }
        public BossState m_State = BossState.wait;
        //private Vector3 startPos;
        private EnemyScript m_eScript;
        private SpriteRenderer m_SpriteRenderer;
        private float dieTimeLength = 5f;
        [SerializeField] List<MonoBehaviour> componentList;
        [SerializeField] GameObject dyingEffect;
        [SerializeField] GameObject onDeathEffect;
        void Start()
        {
            //startPos = transform.position;
            m_eScript = GetComponent<EnemyScript>();
            m_SpriteRenderer = GetComponent<SpriteRenderer>();
        }

        void Update()
        {
            switch (m_State)
            {
                case BossState.appear:
                    m_State = BossState.active;
                    componentSetActive(true);

                    
                    break;
                case BossState.active:
                    if (m_eScript.currentState == EnemyScript.State.dying && m_State != BossState.dying)
                    {
                        m_State = BossState.dying;
                        GameObject dE = Instantiate(dyingEffect, transform.position, Quaternion.identity);
                        dE.GetComponent<explosionScript>().StartExplosion(dieTimeLength);
                        dE.transform.parent = transform;
                        Debug.Log(m_State);
                        m_State = BossState.dying;
                        StartCoroutine(DyingFlash());
                        StartCoroutine(Die());
                    }
                    break;
            }
        }

        public void setM_State(BossState value)
        {
            m_State = value;
        }

        IEnumerator Die()
        {
            yield return new WaitForSeconds(dieTimeLength);
            Instantiate(onDeathEffect, transform.position, Quaternion.identity);

            //yield return new WaitForSeconds(2f);
            //SceneManager.Popup(VictoryPopupController.VICTORYPOPUP_SCENE_NAME);
            StageData.current.TriggerEvent(StageData.GameEvent.bigBossDefeat);
            GameObject.Destroy(gameObject);
        }
        IEnumerator DyingFlash()
        {
            m_SpriteRenderer.material.SetFloat("_FlashAmount", 0.8f);
            yield return new WaitForSeconds(0.1f);
            m_SpriteRenderer.material.SetFloat("_FlashAmount", 0f);
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(DyingFlash());
        }

        public void Reset()
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