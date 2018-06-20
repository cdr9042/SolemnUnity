using UnityEngine;
using System.Collections;

[System.Serializable]
public class GameData { //don't need ": Monobehaviour" because we are not attaching it to a game object

	public static GameData current;
	public Progress _Progress;
	public static string gameMode;

	public GameData () {
		_Progress = new Progress ();
		gameMode = "";
	}
	public void setGameMode (string gM) {
		gameMode = gM;
	}
	public string getGameMode (){
		return gameMode;
	}
}
