﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.SceneManagement;
using SS.Scene;
public class GotoNextStage : MonoBehaviour {
	public Transform NextStagePortal;
	public Transform DataMaster;
	private NextStageCheck _NextStageCheck;
	private bool canGoNextStage = false;
	public string NextScene = "trongTestBan2";
	// Use this for initialization
	void Start () {
		_NextStageCheck = NextStagePortal.GetComponent<NextStageCheck>();
	}
	
	// Update is called once per frame
	void Update () {
		//canGoNextStage = _NextStageCheck.PlayerEnter;
		//if (canGoNextStage) {
		//	// DataMaster.GetComponent<GameInit>()._GameData._Progress.stage = NextScene;
		//	GameData.current._Progress.checkPoint = null;
		//	GameData.current._Progress.stage = NextScene;
		//	SaveLoadGame.Save();
		//	SceneManager.LoadScene(NextScene);
			
		//}
	}

    public void goToNextStage()
    {
        GameData.current._Progress.checkPoint = null;
        GameData.current._Progress.stage = NextScene;
        
        //SceneManager.LoadScene(NextScene);
        SceneManager.LoadScene(Stage2SSController.STAGE2SS_SCENE_NAME);
    }

}
