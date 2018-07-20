using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.Pooling;

public class EnemyMasterScript : MonoBehaviour
{
    public List<GameObject> enemyList = new List<GameObject>();
    public GameObject enemySpawnerParent;
	//public Transform player;
    // Use this for initialization
    void Start()
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("EnemySpawner"))
        {
			enemyList.Add(enemy);
			//.Log(enemy);
			enemy.GetComponent<EnemySpawner>().enabled = true;
			// enemy.GetComponent<EnemySpawner>().player = player;
        };
        enemySpawnerParent = GameObject.Find("SpawnerParent");
    }

    // Update is called once per frame
    // void Update()
    // {

    // }
	public void resetSpawner(){
        enemySpawnerParent.BroadcastMessage("Reset");
		//foreach(GameObject spawner in enemyList){
		//	spawner.GetComponent<EnemySpawner>().resetSpawner();
		//}
	}
}
