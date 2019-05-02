using System.Collections;
using UnityEngine;

namespace Refactoring
{
    public class FadingPlatformBehaviour : RaycastController
    {
        [SerializeField] bool above;
        [SerializeField] bool below;
        [SerializeField] bool right;
        [SerializeField] bool left;
        [SerializeField] float fadeEffectDuration;
        [SerializeField] float fadeDuration;

        [SerializeField] UnityVoidEvent OnTargetCollision;
        [SerializeField] UnityVoidEvent OnFadeEffectComplete;
        [SerializeField] UnityVoidEvent OnFadeEnd;

        MeshRenderer[] meshesToFade;
        Material[] meshesOldMaterials; 
        bool fadeing;

        public CollisionInfo collisions;

        protected override void CustomSetup()
        {
            base.CustomSetup();
            data = Entity.Data as PlatformEntityData;
            OnTargetCollision.AddListener(StartFadeEffect);
            meshesToFade = Entity.gameObject.GetComponentsInChildren<MeshRenderer>();
            FillMeshesColors();
        }

        public override void OnUpdate()
        {
            UpdateRaycastOrigins();

            collisions.Reset();

            if(!fadeing)
                CheckCollisions();
        }

        void CheckCollisions()
        {
            float rayLength = skinWidth * 2;

            if (above)
            {
                for (int i = 0; i < verticalRayCount; i++)
                {

                    Vector2 rayOrigin = raycastOrigins.topLeft;
                    rayOrigin += Vector2.right * (verticalRaySpacing * i);
                    RaycastHit hit;

                    Debug.DrawRay(rayOrigin, Vector2.up, Color.red);

                    if (Physics.Raycast(rayOrigin, Vector2.up, out hit, rayLength, collisionMask))
                    {
                        collisions.above = true;
                        OnTargetCollision.Invoke();
                        break;
                    }
                }
            }

            if (below)
            {
                for (int i = 0; i < verticalRayCount; i++)
                {

                    Vector2 rayOrigin = raycastOrigins.bottomLeft;
                    rayOrigin += Vector2.right * (verticalRaySpacing * i);
                    RaycastHit hit;

                    Debug.DrawRay(rayOrigin, Vector2.down, Color.red);

                    if (Physics.Raycast(rayOrigin, Vector2.down, out hit, rayLength, collisionMask))
                    {
                        collisions.below = true;
                        OnTargetCollision.Invoke();
                        break;
                    }
                }
            }

            if (right)
            {
                for (int i = 0; i < horizontalRayCount; i++)
                {

                    Vector2 rayOrigin = raycastOrigins.bottomRight;
                    rayOrigin += Vector2.up * (horizontalRaySpacing * i);
                    RaycastHit hit;

                    Debug.DrawRay(rayOrigin, Vector2.right, Color.red);

                    if (Physics.Raycast(rayOrigin, Vector2.right, out hit, rayLength, collisionMask))
                    {
                        collisions.right = true;
                        OnTargetCollision.Invoke();
                        break;
                    }
                }
            }

            if (left)
            {
                for (int i = 0; i < horizontalRayCount; i++)
                {

                    Vector2 rayOrigin = raycastOrigins.bottomRight;
                    rayOrigin += Vector2.up * (horizontalRaySpacing * i);
                    RaycastHit hit;

                    Debug.DrawRay(rayOrigin, Vector2.left, Color.red);

                    if (Physics.Raycast(rayOrigin, Vector2.left, out hit, rayLength, collisionMask))
                    {
                        collisions.left = true;
                        OnTargetCollision.Invoke();
                        break;
                    }
                }
            }
        }

        void StartFadeEffect()
        {
            StartCoroutine(FadeEffect());
        }

        IEnumerator FadeEffect()
        {
            float timer = 0;
            float fadePercent = 0;
            fadeing = true;

            while (timer < fadeEffectDuration)
            {
                timer += Time.deltaTime;
                fadePercent = timer / fadeEffectDuration;

                for (int i = 0; i < meshesToFade.Length; i++)
                {
                    meshesToFade[i].material.SetColor("_BaseColor", new Color(meshesToFade[i].material.color.r, meshesToFade[i].material.color.g, meshesToFade[i].material.color.b, Mathf.Lerp(1, 0, fadePercent)));
                }

                yield return null;
            }

            OnFadeEffectComplete.Invoke();
            data.collider.enabled = false;           

            yield return new WaitForSeconds(fadeDuration);

            OnFadeEnd.Invoke();
            data.collider.enabled = true;
            for (int i = 0; i < meshesToFade.Length; i++)
            {
                meshesToFade[i].material.SetColor("_BaseColor", new Color(meshesToFade[i].material.color.r, meshesToFade[i].material.color.g, meshesToFade[i].material.color.b, 1));
            }
            fadeing = false;
        }

        void FillMeshesColors()
        {
            meshesOldMaterials = new Material[meshesToFade.Length];

            for (int i = 0; i < meshesToFade.Length; i++)
            {
                meshesOldMaterials[i] = meshesToFade[i].material;
            }
        }

        public struct CollisionInfo
        {
            public bool above, below;
            public bool left, right;

            public void Reset()
            {
                above = below = false;
                left = right = false;
            }
        }
    }
}