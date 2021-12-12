using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.Dialogue
{
    public class DialogueNode : ScriptableObject
    {
        // configs
        [SerializeField] private bool isPlayerSpeaking = false;
        [SerializeField] private string text;
        [SerializeField] private List<string> children = new List<string>();
        [SerializeField] private Vector2 position = new Vector2(10, 10);
        [SerializeField] private float width = 200f;
        [SerializeField] private float height = 100f;
        [SerializeField] private string onEnterAction;
        [SerializeField] private string onExitAction;


        // getters and setters
        public bool IsPlayerSpeaking()
        {
            return isPlayerSpeaking;
        }

        public string GetText()
        {
            return text;
        }

        public List<string> GetChildren()
        {
            return children;
        }

        public Vector2 GetPosition()
        {
            return position;
        }

        public float GetWidth()
        {
            return width;
        }

        public float GetHeight()
        {
            return height;
        }

        public string GetOnEnterAction()
        {
            return onEnterAction;
        }

        public string GetOnExitAction()
        {
            return onExitAction;
        }

#if UNITY_EDITOR

        public void SetText(string newText)
        {
            if (newText != text)
            {
                Undo.RecordObject(this, "Change dialogue text");
                text = newText;
                EditorUtility.SetDirty(this);
            }
        }

        public void AddChild(string childID)
        {
            Undo.RecordObject(this, "Add node link");
            children.Add(childID);
            EditorUtility.SetDirty(this);
        }

        public void RemoveChild(string childID)
        {
            Undo.RecordObject(this, "Remove node link");
            children.Remove(childID);
            EditorUtility.SetDirty(this);
        }

        public void SetPosition(Vector2 newPosition)
        {
            Undo.RecordObject(this, "Change node position");
            position = newPosition;
            EditorUtility.SetDirty(this);
        }

        public void SetWidth(float newWidth)
        {
            Undo.RecordObject(this, "Change node width");
            width = newWidth;
            EditorUtility.SetDirty(this);
        }

        public void SetHeight(float newHeight)
        {
            Undo.RecordObject(this, "Change node height");
            height = newHeight;
            EditorUtility.SetDirty(this);
        }

        public void SetIsPlayerSpeaking(bool newBoolean)
        {
            Undo.RecordObject(this, "Change dialogue speaker");
            isPlayerSpeaking = newBoolean;
            EditorUtility.SetDirty(this);
        }
#endif
    }
}
