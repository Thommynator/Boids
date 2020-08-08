using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RuleConfigerer))]
public class RuleConfigererEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        RuleConfigerer config = (RuleConfigerer)target;

        if (GUILayout.Button("Avoid Walls"))
        {
            config.ToggleAvoidWallsRule();
        }

        if (GUILayout.Button("Separate"))
        {
            config.ToggleSeparateFromOthersRule();
        }
    }
}
