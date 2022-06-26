using RPG.Control;
using RPG.Movement;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class SceneChanger : MonoBehaviour
    {
        // NOTE: This component must be placed on a gameObject at root level in hierarchy (must not have a parent object).
        // This is so DontDestroyOnLoad works properly.
        
        // configs
        [SerializeField] int sceneToLoad = -1;
        [SerializeField] float fadeOutTime = 1f;
        [SerializeField] float fadeInTime = 2f;
        [SerializeField] float fadeWaitTime = 0.5f;

        // cached references
        SavingWrapper savingWrapper;
        GameObject player;
        PlayerController playerController;

        // Unity messages
        void Awake()
        {
            savingWrapper = FindObjectOfType<SavingWrapper>();
            player = GameObject.FindWithTag("Player");
            playerController = player.GetComponent<PlayerController>();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                LoadScene();
            }
        }

        // public methods
        public void LoadScene() // called in Unity inspector
        {
            StartCoroutine(LoadSceneCoroutine());
        }

        // private methods
        private IEnumerator LoadSceneCoroutine()
        {
            if (sceneToLoad < 0)
            {
                Debug.LogError("Scene to load not set.");
                yield break;
            }
            DontDestroyOnLoad(gameObject);
            Fader fader = FindObjectOfType<Fader>();
            playerController.enabled = false;
            yield return fader.FadeOut(fadeOutTime);
            savingWrapper.Save();
            yield return SceneManager.LoadSceneAsync(sceneToLoad);
            PlayerController newPlayerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            newPlayerController.enabled = false;
            savingWrapper.Save();
            yield return new WaitForSeconds(fadeWaitTime);
            fader.FadeIn(fadeInTime);
            newPlayerController.enabled = true;
            Destroy(gameObject);
        }
    }
}
