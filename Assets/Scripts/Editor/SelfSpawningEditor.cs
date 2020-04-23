using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SelfSpawningContainer))]
public class SelfSpawningEditor : Editor
{ // this script must be in "Editor" folder
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Container.Table((Container)target);
    }
}
