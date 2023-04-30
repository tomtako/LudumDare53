
using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class GoalEnemy : MonoBehaviour
    {
        public Camera cam;
        public Transform character;
        public SpriteRenderer spriteRenderer;
        public Transform shadow;
        public Transform node1;
        public Transform node2;
        public float speed = 5f;
        public float jumpVelocity = 4;
        public float gravity = -20;
        public float stretchAmount = 0.032f;
        public float squashAmount = 0.032f;
        public float minShadowScale = 0.8f;
        public float landDuration = 0.24f;
        public float deathTimescale=0.5f;
        public float deathCameraMoveSpeed = 100;
        public float deathCameraZoomSpeed = 5;
        public float deathSpinSpeed = 90;
        public float cameraZoomSize;

        public float minimumTimeBeforeAllowedToTurnAround = 2;
        public float maximumTimeBeforeAllowedToTurnAround = 6;

        public float minimumTimeBeforeAllowedToJump = 0.5f;
        public float maximumTimeBeforeAllowedToJump = 3f;

        private Rigidbody2D rb;
        private Vector2 currentVelocity;
        private bool movingRight = true;
        private float turnAroundDelay;

        private bool m_landing;
        private bool m_animateLand;
        private float m_velocity;
        private float m_landingTimer;

        private float m_jumpDelay;
        private Vector3 m_originalShadowScale;

        private bool m_death;
        private float m_originalCameraSize;
        private bool m_zoomCameraIn;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            turnAroundDelay = Random.Range(minimumTimeBeforeAllowedToTurnAround, maximumTimeBeforeAllowedToTurnAround);
            m_jumpDelay = Random.Range(minimumTimeBeforeAllowedToJump, maximumTimeBeforeAllowedToJump);
            movingRight = Random.value >0.5f;
            m_originalShadowScale = shadow.localScale;
            m_originalCameraSize = cam.orthographicSize;
        }

        public void OnHit()
        {
            m_death = true;
            m_zoomCameraIn = true;
            Time.timeScale = deathTimescale;
            Jump();

            //Time.timeScale = 0.1f;
        }

        private void LandOnDeath()
        {
            character.DORotate(new Vector3(0,0,90), 1).SetEase(Ease.OutBack);
            character.localPosition = new Vector3(.3f, 0);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                OnHit();
            }

            spriteRenderer.flipX = movingRight;

            float distance = Mathf.Clamp(character.position.y - transform.position.y, 0, 2);

            // Map the distance to a scale factor between minScale and maxScale
            float scaleFactor = Mathf.Lerp(minShadowScale, 1, 1f - distance / 2);

            // Scale the shadow based on the scaleFactor
            shadow.localScale = m_originalShadowScale * scaleFactor;

            if (m_death)
            {
                if (character.localPosition.y > 0)
                {
                    m_velocity += gravity * Time.deltaTime;
                    character.localPosition += Vector3.up * m_velocity * Time.deltaTime;
                    var x = Mathf.MoveTowards(character.localPosition.x, .3f, 10 * Time.deltaTime);
                    character.localPosition = new Vector3(x, character.localPosition.y, character.localPosition.z);
                    character.localEulerAngles += Vector3.forward * Time.deltaTime  * deathSpinSpeed;
                    shadow.localPosition = Vector3.MoveTowards(shadow.localPosition, new Vector3(0, -0.2f, 0),
                        10 * Time.deltaTime);

                    if (character.localPosition.y <= 0)
                    {
                        LandOnDeath();
                    }
                }

                if (m_zoomCameraIn)
                {
                    cam.orthographicSize = Mathf.MoveTowards(cam.orthographicSize, cameraZoomSize,
                        deathCameraZoomSpeed * Time.deltaTime);

                    cam.transform.position = Vector3.MoveTowards(cam.transform.position, new Vector3(transform.position.x,transform.position.y, -10),
                        deathCameraMoveSpeed * Time.deltaTime);

                    if (cam.orthographicSize <= cameraZoomSize)
                    {
                        m_zoomCameraIn = false;
                    }
                }
                else
                {
                    Time.timeScale = Mathf.MoveTowards(Time.timeScale, 1, Time.deltaTime * 10f);

                    cam.orthographicSize = Mathf.MoveTowards(cam.orthographicSize, m_originalCameraSize,
                        deathCameraZoomSpeed * Time.deltaTime);

                    cam.transform.position = Vector3.MoveTowards(cam.transform.position, new Vector3(0,0,-10),
                        deathCameraMoveSpeed * Time.deltaTime);

                    if (cam.orthographicSize >= m_originalCameraSize)
                    {
                        m_death = false;
                        enabled = false;

                        Debug.Log("YOU'VE BEEN SERVED!!");

                        //Destroy(gameObject);

                        // handle changing state!
                    }
                }

                return;
            }



            if (character.localPosition.y > 0)
            {
                m_velocity += gravity * Time.deltaTime;
                character.localPosition += Vector3.up * m_velocity * Time.deltaTime;

                if (character.localPosition.y <= 0)
                {
                    character.localPosition = Vector3.zero;
                    Land();
                }
            }

            if (m_landing)
            {
                AnimateLand();
            }
        }

        private void AnimateLand()
        {
            if (!m_animateLand)
            {
                m_landingTimer -= Time.deltaTime;
                if (m_landingTimer <= 0)
                {
                    m_landing = false;
                }

                return;
            }

            if (m_landing && m_animateLand)
            {
                character.localScale = new Vector3(1 + squashAmount, 1 - squashAmount, 1);
                character.DOScale(1, landDuration).SetEase(Ease.OutBack);
                m_animateLand = false;
                m_landingTimer = landDuration;
            }
        }

        private void Land()
        {
            m_animateLand = true;
            m_landing = true;
        }

        private void Jump()
        {
            Debug.Log("Jump called");
            m_velocity = jumpVelocity;
            character.localPosition = new Vector3(0, 0.01f, 0);
            character.localScale = new Vector3(1 - stretchAmount, 1 + stretchAmount, 1);
            character.DOScale(1, landDuration * 2f).SetEase(Ease.OutBack);
            m_jumpDelay = Random.Range(minimumTimeBeforeAllowedToJump, maximumTimeBeforeAllowedToJump);
        }

        private void FixedUpdate()
        {
            if (m_death)
            {
                return;
            }

            if (m_landing)
            {
                return;
            }

            // Calculate the current direction based on the movingRight flag
            Vector2 direction = movingRight ? (node2.position - transform.position).normalized : (node1.position - transform.position).normalized;

            // Set the current velocity based on the direction and speed
            currentVelocity = direction * speed;

            // Move the rigidbody based on the velocity
            //rb.AddForce( direction * speed, ForceMode2D.Force);
            rb.MovePosition(rb.position + currentVelocity * Time.fixedDeltaTime);

            if (character.localPosition.y <= 0)
            {
                m_jumpDelay -= Time.deltaTime;

                if (m_jumpDelay <= 0)
                {
                    Jump();
                    return;
                }
            }



            turnAroundDelay -= Time.deltaTime;

            // Check if the transform has reached a node
            if ((movingRight && transform.position.x >= node2.position.x) ||
                (!movingRight && transform.position.x <= node1.position.x))
            {
                turnAroundDelay = Random.Range(minimumTimeBeforeAllowedToTurnAround, maximumTimeBeforeAllowedToTurnAround);
                // If so, toggle the movingRight flag to change direction
                movingRight = !movingRight;
            }
            else if (turnAroundDelay <= 0 && character.localPosition.y <= 0)
            {
                turnAroundDelay = Random.Range(minimumTimeBeforeAllowedToTurnAround, maximumTimeBeforeAllowedToTurnAround);
                movingRight = !movingRight;
            }
        }
    }
}