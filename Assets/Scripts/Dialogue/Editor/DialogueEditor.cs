using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace RPG.Dialogue.Editor
{
    public class DialogueEditor : EditorWindow
    {
        DialogueMap selectedDialogueMap = null;
        
        [MenuItem("Window/Dialogue Editor")]
        public static void ShowEditorWindow()
        {
            GetWindow(typeof(DialogueEditor), false, "Dialogue Editor");
        }

        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            if (EditorUtility.InstanceIDToObject(instanceID) as DialogueMap != null)
            {
                ShowEditorWindow();
                return true;
            }
            return false;
        }

        void OnEnable()
        {
            Selection.selectionChanged += OnSelectionChanged;
        }

        void OnSelectionChanged()
        {
            DialogueMap selectedMap = Selection.activeObject as DialogueMap;
            if (selectedMap != null)
            {
                selectedDialogueMap = selectedMap;
                Repaint();
            }
        }

        // Called when interacting with Unity Editor in any way.
        void OnGUI()
        {
            if (selectedDialogueMap == null)
            {
                EditorGUILayout.LabelField("No dialogue map selected.");
            }
            else
            {
                EditorGUILayout.LabelField(selectedDialogueMap.name);
            }
        }
    }
}
