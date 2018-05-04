using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace UnityStandardAssets._2D
{
    public class RedHealBarScript : MonoBehaviour
    {
        Image redHealthBar;
        float maxHealth = 100f;
        public static float redHealth;
        GameObject player;
        private PlayerScript playerScript;
        // Use this for initialization
        void Start()
        {
            redHealthBar = GetComponent<Image>();
            GameObject player = GameObject.Find("Player");
            playerScript = player.GetComponent<PlayerScript>();
            maxHealth = playerScript.m_HealthMax;
            redHealth = maxHealth;
        }

        // Update is called once per frame
        void Update()
        {
            //redHealth = playerScript.m_HealthLeft;
            redHealthBar.fillAmount = redHealth / maxHealth;

            if (HealBarScript.health < redHealth)
            {
                redHealth -= 0.1f;
            }
        }
    }
}