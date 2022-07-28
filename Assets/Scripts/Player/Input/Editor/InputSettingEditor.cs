using UnityEngine;
using UnityEditor;

namespace XVerse.Player.Input
{
    [CustomEditor(typeof(InputSetting))]
    internal class InputSettingEditor : Editor
    {
        private InputSetting inputSetting;
        private void OnEnable()
        {
            inputSetting = target as InputSetting;
        }

        public override void OnInspectorGUI()
        {
            GUILayout.Label("Player Input Setting", EditorStyles.boldLabel);
            GUILayout.Space(5);
            EditorGUI.BeginDisabledGroup(true);
            inputSetting.InputSettingName = EditorGUILayout.TextField("Input Setting Name", inputSetting.InputSettingName);
            EditorGUI.EndDisabledGroup();
            GUILayout.Space(5);
            if (GUILayout.Button("Open Setting Window")) { InputSettingWindow.ShowWindow(inputSetting); }
            GUILayout.Space(5);

            serializedObject.Update();
            serializedObject.ApplyModifiedProperties();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
                AssetDatabase.SaveAssets();
            }
        }
    }
}