using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour {

	// Use this for initialization
  // 1 - Designer variables

  public Transform shotPrefab;

  /// Cooldown in seconds between two shots
  public float shootingRate ;

  // 2 - Cooldown
  

  private float shootCooldown;

  void Start()
  {
    shootCooldown = 0f;
  }

  void Update()
  {
    if (shootCooldown > 0)
    {
      shootCooldown -= Time.deltaTime;
    }
  }

  //--------------------------------
  // 3 - Shooting from another script
  //--------------------------------

  public void Shoot(bool isEnemy)
  {
    if (CanAttack)
    {
      shootCooldown = shootingRate;

      // Create a new shot
      var shotTransform = Instantiate(shotPrefab) as Transform;

      // Assign position
      shotTransform.position = transform.position;

      // The is enemy property
      EnemyBullet shot = shotTransform.gameObject.GetComponent<EnemyBullet>();
      if (shot != null)
      {
        //shot.isEnemyShot = isEnemy;
      }

      // Make the weapon shot always towards it
     	EnemyFlyer fly = shotTransform.gameObject.GetComponent<EnemyFlyer>();
       if (fly != null)
       {
         fly.direction = this.transform.right; // towards in 2D space is the right of the sprite
       }
    }
  }

  
  /// Is the weapon ready to create a new projectile?
  
  public bool CanAttack
  {
    get
    {
      return shootCooldown <= 0f;
    }
  }

}
