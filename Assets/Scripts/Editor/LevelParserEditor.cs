using UnityEditor;
using UnityEngine;

namespace TNNL.Level
{
    [CustomEditor(typeof(LevelParser))]
    public class LevelParserEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.Space(20);

            LevelParser parser = target as LevelParser;

            if (GUILayout.Button("LoadLevel"))
            {
                parser.LoadNextLevel();
            }

            if (GUILayout.Button("UnloadLevel"))
            {
                parser.ClearLevel();
            }
        }

    }
}