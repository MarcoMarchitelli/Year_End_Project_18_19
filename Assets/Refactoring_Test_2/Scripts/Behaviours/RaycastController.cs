using UnityEngine;
using System.Collections;

namespace Refactoring
{
    [RequireComponent(typeof(BoxColliderEntity))]
    public abstract class RaycastController : BaseBehaviour
    {
        BoxColliderEntityData data;

        public LayerMask collisionMask;

        public const float skinWidth = .015f;
        const float dstBetweenRays = .15f;
        [HideInInspector]
        public int horizontalRayCount;
        [HideInInspector]
        public int verticalRayCount;

        [HideInInspector]
        public float horizontalRaySpacing;
        [HideInInspector]
        public float verticalRaySpacing;

        public RaycastOrigins raycastOrigins;

        protected override void CustomSetup()
        {
            data = Entity.Data as BoxColliderEntityData;
            CalculateRaySpacing();
        }

        public void UpdateRaycastOrigins()
        {
            Bounds bounds = data.collider.bounds;
            bounds.Expand(skinWidth * -2);

            raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
            raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
            raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
            raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
        }

        public void CalculateRaySpacing()
        {
            Bounds bounds = data.collider.bounds;
            bounds.Expand(skinWidth * -2);

            float boundsWidth = bounds.size.x;
            float boundsHeight = bounds.size.y;

            horizontalRayCount = Mathf.RoundToInt(boundsHeight / dstBetweenRays);
            verticalRayCount = Mathf.RoundToInt(boundsWidth / dstBetweenRays);

            horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
            verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
        }

        public struct RaycastOrigins
        {
            public Vector2 topLeft, topRight;
            public Vector2 bottomLeft, bottomRight;
        }
    } 
}