using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Dialogue
{
    public class PlayerConversant : MonoBehaviour
    {
        // state
        DialogueMap currentDialogueMap;
        DialogueNode currentNode = null;
        bool isChoosing = false;

        // events
        public event Action onConversationUpdated;

        // methods
        public void StartDialogue(DialogueMap newDialogueMap)
        {
            if (newDialogueMap == null) return;
            currentDialogueMap = newDialogueMap;
            currentNode = currentDialogueMap.GetRootNode();
            onConversationUpdated();
        }

        public void Quit()
        {
            currentDialogueMap = null;
            currentNode = null;
            isChoosing = false;
            onConversationUpdated();
        }

        public bool IsActive()
        {
            return currentDialogueMap != null;
        }

        public bool IsChoosing()
        {
            return isChoosing;
        }

        public string GetText()
        {
            if (currentNode == null) return "";
            return currentNode.GetText();
        }

        public IEnumerable<DialogueNode> GetChoices()
        {
            return currentDialogueMap.GetPlayerChildren(currentNode);
        }

        public void SelectChoice(DialogueNode chosenNode)
        {
            currentNode = chosenNode;
            isChoosing = false;
            Next();
        }

        public void Next()
        {
            int numPlayerResponses = currentDialogueMap.GetPlayerChildren(currentNode).Count();
            if (numPlayerResponses > 0)
            {
                isChoosing = true;
                onConversationUpdated();
                return;
            }
            DialogueNode[] npcChildrenNodes = currentDialogueMap.GetNPCChildren(currentNode).ToArray();
            int randomIndex = UnityEngine.Random.Range(0, npcChildrenNodes.Length);
            currentNode = npcChildrenNodes[randomIndex];
            onConversationUpdated();
        }

        public bool HasNext()
        {
            return currentDialogueMap.GetAllChildren(currentNode).Count() > 0;
        }
    }
}
