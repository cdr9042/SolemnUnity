using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    public Transform bulletPrefab;
    public float bulletNums = 1;
    public float bulletSpeed = 1f;
    public float spreadAngle = 10f;
    //[SerializeField] private float shootAngle = 0f;
    public float reloadTime = 1f;
    [Tooltip("Số lần bắn đạn mỗi giây nếu băng đạn nhiều hơn một viên")]
    public float fireRate = 1f;
    [Tooltip("Magazine Size: số đạn tối đa một băng đạn")]
    public int magSize = 1;
    [SerializeField] private int magLeft = 1;
    public int MagLeft { get { return magLeft; } set { if (value > magSize) { magLeft = magSize; } } }
    [Tooltip("Countdown Reload: đếm ngược thời gian nạp đạn")]
    public float CDReload = 0f;
    public enum gunState { inactive, active, shoot, reloading };
    [SerializeField] private gunState currentGunState = gunState.active;
    public gunState CurrentState { get { return currentGunState; } set { currentGunState = value; } }
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //if (currentGunState != gunState.inactive)
        //{
        //    if (CDReload < reloadTime)
        //    {
        //        CDReload += Time.deltaTime;
        //    }
        //    else if (currentGunState != gunState.shoot)
        //    {
                
        //        //Shoot();
        //        StartCoroutine(Shoot());

        //        //Reload();
                
        //    }
        //}
        if (currentGunState == gunState.active)
        {
            StartCoroutine(Shoot());
        }
        //.DrawLine(transform.position, transform.right);
    }

    IEnumerator Shoot()
    {
        currentGunState = gunState.shoot;
        magLeft--;
        for (float i = 0; i < bulletNums; i++)
        {
            Transform bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
            float angle = Mathf.Deg2Rad * (transform.localEulerAngles.z + (i - (bulletNums - 1) / 2) * spreadAngle); //(shootAngle - (i-1)/2 * spreadAngle);
            //.Log("Goc" + (i - (bulletNums - 1) / 2));
            Vector2 vector = new Vector2(bulletSpeed * Mathf.Cos(angle), bulletSpeed * Mathf.Sin(angle));
            //.DrawLine(transform.position, new Vector3(vector.x, vector.y) + transform.position);
            bullet.GetComponent<BulletPhysic>().velocity = vector;
        }
        yield return new WaitForSeconds(fireRate);
        if (currentGunState != gunState.inactive)
        {
            if (magLeft > 0) { StartCoroutine(Shoot()); }
            else { StartCoroutine(Reload()); }
        }
    }

    IEnumerator Reload()
    {
        CDReload = 0f;
        currentGunState = gunState.reloading;
        yield return new WaitForSeconds(reloadTime);
        magLeft = magSize;
        currentGunState = gunState.active;
    }
}
