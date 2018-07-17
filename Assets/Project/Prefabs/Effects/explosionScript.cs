using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.Pooling;
public class explosionScript : MonoBehaviour {
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] string prefabKey;
    [SerializeField] float explodeRadius = 1f;
    [SerializeField] float explodeInterval = 0.2f;
    public float explosionTimeLength;
	// Use this for initialization
	void Start () {
        if (!SpawningPool.ContainsPrefab(prefabKey))
        {
            SpawningPool.AddPrefab(prefabKey, explosionPrefab);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartExplosion(float expTime)
    {
        explosionTimeLength = expTime;
        StartCoroutine(explosionTiming());
        StartCoroutine(endExplosion());
    }

    IEnumerator explosionTiming()
    {
        Vector3 explodePos = new Vector3(transform.position.x + Random.Range(-explodeRadius, explodeRadius), transform.position.y + Random.Range(-explodeRadius, explodeRadius), 0);
        createAnExplosion(explodePos);
        GameObject explode = SpawningPool.CreateFromCache(prefabKey);
        if (explode != null)
        {
            explode.transform.position = GeneralHelper.AsVector2(transform.position) + Random.insideUnitCircle * explodeRadius;
        }
        else
            Debug.Log("Spawning Pool return null");
        
        yield return new WaitForSeconds(explodeInterval);
        StartCoroutine(explosionTiming());
    }
    IEnumerator endExplosion()
    {
        yield return new WaitForSeconds(explosionTimeLength);
        GameObject.Destroy(gameObject);
    }

    void createAnExplosion(Vector3 explodePos)
    {
    }

    public void returnToCache (GameObject objCall)
    {
        SpawningPool.ReturnToCache(objCall);
    }


}
