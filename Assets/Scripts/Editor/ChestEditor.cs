using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Chest))]
public class ChestEditor : Editor
{ // this script must be in "Editor" folder
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Container.Table((Container)target);
    }
}
