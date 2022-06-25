using RPG.Control;
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
        [SerializeField] Vector3 playerSpawnLocation = new Vector3();
        [SerializeField] Vector3 playerSpawnRotation = new Vector3();

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
            savingWrapper.Load();
            UpdatePlayerLocation();
            savingWrapper.Save();
            yield return new WaitForSeconds(fadeWaitTime);
            fader.FadeIn(fadeInTime);
            newPlayerController.enabled = true;
            Destroy(gameObject);
        }

        private void UpdatePlayerLocation()
        {
            GameObject player = GameObject.FindWithTag("Player");
            NavMeshAgent navMeshAgent = player.GetComponent<NavMeshAgent>();
            navMeshAgent.enabled = false;
            if (navMeshAgent.enabled == false)
            {
                Debug.Log("Nav Mesh was successfully disabled");
            }
            else
            {
                Debug.Log("Nav mesh is still enabled");
            }
            Debug.Log("Player current location is: " + player.transform.position);
            Debug.Log("playerSpawnLocation is: " + playerSpawnLocation);
            player.transform.position = playerSpawnLocation;
            Debug.Log("Finished updating player location");
            // player.transform.Rotate(playerSpawnRotation, Space.World);
            navMeshAgent.enabled = true;
        }
    }
}
