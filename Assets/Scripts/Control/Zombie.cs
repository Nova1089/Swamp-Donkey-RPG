using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Control
{
    public class Zombie : MonoBehaviour
    {
        // configs
        [SerializeField] float metersToLower = 2.5f;
        [SerializeField] float popUpSpeed = .1f;

        // state
        MeshRenderer meshRenderer;
        Rigidbody rigidBody;
        AIController aiController;
        ActionScheduler actionScheduler;
        NavMeshAgent navMeshAgent;
        Fighter fighter;
        Mover mover;

        void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            rigidBody = GetComponent<Rigidbody>();
            aiController = GetComponent<AIController>();
            actionScheduler = GetComponent<ActionScheduler>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            fighter = GetComponent<Fighter>();
            mover = GetComponent<Mover>();
        }

        public IEnumerator Respawn()
        {
            actionScheduler.CancelCurrentAction();
            meshRenderer.enabled = false;
            yield return new WaitUntil(aiController.PrepareForRespawn);
            // Vector3 currentPos = rigidBody.position;         
            // DropUnderGround();
            // yield return new WaitUntil(PopUp);
        }

        private void DropUnderGround()
        {
            navMeshAgent.enabled = false;            
            fighter.enabled = false;
            mover.enabled = false;
            Vector3 currentPos = rigidBody.position;
            Vector3 undergroundPos = new Vector3(
                currentPos.x,
                currentPos.y - metersToLower,
                currentPos.z);
            transform.position = undergroundPos;
        }

        bool PopUp()
        {
            Vector3 direction = new Vector3(
            rigidBody.position.x,
            rigidBody.position.y + 1 * popUpSpeed * Time.deltaTime,
            rigidBody.position.z);
            rigidBody.MovePosition(direction);

            if (rigidBody.position.y >= 0)
            {
                mover.enabled = true;                                
                fighter.enabled = true;
                navMeshAgent.enabled = true;
                return true;
            }
            return false;
        }
    }
}
