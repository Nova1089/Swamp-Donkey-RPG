using RPG.Attributes;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Control
{
    public class Zombie : MonoBehaviour
    {
        // configs
        [SerializeField] float metersToLower = 2.5f;
        [SerializeField] float popUpSpeed = .75f;

        // state
        Rigidbody rigidBody;
        AIController aiController;
        ActionScheduler actionScheduler;
        NavMeshAgent navMeshAgent;
        Fighter fighter;
        Mover mover;
        Health health;
        float aboveGroundY = 0;
        List<GameObject> children = new List<GameObject>();
        

        void Awake()
        {
            rigidBody = GetComponent<Rigidbody>();
            aiController = GetComponent<AIController>();
            actionScheduler = GetComponent<ActionScheduler>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            fighter = GetComponent<Fighter>();
            mover = GetComponent<Mover>();
            health = GetComponent<Health>();
            foreach (Transform child in transform)
            {
                children.Add(child.gameObject);
            }
        }

        public IEnumerator Respawn()
        {
            actionScheduler.CancelCurrentAction();
            SetChildrenEnabled(false);
            SetCombatEnabled(false);
            yield return new WaitUntil(aiController.PrepareForRespawn);
            aboveGroundY = transform.position.y;
            yield return new WaitUntil(DropUnderGround);
            SetChildrenEnabled(true);
            yield return new WaitUntil(PopUp);
            SetCombatEnabled(true);
            health.enabled = true;
        }

        void SetCombatEnabled(bool value)
        {
            health.enabled = value;
            fighter.enabled = value;
        }

        void SetChildrenEnabled(bool value)
        {
            foreach (GameObject child in children)
            {
                child.SetActive(value);
            }
        }

        bool DropUnderGround()
        {
            SetAIEnabled(false);
            Vector3 currentPos = rigidBody.position;
            Vector3 undergroundPos = currentPos + Vector3.down * metersToLower;
            transform.position = undergroundPos;
            if (transform.position == undergroundPos)
            {
                return true;
            }
            else return false;
        }

        bool PopUp()
        {
            Vector3 direction = rigidBody.position + Vector3.up * popUpSpeed * Time.deltaTime;
            rigidBody.MovePosition(direction);

            if (rigidBody.position.y >= aboveGroundY)
            {
                SetAIEnabled(true);
                return true;
            }
            return false;
        }

        void SetAIEnabled(bool value)
        {
            navMeshAgent.enabled = value;
            fighter.enabled = value;
            mover.enabled = value;
            aiController.enabled = value;
        }
    }
}
