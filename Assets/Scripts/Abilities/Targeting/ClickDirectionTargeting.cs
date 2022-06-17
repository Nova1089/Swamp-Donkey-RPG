using RPG.Control;
using System;
using System.Collections;
using UnityEngine;

namespace RPG.Abilities.Targeting
{
    [CreateAssetMenu(fileName = "Click Direction Targeting", menuName = "Abilities/Targeting/Click Direction", order = 0)]
    public class ClickDirectionTargeting : TargetingStrategy
    {
        // configs
        [SerializeField] Texture2D cursorTexture;
        [SerializeField] Vector2 cursorHotspot;
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private float groundOffset = 1;

        public override void StartTargeting(AbilityData data, Action finished)
        {
            PlayerController playerController = data.GetUser().GetComponent<PlayerController>();
            playerController.StartCoroutine(Targeting(data, playerController, finished));
        }

        private IEnumerator Targeting(AbilityData data, PlayerController playerController, Action finished)
        {
            playerController.enabled = false;

            while (!data.IsCancelled()) // Run every frame;
            {
                Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.Auto);
                RaycastHit raycastHit;
                Ray ray = PlayerController.GetMouseRay();
                if (Physics.Raycast(ray, out raycastHit, 1000, layerMask))
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        yield return new WaitWhile(() => Input.GetMouseButton(0)); // Don't proceed until mouse button has been released.
                        data.SetTargetedPoint(raycastHit.point + ray.direction * groundOffset / ray.direction.y);
                        break;
                    }
                }
                yield return null;
            }
            playerController.enabled = true;
            finished();
        }
    }
}