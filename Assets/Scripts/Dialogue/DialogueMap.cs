using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogue
{
    [CreateAssetMenu(fileName = "New Dialogue Map", menuName = "Dialogue Map", order = 0)]
    public class DialogueMap : ScriptableObject
    {
        [SerializeField] DialogueNode[] nodes;
    }
}
