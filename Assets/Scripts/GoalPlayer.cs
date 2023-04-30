using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class GoalPlayer : MonoBehaviour
    {
        public Transform reticle;
        public Transform shootNozzle;
        public Projectile projectilePrefab;
        public float projectileSpeed = 20;
        public float throwCooldown = 0.2f;

        public float minAngle = -45f;
        public float maxAngle = 45f;
        public float speed = 5f;

        private bool rotatingRight = true;
        private float currentAngle;
        private float m_cooldown;

        private void Start()
        {
            currentAngle = reticle.rotation.eulerAngles.z;
        }

        private void Update()
        {
            // Calculate the target angle based on the rotatingRight flag
            float targetAngle = rotatingRight ? maxAngle : minAngle;

            // Rotate towards the target angle
            currentAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, speed * Time.deltaTime);
            reticle.rotation = Quaternion.Euler(0f, 0f, currentAngle);

            // Check if we've reached the target angle
            if (Mathf.Approximately(currentAngle, targetAngle))
            {
                // If so, toggle the rotatingRight flag to change direction
                rotatingRight = !rotatingRight;
            }

            if (m_cooldown > 0)
            {
                m_cooldown -= Time.deltaTime;
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire1"))
                {
                    ShootProjectile();
                }
            }
        }


        private void ShootProjectile()
        {
            m_cooldown = throwCooldown;

            var projectile = Instantiate(projectilePrefab, shootNozzle.position, Quaternion.identity);
            projectile.Shoot(currentAngle, projectileSpeed);
        }

    }
}