using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

[CustomEditor(typeof(StateMachine))]
public class LoadStateEditor : Editor {
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
		if(GUILayout.Button("Save State"))
        {
            GameMaster.instance.StateMachine.LoadGameState();
        }
        if(GUILayout.Button("Load State"))
        {
            GameMaster.instance.StateMachine.LoadGameState();
        }
    }
}
