using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI
{
    public class UISwitcher : MonoBehaviour
    {

        [SerializeField] private GameObject entryPoint;

        void Start()
        {
            SwitchTo(entryPoint);
        }

        public void SwitchTo(GameObject menuToDisplay)
        {
            if (menuToDisplay.transform.parent != this.transform) return;

            foreach (Transform childTransform in this.transform)
            {
                childTransform.gameObject.SetActive(childTransform.gameObject == menuToDisplay);
            }
        }
    }
}
