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
        public Transform GreenBar;
        private PlayerScript playerScript;
        public HealBarScript GreenBarScript;
        // Use this for initialization
        void Start()
        {
            redHealthBar = GetComponent<Image>();
            // foreach (GameObject playerobj in GameObject.FindGameObjectsWithTag("Player")) {
            //     Debug.Log(playerobj);
            // }
            // GameObject player = GameObject.FindGameObjectsWithTag("Player")[0];
            // Debug.Break();
            // playerScript = player.GetComponent<PlayerScript>();
            GreenBarScript = GreenBar.GetComponent<HealBarScript>();
            maxHealth = GreenBarScript.maxHealth;
            redHealth = maxHealth;
        }

        // Update is called once per frame
        void Update()
        {
            //redHealth = playerScript.m_HealthLeft;
            redHealthBar.fillAmount = redHealth / maxHealth;

            if (GreenBarScript.health < redHealth)
            {
                redHealth -= 0.1f;
            }
        }
    }
}