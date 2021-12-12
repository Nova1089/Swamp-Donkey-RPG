using RPG.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogue
{
    public class npcConversant : MonoBehaviour, IRaycastable
    {
        // configs
        [SerializeField] DialogueMap dialogueMap = null;

        // state
        PlayerConversant playerConversant;

        public CursorType GetCursorType()
        {
            return CursorType.Dialogue;
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if (dialogueMap == null) return false;
            
            if (Input.GetMouseButtonDown(0))
            {
                playerConversant = callingController.GetComponent<PlayerConversant>();
                playerConversant.StartDialogue(dialogueMap);
            }
            return true;            
        }
    }
}
