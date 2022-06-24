using RPG.Control;
using RPG.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI
{
    public class PauseMenuUI : MonoBehaviour
    {
        // cache
        PlayerController playerController;
        SavingWrapper savingWrapper;

        void Awake()
        {
            playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }

        void OnEnable()
        {            
            PauseGame(true);
        }

        void OnDisable()
        {
            PauseGame(false);
        }
        
        public void Save()
        {
            savingWrapper = FindObjectOfType<SavingWrapper>();
            savingWrapper.Save();
        }

        public void SaveAndQuit()
        {
            Save();
            savingWrapper.LoadMainMenu();
        }

        private void PauseGame(bool pauseState)
        {
            if (playerController == null) return;

            if (pauseState == true)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 5f;
            }

            playerController.enabled = !pauseState;
        }
    }
}
