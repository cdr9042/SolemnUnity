using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace UnityStandardAssets._2D
{
    public class HealBarScript : MonoBehaviour
    {

        Image healthBar;
        public float maxHealth = 100f;
        public float health;
        public GameObject player;
        private PlayerScript playerScript;
        void Start()
        {
            healthBar = GetComponent<Image>();        
            // GameObject player = GameObject.FindGameObjectsWithTag("Player")[0];
            // playerScript = player.GetComponent<PlayerScript>();
            // maxHealth = playerScript.m_HealthMax;
            // health = maxHealth;
        }
        public void init()
        {
            // healthBar = GetComponent<Image>();
            // GameObject player = GameObject.FindGameObjectsWithTag("Player")[0];
            playerScript = player.GetComponent<PlayerScript>();
            maxHealth = playerScript.m_HealthMax;
            health = maxHealth;
        }
        void Update()
        {

            if (playerScript != null) 
                health = playerScript.m_HealthLeft;
            healthBar.fillAmount = health / maxHealth;


        }
        public float getMaxHealth()
        {
            return maxHealth;
        }
        public void setPlayer(GameObject _player) {
            player= _player;
        }
    }
}