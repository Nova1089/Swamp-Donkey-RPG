using System;
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
        GUIStyle nodeStyle;
        DialogueNode draggingNode = null;
        Vector2 draggingOffset;

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

            BuildNodeStyle();
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

        void BuildNodeStyle()
        {
            nodeStyle = new GUIStyle();
            nodeStyle.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
            nodeStyle.padding = new RectOffset(20, 20, 20, 20);
            nodeStyle.border = new RectOffset(12, 12, 12, 12);
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
                ProcessMouseEvents();
                foreach (DialogueNode node in selectedDialogueMap.GetAllNodes())
                {
                    OnGUINode(node);
                }
            }            
        }

        void ProcessMouseEvents()
        {
            if (Event.current.type == EventType.MouseDown && draggingNode == null)
            {
                draggingNode = GetNodeAtPoint(Event.current.mousePosition);
                if (draggingNode != null)
                {
                    draggingOffset = new Vector2();
                    draggingOffset.x = draggingNode.position.x - Event.current.mousePosition.x;
                    draggingOffset.y = draggingNode.position.y - Event.current.mousePosition.y;
                }
            }
            else if (Event.current.type == EventType.MouseDrag && draggingNode != null)
            {
                Undo.RecordObject(selectedDialogueMap, "Drag Node");
                draggingNode.position = Event.current.mousePosition + draggingOffset;
                GUI.changed = true;
            }
            else if (Event.current.type == EventType.MouseUp && draggingNode != null)
            {
                draggingNode = null;
            }        
        }

        void OnGUINode(DialogueNode node)
        {
            GUILayout.BeginArea(new Rect(node.position.x, node.position.y, node.width, node.height), nodeStyle);

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.LabelField("Node: ");
            string newText = EditorGUILayout.TextField(node.text);
            string newUniqueID = EditorGUILayout.TextField(node.uniqueID);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(selectedDialogueMap, "Update Node Values");
                node.text = newText;
                node.uniqueID = newUniqueID;
            }

            foreach (DialogueNode childNode in selectedDialogueMap.GetAllChildren(node))
            {
                EditorGUILayout.LabelField(childNode.text);
            }

            GUILayout.EndArea();
        }

        DialogueNode GetNodeAtPoint(Vector2 point)
        {
            DialogueNode foundNode = null;

            foreach (DialogueNode node in selectedDialogueMap.GetAllNodes())
            {
                if (point.x >= node.position.x && point.y >= node.position.y)
                {
                    if (point.x <= node.position.x + node.width && point.y <= node.position.y + node.height)
                    {
                        foundNode = node;
                    }
                }
            }
            return foundNode;
        }
    }
}
