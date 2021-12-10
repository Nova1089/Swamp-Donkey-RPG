using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Dialogue;
using TMPro;
using UnityEngine.UI;

namespace RPG.UI
{
    public class DialogueUI : MonoBehaviour
    {
        PlayerConversant playerConversant;
        [SerializeField] TextMeshProUGUI npcText;
        [SerializeField] Button nextButton;
        [SerializeField] Transform choiceRoot;
        [SerializeField] private GameObject choiceButtonPrefab;



        void Awake()
        {
            playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
        }

        void OnEnable()
        {
            nextButton.onClick.AddListener(Next);
        }

        void Start()
        {
            UpdateUI();
        }

        void Next()
        {
            playerConversant.Next();
            UpdateUI();
        }

        private void UpdateUI()
        {
            npcText.text = playerConversant.GetText();
            nextButton.gameObject.SetActive(playerConversant.HasNext());
            foreach (Transform item in choiceRoot)
            {
                Destroy(item.gameObject);
            }
            foreach (string choiceText in playerConversant.GetChoices())
            {
                GameObject choiceButtonInstance = Instantiate(choiceButtonPrefab, choiceRoot);
                var TMPComponent = choiceButtonInstance.GetComponentInChildren<TextMeshProUGUI>();
                TMPComponent.text = choiceText;
            }
        }
    }
}
