using RPG.Control;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class SceneChanger : MonoBehaviour
    {
        // configs
        [SerializeField] int sceneToLoad = -1;
        [SerializeField] float fadeOutTime = 1f;
        [SerializeField] float fadeInTime = 2f;
        [SerializeField] float fadeWaitTime = 0.5f;

        // state


        // cached references
        Fader fader;
        SavingWrapper savingWrapper;
        GameObject player;
        PlayerController playerController;

        // Unity messages
        void Awake()
        {
            fader = FindObjectOfType<Fader>();
            savingWrapper = FindObjectOfType<SavingWrapper>();
            player = GameObject.FindWithTag("Player");
            playerController = player.GetComponent<PlayerController>();
        }

        // public methods
        public void LoadScene()
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
            playerController.enabled = false;
            yield return fader.FadeOut(fadeOutTime);
            savingWrapper.Save();
            yield return SceneManager.LoadSceneAsync(sceneToLoad);
            PlayerController newPlayerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            newPlayerController.enabled = false;
            savingWrapper.Load();
            savingWrapper.Save();
            yield return new WaitForSeconds(fadeWaitTime);
            fader.FadeIn(fadeInTime);
            newPlayerController.enabled = true;
            Destroy(gameObject);
        } 
    }
}
