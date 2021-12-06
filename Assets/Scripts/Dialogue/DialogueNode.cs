using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogue
{    
   [System.Serializable] public class DialogueNode
    {
        public string uniqueID;
        public string text;
        public List<string> children = new List<string>();
        public Vector2 position = new Vector2(10, 10);
        public float width = 200f;
        public float height = 100f;
    }
}
