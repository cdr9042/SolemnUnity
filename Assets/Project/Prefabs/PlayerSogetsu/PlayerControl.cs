using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;
namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof (PlayerScript))]
    public class PlayerControl : MonoBehaviour
    {
        private PlayerScript m_Character;
        private bool m_Jump, m_Attack;


        private void Awake()
        {
            m_Character = GetComponent<PlayerScript>();
        }


        private void Update()
        {
            if (!m_Jump)
            {
                // Read the jump input in Update so button presses aren't missed.
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }

            if (!m_Attack) {
                m_Attack = Input.GetKeyDown("c");
            }
        }


        private void FixedUpdate()
        {
            // Read the inputs.
            bool crouch = Input.GetKey(KeyCode.LeftControl);
            // bool attack = Input.GetKey("c");
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            // Debug.Log(h);
            // Pass all parameters to the character control script.
            m_Character.setMove(h);
            m_Character.Move(h, crouch, m_Jump);
            if(m_Character.state == 4) {
                if (m_Jump) {
                    SceneManager.LoadScene("Main_menu");
                }
            }
            m_Jump = false;

            m_Character.Attack(m_Attack);
            m_Attack = false;
        }
    }
}
