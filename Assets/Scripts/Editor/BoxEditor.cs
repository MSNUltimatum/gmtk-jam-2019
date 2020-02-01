using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Box))]
public class BoxEditor : Editor 
{ // this script must be in "Editor" folder
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Container.Table((Container)target);        
    }
}
