using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class GoalHouse : MonoBehaviour
    {
        public SpriteAnimation animator;
        public SpriteRenderer overlay;
        public Transform arrows;
        public GameObject collision;

        public bool m_isGoalHouse;
        private float counter = 0;

        private void Awake()
        {
            animator.SetFrame("animations", 0);
            ResetHouse();
        }

        private void ResetHouse()
        {
            m_isGoalHouse = false;
            arrows.SetActive(false);
            overlay.color = new Color(1, 1, 1, 0);
            collision.SetActive(true);
        }

        public void SetAsGoalHouse()
        {
            m_isGoalHouse = true;
            arrows.SetActive(true);
            collision.SetActive(false);
        }

        public bool IsGoalHouse()
        {
            return m_isGoalHouse;
        }

        private void Update()
        {
            if (m_isGoalHouse)
            {
                counter += Time.deltaTime * 5f;
                overlay.color = new Color(1, 1, 1, Mathf.PingPong(.5f, Time.time));
                arrows.localPosition = new Vector3(0, Mathf.PingPong(.16f, Time.time));
            }
        }

        public void OnBreak()
        {
            // server papers!
            animator.SetFrame("animations", 1);
            ResetHouse();
        }
    }
}