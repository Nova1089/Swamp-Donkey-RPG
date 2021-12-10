using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Dialogue
{
    public class PlayerConversant : MonoBehaviour
    {
        // configs
        [SerializeField] DialogueMap currentDialogueMap;

        // state
        DialogueNode currentNode = null;

        void Awake()
        {
            if (currentDialogueMap != null)
            {
                currentNode = currentDialogueMap.GetRootNode();
            }            
        }

        public string GetText()
        {
            if (currentNode == null) return "";
            return currentNode.GetText();
        }

        public IEnumerable<string> GetChoices()
        {
            yield return "Example text 1";
            yield return "Example text 2";
            yield return "Example text 3";
        }

        public void Next()
        {
            DialogueNode[] childrenNodes = currentDialogueMap.GetAllChildren(currentNode).ToArray();
            currentNode = childrenNodes[Random.Range(0, childrenNodes.Length)];      
        }

        public bool HasNext()
        {
            return currentDialogueMap.GetAllChildren(currentNode).Count() > 0;
        }
    }
}
