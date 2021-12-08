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
        // state
        DialogueMap selectedDialogueMap = null;
        [NonSerialized] GUIStyle npcNodeStyle;
        [NonSerialized] GUIStyle playerNodeStyle;
        [NonSerialized] DialogueNode draggingNode = null;
        [NonSerialized] Vector2 draggingOffset;
        [NonSerialized] DialogueNode creatingNode = null;
        [NonSerialized] DialogueNode deletingNode = null;
        [NonSerialized] DialogueNode linkingParentNode = null;
        Vector2 scrollPosition;

        // constants
        const float canvasWidth = 4000f;
        const float canvasHeight = 4000f;
        const float backgroundSize = 50f;

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

            BuildNPCNodeStyle();
            BuildPlayerNodeStyle();
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

        void BuildNPCNodeStyle()
        {
            npcNodeStyle = new GUIStyle();
            npcNodeStyle.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
            npcNodeStyle.padding = new RectOffset(20, 20, 20, 20);
            npcNodeStyle.border = new RectOffset(12, 12, 12, 12);
        }

        void BuildPlayerNodeStyle()
        {
            playerNodeStyle = new GUIStyle();
            playerNodeStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D;
            playerNodeStyle.padding = new RectOffset(20, 20, 20, 20);
            playerNodeStyle.border = new RectOffset(12, 12, 12, 12);
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
                ProcessEvents();
                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

                Rect canvas = GUILayoutUtility.GetRect(canvasWidth, canvasHeight);
                Texture2D backgroundTexture = Resources.Load("background") as Texture2D;
                Rect textureCoordinates = new Rect(0, 0, canvasWidth / backgroundSize, canvasHeight / backgroundSize);
                GUI.DrawTextureWithTexCoords(canvas, backgroundTexture, textureCoordinates);


                foreach (DialogueNode node in selectedDialogueMap.GetAllNodes())
                {
                    DrawConnections(node);
                }
                foreach (DialogueNode node in selectedDialogueMap.GetAllNodes())
                {
                    DrawNode(node);
                }

                EditorGUILayout.EndScrollView();

                if (creatingNode != null)
                {
                    selectedDialogueMap.CreateNode(creatingNode, true);
                    creatingNode = null;
                }
                if (deletingNode != null)
                {
                    selectedDialogueMap.DeleteNode(deletingNode);
                    deletingNode = null;
                }
            }            
        }

        void ProcessEvents()
        {
            if (Event.current.type == EventType.MouseDown && draggingNode == null)
            {
                draggingNode = GetNodeAtPoint(Event.current.mousePosition + scrollPosition);
                if (draggingNode != null)
                {
                    draggingOffset = draggingNode.GetPosition() - Event.current.mousePosition;
                    Selection.activeObject = draggingNode;
                }
                else
                {
                    draggingOffset = Event.current.mousePosition + scrollPosition;
                    Selection.activeObject = selectedDialogueMap;
                }
            }
            else if (Event.current.type == EventType.MouseDrag && draggingNode != null)
            {
                draggingNode.SetPosition(Event.current.mousePosition + draggingOffset);
                GUI.changed = true;
            }
            else if (Event.current.type == EventType.MouseUp && draggingNode != null)
            {
                draggingNode = null;
            }
            else if (Event.current.type == EventType.MouseDrag)
            {
                scrollPosition = draggingOffset - Event.current.mousePosition;
                GUI.changed = true;
            }
        }

        void DrawNode(DialogueNode node)
        {
            if (node == null) return;

            GUIStyle nodeStyle = npcNodeStyle;

            if (node.IsPlayerSpeaking())
            {
                nodeStyle = playerNodeStyle;
            }

            GUILayout.BeginArea(new Rect(node.GetPosition().x, node.GetPosition().y, node.GetWidth(), node.GetHeight()), nodeStyle);

            node.SetText(EditorGUILayout.TextField(node.GetText()));

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("+"))
            {
                creatingNode = node;
            }

            DrawLinkButtons(node);

            if (GUILayout.Button("x"))
            {
                deletingNode = node;
            }

            GUILayout.EndHorizontal();

            GUILayout.EndArea();
        }

        private void DrawConnections(DialogueNode parentNode)
        {
            if (parentNode == null) return;
            Rect parentNodeRect = new Rect(parentNode.GetPosition().x, parentNode.GetPosition().y, parentNode.GetWidth(), parentNode.GetHeight());
            Vector3 startPosition = new Vector3(parentNodeRect.xMax, parentNodeRect.center.y, 0);

            foreach (DialogueNode childNode in selectedDialogueMap.GetAllChildren(parentNode))
            {
                Rect childNodeRect = new Rect(childNode.GetPosition().x, childNode.GetPosition().y, childNode.GetWidth(), childNode.GetHeight());
                Vector3 endPosition = new Vector3(childNodeRect.xMin, childNodeRect.center.y, 0);
                Vector3 controlPointOffset = endPosition - startPosition;
                controlPointOffset.y = 0;
                controlPointOffset.x *= .8f;
                Handles.DrawBezier(startPosition, endPosition, startPosition + controlPointOffset, endPosition - controlPointOffset, Color.white, null, 4f);
            }
        }

        DialogueNode GetNodeAtPoint(Vector2 point)
        {
            DialogueNode foundNode = null;

            foreach (DialogueNode node in selectedDialogueMap.GetAllNodes())
            {
                if (node == null) break;
                Rect nodeRect = new Rect(node.GetPosition().x, node.GetPosition().y, node.GetWidth(), node.GetHeight());
                if (nodeRect.Contains(point))
                {
                    foundNode = node;
                }
            }
            return foundNode;
        }

        private void DrawLinkButtons(DialogueNode node)
        {
            if (linkingParentNode == null)
            {
                if (GUILayout.Button("link"))
                {
                    linkingParentNode = node;
                }
            }
            else if (linkingParentNode == node)
            {
                if (GUILayout.Button("cancel"))
                {
                    linkingParentNode = null;
                }
            }
            else if (linkingParentNode.GetChildren().Contains(node.name))
            {
                if (GUILayout.Button("unlink"))
                {
                    linkingParentNode.RemoveChild(node.name);
                    linkingParentNode = null;
                }
            }
            else
            {
                if (GUILayout.Button("child"))
                {
                    linkingParentNode.AddChild(node.name);
                    linkingParentNode = null;
                }
            }
        }
    }
}
