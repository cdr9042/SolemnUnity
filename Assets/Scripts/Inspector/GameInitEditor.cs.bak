using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameInit))]
public class GameInitEditor : Editor {

public string lastCheckpointName;
	public override void OnInspectorGUI()
    {
		if (GameData.current != null)
			lastCheckpointName = GameData.current._Progress.checkPoint;
		EditorGUILayout.LabelField("Level", lastCheckpointName);
    }
}
