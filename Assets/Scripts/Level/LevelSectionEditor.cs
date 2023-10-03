
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(LevelSection))]
public class LevelSectionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        LevelSection section = target as LevelSection;

        if (GUILayout.Button("GenerateLevelData"))
        {
            if (section.Notations?.Length > 0)
            {
                Debug.LogWarning("Attempting to overrite level data. Delete the existing notations to regenerate");
                // consider replacing this with an "Overwrite?" confirmation button that activates when the user clicks this button
            }
            else
            {
                LevelGenerator.GenerateLevelSection(section);
            }
        }

        DrawDefaultInspector();
    }
}