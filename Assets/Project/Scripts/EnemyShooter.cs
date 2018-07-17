using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Solemn.Enemy
{
    [RequireComponent(typeof(EnemyScript))]
    public class EnemyShooter : MonoBehaviour
    {
        private enum TypeList { Fixed, Follow, SlowFollow }
        [SerializeField] private TypeList shootType = TypeList.Fixed;
        private GunScript[] weapons;
        private Transform target;
        private EnemyScript m_eScript;
        void Awake()
        {
            // Retrieve the weapon only once
            weapons = GetComponentsInChildren<GunScript>();
            if(GameData.current != null)
                target = StageData.current.players[0];
            m_eScript = GetComponent<EnemyScript>();
        }

        void Update()
        {
            // Auto-fire
            foreach (GunScript weapon in weapons)
            {
                // Auto-fire
                if (weapon != null)
                {
                    if (weapon.CurrentState == GunScript.gunState.active && m_eScript.currentState == EnemyScript.State.idle)
                        weapon.Shoot();
                    switch (shootType)
                    {
                        case TypeList.Follow:
                            //weapon.transform.eulerAngles = new Vector3(0, 0, Vector3.Angle(new Vector3(1, 0), target.position - transform.position));
                            if(target != null)
                            weapon.transform.right = target.position - weapon.transform.position;
                            //// Distance moved = time * speed.
                            //float distCovered = (Time.time - startTime) * speed;

                            //// Fraction of journey completed = current distance divided by total distance.
                            //float fracJourney = distCovered / journeyLength;

                            //// Set our position as a fraction of the distance between the markers.
                            //transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fracJourney);

                            break;
                    }
                }
            }
        }
    }

}
