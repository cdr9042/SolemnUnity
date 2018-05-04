using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace UnityStandardAssets._2D
{

    public class GameOverUIScript : MonoBehaviour
    {
        public static bool playerIsDeath = false;
        float healLeft ;
        public GameObject gameOverUI;

        private PlayerScript playerScript;
        GameObject player;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Player")
            {
                gameOverUI.SetActive(true);
            }
        }

       public void active() {
            gameOverUI.SetActive(true);
        }


        // Use this for initialization
        void Start()
        {
            GameObject player = GameObject.Find("Player");
            playerScript = player.GetComponent<PlayerScript>();
            healLeft = playerScript.m_HealthLeft;
        }

        // Update is called once per frame
        void Update()
        {
            if (healLeft == 0)
            {
                //gameOverUI.SetActive(true);
                Time.timeScale = 0f;
            }
        }
    }
}
