using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealBarScriptAni : MonoBehaviour {

    Image healthBarAni;


    float maxHealth = 100f;
    public static float healthAni;


    void Start()
    {
        healthBarAni = GetComponent<Image>();
        healthAni = maxHealth;



    }

    // Update is called once per frame
    void Update () {
        healthBarAni.fillAmount = healthAni / maxHealth;
        if (healthAni > HealBarScript.health)
        {
            healthAni -= 0.1f;
        }
        
        
	}
}
