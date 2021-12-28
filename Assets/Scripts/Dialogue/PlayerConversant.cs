using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Dialogue
{
    public class PlayerConversant : MonoBehaviour
    {
        // configs
        [SerializeField] private string playerName;

        // state
        DialogueMap currentDialogueMap;
        DialogueNode currentNode = null;
        NPCConversant currentNPCConversant = null;
        bool isChoosing = false;

        // events
        public event Action onConversationUpdated;

        // methods
        public void StartDialogue(NPCConversant newNPCConversant, DialogueMap newDialogueMap)
        {
            currentNPCConversant = newNPCConversant;
            if (newDialogueMap == null) return;
            currentDialogueMap = newDialogueMap;
            currentNode = currentDialogueMap.GetRootNode();
            TriggerEnterAction();
            onConversationUpdated();
        }

        public void Quit()
        {
            currentDialogueMap = null;
            TriggerExitAction();
            currentNode = null;
            isChoosing = false;
            currentNPCConversant = null;
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
            return FilterOnCondition(currentDialogueMap.GetPlayerChildren(currentNode));
        }

        public void SelectChoice(DialogueNode chosenNode)
        {            
            currentNode = chosenNode;
            TriggerEnterAction();
            isChoosing = false;            
            Next();
        }

        public void Next()
        {
            int numPlayerResponses = FilterOnCondition(currentDialogueMap.GetPlayerChildren(currentNode)).Count();
            if (numPlayerResponses > 0)
            {
                isChoosing = true;
                TriggerExitAction();
                onConversationUpdated();
                return;
            }
            TriggerExitAction();
            DialogueNode[] npcChildrenNodes = FilterOnCondition(currentDialogueMap.GetNPCChildren(currentNode)).ToArray();
            if (npcChildrenNodes.Length == 0)
            {
                Quit();
                return;
            }
            int randomIndex = UnityEngine.Random.Range(0, npcChildrenNodes.Length);            
            currentNode = npcChildrenNodes[randomIndex];
            TriggerEnterAction();
            onConversationUpdated();            
        }

        public bool HasNext()
        {
            return FilterOnCondition(currentDialogueMap.GetAllChildren(currentNode)).Count() > 0;
        }

        private IEnumerable<DialogueNode> FilterOnCondition(IEnumerable<DialogueNode> inputNodes)
        {
            foreach (DialogueNode node in inputNodes)
            {
                if (node.CheckCondition(GetEvaluators()))
                {
                    yield return node;
                }
            }
        }

        private IEnumerable<IPredicateEvaluator> GetEvaluators()
        {
            return GetComponents<IPredicateEvaluator>();
        }

        void TriggerEnterAction()
        {
            if (currentNode != null)
            {
                if (currentNPCConversant == null) return;
                TriggerAction(currentNode.GetOnEnterAction());
            }
        }

        void TriggerExitAction()
        {
            if (currentNode != null)
            {
                if (currentNPCConversant == null) return;
                TriggerAction(currentNode.GetOnExitAction());
            }
        }

        private void TriggerAction(string action)
        {
            if (action == "") return;
            DialogueTrigger[] npcDialogueTriggers = currentNPCConversant.GetComponents<DialogueTrigger>();
            if (npcDialogueTriggers.Length == 0) return;
            foreach (DialogueTrigger trigger in npcDialogueTriggers)
            {
                trigger.Trigger(action);
            }
        }

        public string GetCurrentSpeakerName()
        {
            if (isChoosing)
            {
                return playerName;
            }
            else
            {
                return currentNPCConversant.GetName();
            }
        }
    }
}
