using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RaycastEnemy))]
public class EnemyController : EntityBaseController
{ 
    [Header("Patrol")]
    /// <summary>
    /// Punti da cui passerà il nemico
    /// </summary>
    [Tooltip("Punti da cui passerà il nemico")]
    public Transform[] TravelPoints = new Transform[2];

    protected override void Start()
    {
        myRayCon = GetComponent<RaycastEnemy>();

        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        if (isAlive && !CanvasManager.isPaused)
        {
            (myRayCon as RaycastEnemy).DamagePlayer(AttackDamage);
            (myRayCon as RaycastEnemy).CalculatePassengerMovement(velocity);

            (myRayCon as RaycastEnemy).MovePassenger(true);
            Move(velocity * Time.deltaTime);
            (myRayCon as RaycastEnemy).MovePassenger(false);
        }

    }

    public void MoveLeft()
    {
        if (TravelPoints[0] == null)
        {
            velocity.x = 0;
            if (myRayCon.Collisions.below)
            {
                ResetJump();
                Jump();
            }
            return;
        }

        velocity.x = -MovementSpeed;

        if (transform.position.x <= TravelPoints[0].position.x)
        {
            MoveRight();
        }
    }

    public void MoveRight()
    {
        velocity.x = 0;
        if (TravelPoints[1] == null)
        {
            if (myRayCon.Collisions.below)
            {
                ResetJump();
                Jump();
            }
            return;
        }

        velocity.x = MovementSpeed;

        if (transform.position.x >= TravelPoints[1].position.x)
        {
            MoveLeft();
        }
    }
}
