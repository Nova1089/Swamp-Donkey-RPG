using GameDevTV.Saving;
using GameDevTV.Utils;
using RPG.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class LoadSavedGameUI : MonoBehaviour
    {
        // configs
        [SerializeField] private Transform contentRoot;
        [SerializeField] private GameObject buttonPrefab;

        // cache
        SavingWrapper savingWrapper;

        void OnEnable()
        {
            if (contentRoot == null) return;
            if (buttonPrefab == null) return;
            savingWrapper = FindObjectOfType<SavingWrapper>();
            if (savingWrapper == null) return;

            foreach (Transform child in contentRoot)
            {
                Destroy(child.gameObject);
            }
            SpawnButtons();
        }

        public void SpawnButtons()
        {
            foreach (string fileName in savingWrapper.ListSaves())
            {
                GameObject instance = Instantiate(buttonPrefab, contentRoot);
                instance.GetComponentInChildren<TextMeshProUGUI>().text = fileName;
                Button button = instance.GetComponentInChildren<Button>();
                button.onClick.AddListener(() => savingWrapper.LoadGame(fileName));
            }            
        }

        private SavingWrapper GetSavingWrapper()
        {
            return FindObjectOfType<SavingWrapper>();
        }
    }  
}
