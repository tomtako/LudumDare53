using System;
using UnityEngine;

namespace DefaultNamespace
{
    [RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
    public class CarController : MonoBehaviour
    {
        [Range(0,1)]
        public float driftFactor = .5f;
        public float maxSpeed = 12;
        public float maxBackupSpeed = 6;
        public float acceleration = 30f;
        public float turnSpeed = 3.5f;
        public float minSpeedToTurnFactor = 8f;
        public float maxDrag = 3;
        public float dragForceWhenNotPressingInput = 3;
        public float lateralVelocityNeededToScreech = 1f;
        public float brakeForce;

        private float m_accelerationInput;
        private float m_steeringInput;

        private float m_rotationAngle;

        private bool m_isBreaking;
        private float m_velocityVsUp;

        private Rigidbody2D m_rigidBody;

        public Rigidbody2D rb => m_rigidBody;

        private void Awake()
        {
            m_rigidBody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {

            ApplyEngineForce();
            UpdateOrthogonalVelocity();
            ApplySteering();
        }

        private void ApplyEngineForce()
        {
            m_velocityVsUp = Vector2.Dot(transform.up, m_rigidBody.velocity);

            if (m_velocityVsUp > maxSpeed && m_accelerationInput > 0)
            {
                return;
            }
            //
            // if (m_velocityVsUp > -maxBackupSpeed && m_accelerationInput < 0)
            // {
            //     return;
            // }
            //
            // if (m_rigidBody.velocity.sqrMagnitude > maxSpeed * maxSpeed && m_accelerationInput > 0)
            // {
            //     return;
            // }

            if (m_accelerationInput == 0)
            {
                m_rigidBody.drag = Mathf.Lerp(m_rigidBody.drag, maxDrag,
                    Time.fixedDeltaTime * dragForceWhenNotPressingInput);
            }
            else
            {
                m_rigidBody.drag = 0;
            }

            Vector2 force = transform.up * m_accelerationInput * acceleration;

            if (m_isBreaking)
            {
                force *= brakeForce;
            }

            m_rigidBody.AddForce( force, ForceMode2D.Force);
        }

        private void ApplySteering()
        {
            float minSpeedBeforeAllowTurning = m_rigidBody.velocity.magnitude / minSpeedToTurnFactor;
            minSpeedBeforeAllowTurning = Mathf.Clamp01(minSpeedBeforeAllowTurning);

            m_rotationAngle -= m_steeringInput * turnSpeed * minSpeedBeforeAllowTurning;
            m_rigidBody.MoveRotation( m_rotationAngle );
        }

        private void UpdateOrthogonalVelocity()
        {
            Vector2 forwardVelocity = transform.up * Vector2.Dot(m_rigidBody.velocity, transform.up);
            Vector2 rightVelocity = transform.right * Vector2.Dot(m_rigidBody.velocity, transform.right);
            m_rigidBody.velocity = forwardVelocity + rightVelocity * driftFactor;
        }

        private float GetLateralVelocity()
        {
            return Vector2.Dot(transform.right, m_rigidBody.velocity);
        }

        public bool IsTireScreeching( out float lateralVelocity, out bool isBreaking )
        {
            lateralVelocity = GetLateralVelocity();
            isBreaking = false;

            if (m_accelerationInput < 0 && m_velocityVsUp > 0 || m_isBreaking)
            {
                isBreaking = true;
                return true;
            }

            if (Mathf.Abs(lateralVelocity) > lateralVelocityNeededToScreech)
            {
                return true;
            }

            return false;
        }

        public bool IsMoving()
        {
            return m_rigidBody.velocity.sqrMagnitude > 0;
        }

        public void SetInputVector(Vector2 inputVector)
        {
            m_steeringInput = inputVector.x;
            m_accelerationInput = inputVector.y;
        }

        public void Brake(bool shouldBreak )
        {
            m_isBreaking = shouldBreak;

            if (m_isBreaking)
            {
                if (IsMoving() && !IsMovingBackwards())
                {
                    m_accelerationInput = -1;
                }
                else
                {
                    m_accelerationInput = 0;
                }
            }
        }

        public bool IsMovingBackwards()
        {
            return Vector3.Angle(transform.up, rb.velocity) > 90f;
        }
    }
}