using RPG.Stats;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class TraitsUI : MonoBehaviour
    {
        // configs
        [SerializeField] private TextMeshProUGUI unallocatedPointsText;

        // cache
        TraitStore traitStore;

        void Awake()
        {
            traitStore = GameObject.FindGameObjectWithTag("Player").GetComponent<TraitStore>();
        }

        void Update()
        {
            unallocatedPointsText.text = traitStore.GetUnstagedPoints().ToString();
        }

        // called by Confirm button
        public void Confirm()
        {
            traitStore.Commit();
        }
    }
}
