using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastPlatform : RaycastController
{
    public LayerMask passengerMask;

    private List<PassengerMovement> passengerMovementList;
    private Dictionary<Transform, EntityBaseController> passengerDictionary = new Dictionary<Transform, EntityBaseController>();

    private struct PassengerMovement
    {
        public Transform transform;
        public Vector3 velocity;
        public bool isStandingOnPlatform;
        public bool isMovingBeforePlatform;

        public PassengerMovement(Transform _transform, Vector3 _velocity, bool _isStandingOnPlatform, bool _isMovingBeforePlatform)
        {
            transform = _transform;
            velocity = _velocity;
            isStandingOnPlatform = _isStandingOnPlatform;
            isMovingBeforePlatform = _isMovingBeforePlatform;
        }
    }

    private void Update()
    {
        UpdateRaycastOrigins();
    }

    public void MovePassenger(bool isPassengerMovingBeforePlatform)
    {
        foreach (PassengerMovement passenger in passengerMovementList)
        {
            if (!passengerDictionary.ContainsKey(passenger.transform))
            {
                passengerDictionary.Add(passenger.transform, passenger.transform.GetComponent<EntityBaseController>());
            }

            if (passenger.isMovingBeforePlatform == isPassengerMovingBeforePlatform)
            {
                passengerDictionary[passenger.transform].Move(passenger.velocity, passenger.isStandingOnPlatform);
            }
        }
    }

    public void CalculatePassengerMovement(Vector3 velocity)
    {
        HashSet<Transform> movedPassengers = new HashSet<Transform>();
        passengerMovementList = new List<PassengerMovement>();

        float directionX = Mathf.Sign(velocity.x);
        float directionY = Mathf.Sign(velocity.y);

        if (velocity.y != 0) // Vertically moving platform
        {
            float rayLength = Mathf.Abs(velocity.y) + SkinWidth;

            for (int i = 0; i < VerticalRayCount; i++)
            {
                Vector2 rayOrigin = (directionY == -1) ? myRaycastOrigins.BottomLeft : myRaycastOrigins.TopLeft;
                rayOrigin += Vector2.right * (verticalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, directionY * Vector2.up, rayLength, passengerMask);

                if (hit)
                {
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);
                        float pushX = (directionY == 1) ? velocity.x : 0f;
                        float pushY = velocity.y - (hit.distance - SkinWidth) * directionY;

                        passengerMovementList.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), directionY == 1, true));
                    }
                }
            }
        }

        if (velocity.x != 0) // Horizontally moving platform
        {
            float rayLength = Mathf.Abs(velocity.x) + SkinWidth;

            for (int i = 0; i < HorizontalRayCount; i++)
            {
                Vector2 rayOrigin = (directionX == -1) ? myRaycastOrigins.BottomLeft : myRaycastOrigins.BottomRight;
                rayOrigin += Vector2.up * (horizontalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, directionX * Vector2.right, rayLength, passengerMask);

                if (hit)
                {
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);
                        float pushX = velocity.x - (hit.distance - SkinWidth) * directionX;
                        float pushY = 0;

                        passengerMovementList.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), true, true));
                    }
                }
            }
        }

        if (directionY == -1 || velocity.y == 0 && velocity.x != 0) // Passenger on top of an horizontally or downward moving platform
        {
            float rayLength = SkinWidth * 2;

            for (int i = 0; i < VerticalRayCount; i++)
            {
                Vector2 rayOrigin = myRaycastOrigins.TopLeft + Vector2.right * (verticalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLength, passengerMask);

                if (hit)
                {
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);
                        float pushX = velocity.x;
                        float pushY = velocity.y;

                        passengerMovementList.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), true, false));
                    }
                }
            }
        }
    }
}
