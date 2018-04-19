using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

[CustomEditor(typeof(GUI_Object))]
public class PositionSetEditor : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUI_Object GUIobj = (GUI_Object)target;

        if(GUILayout.Button("Set Animation Target Pos"))
        {
            Debug.Log("I GOT MY LOCS ON");
            GUIobj.SetAnimationTarget(GUIobj.transform.position);
        }
    }

}
