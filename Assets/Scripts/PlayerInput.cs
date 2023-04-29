using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerInput : MonoBehaviour
    {
        public SpriteAnimation animator;
        public float angleThreshold = 22.5f;

        private CarController m_controller;
        private Quaternion initialRotation;

        private void Awake()
        {
            m_controller = GetComponent<CarController>();
            initialRotation = animator.transform.rotation;
        }

        private void Update()
        {
            Vector2 input = new Vector2(0, 0);
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");
            m_controller.SetInputVector(input);
            //GetComponent<Rigidbody2D>().AddForce(input, ForceMode2D.Force);

            UpdateAnimations();
        }

        private void LateUpdate()
        {
            animator.transform.rotation = initialRotation;
        }

        private void UpdateAnimations()
        {
            // not moving.. can use the last frame.
            // if (m_controller.RigidBody.velocity.magnitude <= 0)
            // {
            //     return;
            // }

            Vector2 direction = m_controller.RigidBody.velocity.normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Adjust the angle based on the desired orientation
            angle += 90f;

            if (angle < 0f)
            {
                angle += 360f;
            }

            // Flip the angle horizontally to make 0 down and 180 up
            angle *= -1f;

            if (angle < 0f)
            {
                angle += 360f;
            }

            var halfAngle = angle > 180f ?  angle - 180f : angle;
            var animIndex = Mathf.RoundToInt(halfAngle / (180f / 11f) );
            animator.SetFrame("move", animIndex);
        }
    }
}