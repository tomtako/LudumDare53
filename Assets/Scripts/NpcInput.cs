using System;
using KennethDevelops.ProLibrary.DataStructures.Pool;
using KennethDevelops.ProLibrary.Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class NpcInput : MonoBehaviour, IPoolObject
    {
        // moves to the assigned node.
        // the node tell them if they can move past it to the "next" node.
        // the
        public float moveSpeed;
        public float distanceToCheckForCars=32f;
        public SpriteAnimation animator;
        public BoxCollider2D boxCollider;
        public float jumpHeight = 1;
        public float tossMultiplier = 2;
        public float gravity = -10f;
        public GameObject damageShadow;

        private TrafficSystem m_system;
        private int m_currentNodeIndex;
        private TrafficNode m_currentNode;
        private PoolManager m_pool;
        private bool m_canUpdate;
        private bool m_isMoving;
        private float m_jumpVelocity;

        private bool m_damaged;
        private Vector2 m_hitDirection;
        private float m_hitSpeed;

        public void SetSystem(TrafficSystem system, int currentNodeIndex = 1)
        {
            m_system = system;
            m_currentNodeIndex = currentNodeIndex;
            m_currentNode = m_system.GetNode(m_currentNodeIndex);

            moveSpeed = Random.Range(3, 5);
            distanceToCheckForCars = Random.Range(1.5f, 3f);

            if (m_system.direction == TrafficSystem.RoadDirection.Left)
            {
                animator.Play("horizontal");
                animator.renderer.flipX = false;
            }

            if (m_system.direction == TrafficSystem.RoadDirection.Right)
            {
                animator.Play("horizontal");
                animator.renderer.flipX = true;
            }

            if (m_system.direction == TrafficSystem.RoadDirection.Up)
            {
                animator.Play("up");
                animator.renderer.flipX = false;
            }

            if (m_system.direction == TrafficSystem.RoadDirection.Down)
            {
                animator.Play("down");
                animator.renderer.flipX = false;
            }
        }

        private void Update()
        {
            if (GameManager.Instance.GetCurrentState() != GameManager.GameState.Gameplay)
            {
                return;
            }

            if (Damaged())
            {
                m_isMoving = false;
                transform.position += (Vector3)m_hitDirection * m_hitSpeed * Time.deltaTime;

                m_jumpVelocity += gravity * Time.deltaTime;
                animator.transform.localPosition += Vector3.up * m_jumpVelocity * Time.deltaTime;

                if (animator.transform.localPosition.y <= 0)
                {
                    animator.transform.localPosition = Vector3.zero;
                    m_pool.Dispose(this);
                }

                return;
            }

            if (!m_canUpdate)
            {
                m_isMoving = false;
                return;
            }

            if (Blocked())
            {
                m_isMoving = false;
                return;
            }

            if (m_currentNode != null)
            {
                m_isMoving = true;
                transform.position = Vector3.MoveTowards(transform.position, m_currentNode.transform.position, moveSpeed * Time.deltaTime);

                if (Vector3.Distance(transform.position, m_currentNode.transform.position) <= 0.1f)
                {
                    m_isMoving = false;

                    if (m_currentNode.nodeType == TrafficNode.NodeType.Despawn)
                    {
                        m_pool.Dispose(this);
                    }
                    else if (m_currentNode.nodeType == TrafficNode.NodeType.Flow && m_currentNode.IsFlowing())
                    {
                        m_currentNodeIndex++;
                        m_currentNode = m_system.GetNode(m_currentNodeIndex);
                    }
                }
            }
        }

        private bool Blocked()
        {
            if (m_currentNode == null)
            {
                return true;
            }

            var direction = (m_currentNode.transform.position - transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distanceToCheckForCars);

            if (hit)
            {
                return true;
            }

            return false;
        }

        public bool Damaged()
        {
            return m_damaged;
        }

        public void OnAcquire(PoolManager poolManager)
        {
            m_pool = poolManager;
            m_canUpdate = true;
            animator.renderer.sortingLayerName = "Default";
            boxCollider.enabled = true;
            damageShadow.SetActive(false);
        }

        public void OnDispose()
        {
            m_isMoving = false;
            m_canUpdate = false;
            m_damaged = false;
            gameObject.SetActive(false);
            damageShadow.SetActive(false);
        }

        public bool IsMoving()
        {
            return m_isMoving;
        }

        public void OnHit(CarController controller)
        {
            m_damaged = true;
            boxCollider.enabled = false;
            m_hitDirection = controller.rb.velocity.normalized;
            m_hitSpeed = tossMultiplier;
            m_jumpVelocity = jumpHeight;
            animator.Play("damaged");
            animator.renderer.sortingLayerName = "Effects";
            damageShadow.SetActive(true);
        }
    }
}