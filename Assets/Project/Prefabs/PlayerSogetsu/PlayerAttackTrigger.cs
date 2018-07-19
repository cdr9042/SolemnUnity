using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Solemn.Audio;
using Solemn.Enemy;
public class PlayerAttackTrigger : MonoBehaviour
{
    public float damage = 20f;
    public SfxListScript nakedHitSfx;

    void OnCollisionEnter2D(Collision2D collider)
    {
        Debug.Log(collider);
    }


    void OnTriggerEnter2D(Collider2D collider)
    {
        // .Log("collider" + collider.isTrigger);
        // .Log("tag" + collider.CompareTag("Enemy"));
        if (collider.isTrigger != true && collider.CompareTag("Enemy"))
        {
            // .Log("contact enemy");
            object[] param = new object[4];
            param[0] = damage;
            param[1] = Mathf.Sign(collider.transform.position.x - transform.position.x);
            collider.SendMessageUpwards("Damage", param);
            AudioClip[] hitSfx;
            switch (collider.GetComponent<EnemyScript>().m_ArmorType)
            {
                default: hitSfx = nakedHitSfx.soundList; break;
            }
            AudioManager.instance.RandomizeSfx(hitSfx);
        }
    }
}
