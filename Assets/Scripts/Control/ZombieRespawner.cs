using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Control
{
    public class ZombieRespawner : MonoBehaviour
    {
        // state
        Zombie[] zombies;
        int numZombiesDead = 0;

        void Awake()
        {
            zombies = FindObjectsOfType<Zombie>();
        }

        public void IncrementDeadZombies()
        {
            numZombiesDead++;

            if (numZombiesDead == zombies.Length)
            {
                RespawnZombies();
                numZombiesDead = 0;
            }
        }

        private void RespawnZombies()
        {
            foreach (Zombie zombie in zombies)
            {
                StartCoroutine(zombie.Respawn());
            }
        }
    }
}
