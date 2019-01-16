using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastEnemy : RaycastController
{
    /// <summary>
    /// I layer inseriti qui verranno spinti dal nemico
    /// </summary>
    [Tooltip("I layer inseriti qui verranno spinti dal nemico")]
    public LayerMask pushMask;

    private List<PassengerMovement> passengerMovementList;
    private Dictionary<Transform, EntityBaseController> passengerDictionary = new Dictionary<Transform, EntityBaseController>();

    private struct PassengerMovement
    {
        public Transform transform;
        public Vector3 velocity;
        public bool isMovingBeforePlatform;

        public PassengerMovement(Transform _transform, Vector3 _velocity, bool _isMovingBeforePlatform)
        {
            transform = _transform;
            velocity = _velocity;
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
                passengerDictionary[passenger.transform].Move(passenger.velocity);
            }
        }
    }

    public void DamagePlayer(int damageToDo)
    {
        HashSet<Transform> passengersToHit = new HashSet<Transform>();

        for (int i = 0; i < HorizontalRayCount; i++)
        {
            Vector2 rayOrigin = myRaycastOrigins.BottomLeft;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.left, leftRayLength, attackMask);

            Debug.DrawRay(rayOrigin, Vector2.left * leftRayLength, Color.red);

            if (hit) // Mentre colpisco qualcosa
            {
                Collisions.left = true;

                if (!passengersToHit.Contains(hit.transform))
                {
                    passengersToHit.Add(hit.transform);
                    if (hit.collider.GetComponent<PlayerController>() != null)
                    {
                        hit.collider.GetComponent<PlayerController>().TakeDamage(damageToDo); // Viene chiamato ad ogni Update, fixare in caso di lag
                    }
                }
            }
        }

        for (int i = 0; i < HorizontalRayCount; i++)
        {
            Vector2 rayOrigin = myRaycastOrigins.BottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right, rightRayLength, attackMask);

            Debug.DrawRay(rayOrigin, Vector2.right * rightRayLength, Color.red);

            if (hit) // Mentre colpisco qualcosa
            {
                Collisions.right = true;

                if (!passengersToHit.Contains(hit.transform))
                {
                    passengersToHit.Add(hit.transform);
                    if (hit.collider.GetComponent<PlayerController>() != null)
                    {
                        hit.collider.GetComponent<PlayerController>().TakeDamage(damageToDo); // Viene chiamato ad ogni Update, fixare in caso di lag
                    }
                }
            }
        }

        for (int i = 0; i < VerticalRayCount; i++)
        {
            Vector2 rayOrigin = myRaycastOrigins.TopLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, topRayLength, attackMask);

            Debug.DrawRay(rayOrigin, Vector2.up * topRayLength, Color.red);

            if (hit) // Mentre colpisco qualcosa
            {
                Collisions.above = true;

                if (!passengersToHit.Contains(hit.transform))
                {
                    passengersToHit.Add(hit.transform);
                    if (hit.collider.GetComponent<PlayerController>() != null)
                    {
                        hit.collider.GetComponent<PlayerController>().TakeDamage(damageToDo); // Viene chiamato ad ogni Update, fixare in caso di lag
                    }
                }
            }
        }

        for (int i = 0; i < VerticalRayCount; i++)
        {
            Vector2 rayOrigin = myRaycastOrigins.BottomLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, bottomRayLength, attackMask);

            Debug.DrawRay(rayOrigin, Vector2.down * bottomRayLength, Color.red);

            if (hit) // Mentre colpisco qualcosa
            {
                Collisions.below = true;

                if (!passengersToHit.Contains(hit.transform))
                {
                    passengersToHit.Add(hit.transform);
                    if (hit.collider.GetComponent<PlayerController>() != null)
                    {
                        hit.collider.GetComponent<PlayerController>().TakeDamage(damageToDo); // Viene chiamato ad ogni Update, fixare in caso di lag
                    }
                }
            }
        }
    }

    public void CalculatePassengerMovement(Vector3 velocity)
    {
        HashSet<Transform> movedPassengers = new HashSet<Transform>();
        passengerMovementList = new List<PassengerMovement>();

        if (velocity.x > 0) // Enemy going right
        {
            for (int i = 0; i < HorizontalRayCount; i++)
            {
                Vector2 rayOrigin = myRaycastOrigins.BottomRight;
                rayOrigin += Vector2.up * (horizontalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right, rightRayLength, pushMask);

                Debug.DrawRay(rayOrigin, Vector2.right * rightRayLength, Color.red);

                if (hit)
                {
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);
                        float pushX = rightRayLength - (hit.distance - SkinWidth);
                        float pushY = 0;

                        passengerMovementList.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), true));
                    }
                }
            }
        }

        if (velocity.x < 0) // Enemy going left
        {
            for (int i = 0; i < HorizontalRayCount; i++)
            {
                Vector2 rayOrigin = myRaycastOrigins.BottomLeft;
                rayOrigin += Vector2.up * (horizontalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.left, leftRayLength, pushMask);

                Debug.DrawRay(rayOrigin, Vector2.left * leftRayLength, Color.red);

                if (hit)
                {
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);
                        float pushX = leftRayLength - (hit.distance - SkinWidth);
                        float pushY = 0;

                        passengerMovementList.Add(new PassengerMovement(hit.transform, new Vector3(-pushX, pushY), true));
                    }
                }
            }
        }
    }
}