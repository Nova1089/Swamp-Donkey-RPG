using Cinemachine;
using RPG.Attributes;
using RPG.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Control
{
    public class Respawner : MonoBehaviour
    {
        // configs
        [SerializeField] Transform respawnLocation;
        [SerializeField] private float respawnDelay = 2f;
        [SerializeField] private float fadeOutTime = 2f;
        [SerializeField] private float fadeInTime = 1f;
        [SerializeField] private float percentHealthToRegen = 20;

        // cache
        Health health;
        NavMeshAgent navMeshAgent;
        Fader fader;        

        void Awake()
        {
            health = GetComponent<Health>();
            navMeshAgent = GetComponent<NavMeshAgent>();            
        }

        void OnEnable()
        {
            health.onDie.AddListener(Respawn);
        }

        void Start()
        {
            if (health.IsDead())
            {
                Respawn();
            }
        }

        private void Respawn()
        {
            StartCoroutine(RespawnRoutine());
        }

        IEnumerator RespawnRoutine()
        {
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
            savingWrapper.Save();
            fader = FindObjectOfType<Fader>();
            yield return new WaitForSeconds(respawnDelay);
            yield return fader.FadeOut(fadeOutTime);
            RespawnPlayer();
            ResetEnemies();
            savingWrapper.Save();
            fader.FadeIn(fadeInTime);
        }

        private void ResetEnemies()
        {
            foreach (AIController enemyControllers in FindObjectsOfType<AIController>())
            {
                enemyControllers.ResetState();
            }
        }

        private void RespawnPlayer()
        {
            Vector3 positionDelta = respawnLocation.position - transform.position;
            navMeshAgent.Warp(respawnLocation.position);
            health.Heal(health.GetMaxHealthPoints() * percentHealthToRegen / 100);
            ICinemachineCamera activeVirtualCamera = FindObjectOfType<CinemachineBrain>().ActiveVirtualCamera;
            if (activeVirtualCamera.Follow == transform)
            {
                activeVirtualCamera.OnTargetObjectWarped(transform, positionDelta);
            }
        }
    }
}
