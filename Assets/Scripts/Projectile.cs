using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class Projectile : MonoBehaviour
    {
        private float m_speed;
        private Vector2 m_moveDirection;

        private void Update()
        {
            transform.position += (Vector3)m_moveDirection * m_speed * Time.deltaTime;
        }

        public void Shoot(float angle, float speed)
        {
            float angleInRadians = (angle+90) * Mathf.Deg2Rad;
            m_moveDirection = new Vector2(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians));
            m_speed = speed;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Wall"))
            {
                Destroy(gameObject);
            }
            else if (col.CompareTag("GoalEnemy"))
            {
                var enemy = col.GetComponentInParent<GoalEnemy>();
                enemy.OnHit();
            }
        }
    }
}