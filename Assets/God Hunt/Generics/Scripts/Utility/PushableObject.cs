using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
    public class PushableObject : MonoBehaviour
    {

        public float gravity = 12;
        PlayerCollisionsBehaviour controller;
        Vector3 velocity;

        void Start()
        {
            controller = GetComponent<PlayerCollisionsBehaviour>();
        }

        void Update()
        {
            velocity += Vector3.down * gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime, false);

            if (controller.Below)
            {
                velocity = Vector3.zero;
            }
        }

        public Vector2 Push(Vector2 amount)
        {
            return controller.Move(amount, false);
        }
    } 
}