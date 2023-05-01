
using UnityEngine;

namespace DefaultNamespace
{
    public class Particle : MonoBehaviour
    {
        public float gravity = -10;
        public float initialVelocity = 10;
        public float initialMomentum=20;
        public float drag = 2;
        public float initialHeight = 0.25f;
        public SpriteAnimation animator;

        private Vector2 m_direction;
        private float m_velocity;
        private float m_momentum;

        public void SetDirection(Vector2 direction)
        {
            animator.SetFrame("anim", Random.Range(0, animator.GetFrameCount()));
            m_direction = direction;
            m_velocity = initialVelocity;
            m_momentum = initialMomentum;
            animator.transform.localPosition = new Vector3(0, initialHeight, 0);
        }

        private void Update()
        {
            if (Vector2.Distance(animator.transform.localPosition, Vector2.zero) > 0.01f)
            {
                m_velocity += gravity * Time.deltaTime;
                animator.transform.localPosition += Vector3.up * m_velocity * Time.deltaTime;

                if (Vector2.Distance(animator.transform.localPosition, Vector2.zero) <= 0.01f)
                {
                    animator.transform.localPosition = Vector2.zero;
                }
            }

            if (m_momentum > 0)
            {
                m_momentum -= drag * Time.deltaTime;
                transform.position += (Vector3)m_direction * m_momentum * Time.deltaTime;
            }
        }
    }
}