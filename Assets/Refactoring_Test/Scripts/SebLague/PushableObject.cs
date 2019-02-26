using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Refactoring
{
    public class PushableObject : MonoBehaviour
    {

        public float gravity = 12;
        Controller3D controller;
        Vector3 velocity;

        void Start()
        {
            controller = GetComponent<Controller3D>();
        }

        void Update()
        {
            velocity += Vector3.down * gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime, false);
            if (controller.collisions.below)
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