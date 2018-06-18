using UnityEngine;
using System.Collections;

[System.Serializable]
public class GameData { //don't need ": Monobehaviour" because we are not attaching it to a game object

	public static GameData current;
	public Progress _Progress;

	public GameData () {
		_Progress = new Progress ();
	}
}
