using UnityEngine;
using RPG.Quests;
using System;

namespace RPG.Utilities
{
    public class ComponentFinder : MonoBehaviour
    {

        void Start()
        {
            var foundObjects = FindObjectsOfType<QuestCompletion>();

            if (foundObjects.Length == 0)
            {
                Debug.Log("Found no objects containing searched component");
            }
            else
            {
                foreach (var foundObject in foundObjects)
                {
                    Debug.Log("Object containing component is: " + foundObject.gameObject.name);               
                }
            }
        }
    }
}
