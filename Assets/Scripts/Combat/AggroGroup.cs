using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class AggroGroup : MonoBehaviour
    {
        [SerializeField] private Fighter[] fighters;
        [SerializeField] private bool activateOnStart = false;


        void Start()
        {
            SetActivated(activateOnStart);
        }

        public void SetActivated(bool shouldActivate)
        {
            foreach (Fighter fighter in fighters)
            {                
                CombatTarget combatTarget = fighter.GetComponent<CombatTarget>();
                if (combatTarget != null)
                {
                    combatTarget.enabled = shouldActivate;
                }
                fighter.enabled = shouldActivate;
            }
        }
    }
}
