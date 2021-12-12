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
        [SerializeField] GameObject npcResponse;
        [SerializeField] Button quitButton;

        void Awake()
        {
            playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
        }

        void OnEnable()
        {
            playerConversant.onConversationUpdated += UpdateUI;
            nextButton.onClick.AddListener(Next);
            quitButton.onClick.AddListener(Quit);            
        }

        void Start()
        {
            UpdateUI();
        }

        void Quit()
        {
            playerConversant.Quit();
        }

        void Next()
        {
            playerConversant.Next();
        }

        private void UpdateUI()
        {
            gameObject.SetActive(playerConversant.IsActive());

            if (!playerConversant.IsActive()) return;

            if (playerConversant.IsChoosing())
            {
                DisplayChoiceList();
            }
            else
            {
                DisplayNPCText();
            }
        }

        private void DisplayChoiceList()
        {
            foreach (Transform item in choiceRoot)
            {
                Destroy(item.gameObject);
            }
            foreach (DialogueNode choice in playerConversant.GetChoices())
            {
                GameObject choiceButtonInstance = Instantiate(choiceButtonPrefab, choiceRoot);
                var TMPComponent = choiceButtonInstance.GetComponentInChildren<TextMeshProUGUI>();
                TMPComponent.text = choice.GetText();
                Button button = choiceButtonInstance.GetComponentInChildren<Button>();
                button.onClick.AddListener(() =>
                {
                    playerConversant.SelectChoice(choice);
                });
            }

            choiceRoot.gameObject.SetActive(true);
            npcResponse.SetActive(false);
        }

        private void DisplayNPCText()
        {
            npcText.text = playerConversant.GetText();
            nextButton.gameObject.SetActive(playerConversant.HasNext());
            npcResponse.SetActive(true);
            choiceRoot.gameObject.SetActive(false);
        }
    }
}
