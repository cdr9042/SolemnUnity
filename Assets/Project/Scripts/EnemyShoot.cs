using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour {

	private Shooting[] weapons;

  void Awake()
  {
    // Retrieve the weapon only once
    weapons = GetComponentsInChildren<Shooting>();
  }

  void Update()
  {
    // Auto-fire
    foreach (Shooting weapon in weapons)
    {
      // Auto-fire
      if (weapon != null && weapon.CanAttack)
      {
        weapon.Shoot(true);
      }
    }
  }
}
