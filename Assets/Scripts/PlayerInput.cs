
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerInput : MonoBehaviour
    {
        public SpriteAnimation animator;
        public float bloodTrailTime = 3;
        public float speedUpFromPedestrians = 2;
        public float slowDownFromCars = 5f;
        public int carFrames = 12;
        public float angleCorrection = 180;

        private CarController m_controller;
        private Quaternion initialRotation;
        private float m_hitPedestrianTimer;

        private void Awake()
        {
            m_controller = GetComponent<CarController>();
            initialRotation = animator.transform.rotation;
        }

        private void Update()
        {
            if (GameManager.Instance.GetCurrentState() != GameManager.GameState.Gameplay)
            {
                m_controller.rb.velocity = Vector2.zero;
                return;
            }

            Vector2 input =Vector2.zero;

            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            m_controller.SetInputVector(input);
            m_controller.Brake(Input.GetButton("Fire1") || Input.GetKeyDown(KeyCode.Space));

            UpdateAnimations();

            if (m_hitPedestrianTimer > 0)
            {
                m_hitPedestrianTimer -= Time.deltaTime;
            }
        }

        private void LateUpdate()
        {
            animator.transform.rotation = initialRotation;
        }


        private void UpdateAnimations()
        {
            if (m_controller.rb.velocity.sqrMagnitude <= 0)
            {
                return;
            }

            bool movingBackwards = Vector3.Angle(transform.up, m_controller.rb.velocity) > 90f;

            Vector2 direction = m_controller.rb.velocity.normalized;

            if (direction.x > 0)
            {
                direction.y *= -1;
            }

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            angle += angleCorrection;

            //var angle0 = angle;

            if (angle < 0f)
            {
                angle += 360f;
            }

            angle *= -1f;

            if (angle < 0f)
            {
                angle += 360f;
            }

            var halfAngle = angle > 180f ?  angle - 180f : angle;
            var animIndex = Mathf.RoundToInt(halfAngle / (180f / carFrames) );

            if (animIndex > carFrames)
            {
                animIndex = carFrames;
            }

            if (!movingBackwards)
            {
                animator.SetFrame("move", animIndex);
                animator.renderer.flipX = direction.x < 0;
            }
            else
            {
                animator.SetFrame("move", carFrames - animIndex);
                animator.renderer.flipX = direction.x > 0;
            }
        }

        public bool DidHitPedestrian()
        {
            return m_hitPedestrianTimer > 0;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (GameManager.Instance.GetCurrentState() != GameManager.GameState.Gameplay)
            {
                return;
            }

            if (col.CompareTag("Pedestrian"))
            {
                GameManager.Instance.OnHitPedestrian();

                CameraFollower.Instance.Shake();
                m_hitPedestrianTimer = bloodTrailTime;
                var pedestrian = col.gameObject.GetComponent<PedestrianController>();
                pedestrian.Hit();

                var direction = m_controller.rb.velocity.normalized;
                m_controller.rb.AddForce(direction.normalized * speedUpFromPedestrians, ForceMode2D.Impulse);
            }

            if (col.CompareTag("Car"))
            {
                CameraFollower.Instance.Shake();
                var car = col.gameObject.GetComponent<NpcInput>();

                if (!car.Damaged())
                {
                    GameManager.Instance.OnHitCar();

                    car.OnHit(m_controller);

                    //m_controller.RigidBody.velocity = Vector2.zero;

                    var direction = -m_controller.rb.velocity.normalized;
                    m_controller.rb.AddForce(direction.normalized * slowDownFromCars, ForceMode2D.Impulse);
                }

            }
        }
    }
}