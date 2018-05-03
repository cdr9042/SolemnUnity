using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace UnityStandardAssets._2D {
    public class HealBarScript : MonoBehaviour {

        Image healthBar;
        float maxHealth = 100f;
        public static float health;
        GameObject player;
        private PlayerScript playerScript;
        void Start()
        {
            healthBar = GetComponent<Image>();        
            GameObject player = GameObject.Find("Player");
            playerScript = player.GetComponent<PlayerScript>();
            maxHealth = playerScript.m_HealthMax;
            health = maxHealth;
        }

        void Update()
        {
            health = playerScript.m_HealthLeft;
            healthBar.fillAmount = health / maxHealth;
        }
    }
}