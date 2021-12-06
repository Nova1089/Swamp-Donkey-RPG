using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogue
{    
   [System.Serializable] public class DialogueNode
    {
        public string uniqueID;
        public string text;
        public string[] children;
        public Vector2 position = new Vector2(0, 0);
        public float width = 200f;
        public float height = 100f;        
    }
}
