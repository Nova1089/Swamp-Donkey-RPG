using RPG.Stats;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class TraitRowUI : MonoBehaviour
    {
        // configs
        [SerializeField] private Trait trait;
        [SerializeField] private TextMeshProUGUI valueText;
        [SerializeField] private Button minusButton;
        [SerializeField] private Button plusButton;

        // cache
        TraitStore playerTraitStore;

        void Start()
        {
            playerTraitStore = GameObject.FindGameObjectWithTag("Player").GetComponent<TraitStore>();
        }

        void Update()
        {
            minusButton.interactable = playerTraitStore.CanStagePoints(trait, -1);
            plusButton.interactable = playerTraitStore.CanStagePoints(trait, +1);
            valueText.text = playerTraitStore.GetProposedPoints(trait).ToString();
        }

        // called by plus and minus buttons
        public void StagePoints(int points)
        {
            playerTraitStore.StagePoints(trait, points);
        }
    }
}
