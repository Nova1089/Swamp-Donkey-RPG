using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.Dialogue
{
    [CreateAssetMenu(fileName = "New Dialogue Map", menuName = "Dialogue Map", order = 0)]
    public class DialogueMap : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] List<DialogueNode> nodes = new List<DialogueNode>();
        Dictionary<string, DialogueNode> nodeLookup = new Dictionary<string, DialogueNode>();
        [SerializeField] Vector2 newNodeOffset = new Vector2(25f, 0);

#if UNITY_EDITOR
        public void CreateNode(DialogueNode parentNode, bool recordInUndoSystem)
        {
            DialogueNode newNode = CreateInstance<DialogueNode>();
            newNode.name = Guid.NewGuid().ToString();

            if (parentNode != null)
            {
                Vector2 offset = new Vector2(newNodeOffset.x + parentNode.GetWidth(), newNodeOffset.y);
                newNode.SetPosition(parentNode.GetPosition() + offset);
                parentNode.AddChild(newNode.name);
                newNode.SetIsPlayerSpeaking(!parentNode.IsPlayerSpeaking());
            } 

            if (recordInUndoSystem)
            {
                Undo.RegisterCompleteObjectUndo(newNode, "Create dialogue node");
                Undo.RecordObject(this, "Add dialogue node");
            }

            nodes.Add(newNode);
            OnValidate();
        }

        public void DeleteNode(DialogueNode nodeToDelete)
        {
            Undo.RecordObject(this, "Delete dialogue node");
            nodes.Remove(nodeToDelete);
            OnValidate();
            CleanDanglingChildren(nodeToDelete);
            Undo.DestroyObjectImmediate(nodeToDelete);
        }

        private void CleanDanglingChildren(DialogueNode childNodeToDelete)
        {
            foreach (DialogueNode node in GetAllNodes())
            {
                node.RemoveChild(childNodeToDelete.name);
            }
        }
#endif
        // Also called by Unity Editor when script is loaded or a value changes in the inspector.
        void OnValidate()
        {
            BuildNodeLookup();
        }

        private void BuildNodeLookup()
        {
            nodeLookup.Clear();
            foreach (DialogueNode node in GetAllNodes())
            {
                if (node != null)
                {
                    nodeLookup[node.name] = node;
                }
            }
        }

        public IEnumerable<DialogueNode> GetAllNodes()
        {
            return nodes;
        }

        public DialogueNode GetRootNode()
        {
            return nodes[0];
        }

        public IEnumerable<DialogueNode> GetAllChildren(DialogueNode parentNode)
        {
            foreach (string childID in parentNode.GetChildren())
            {
                if (nodeLookup.ContainsKey(childID))
                {
                    yield return nodeLookup[childID]; 
                }
            }
        }

        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            if (nodes.Count == 0)
            {
                CreateNode(null, false);
            }

            if (AssetDatabase.GetAssetPath(this) != "")
            {
                foreach (DialogueNode node in GetAllNodes())
                {
                    if (AssetDatabase.GetAssetPath(node) == "")
                    {
                        AssetDatabase.AddObjectToAsset(node, this);
                    }
                }
            }
#endif
        }

        public void OnAfterDeserialize()
        {
        }
    } 
}
