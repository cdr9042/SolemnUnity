using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadProgress : MonoBehaviour {
	// public GameData _GameData;
	// Use this for initialization
	void Start () {
        SaveLoadGame.Load();
		// _GameData = new GameData();
		// Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer ("PlayerLayer"), LayerMask.NameToLayer ("EnemyLayer"));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
