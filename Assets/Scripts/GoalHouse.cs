using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class GoalHouse : MonoBehaviour
    {
        public SpriteAnimation animator;
        public SpriteRenderer overlay;
        public Transform arrows;
        public GameObject collision;
        public Particle glassParticlePrefab;
        private List<Particle> glassParticles;

        public bool m_isGoalHouse;

        private void Awake()
        {
            glassParticles = new List<Particle>();
            animator.SetFrame("animations", 0);
            ResetHouse();
        }

        public void ResetHouse()
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
            animator.SetFrame("animations", 0);

            for (var i = 0; i < glassParticles.Count; i++)
            {
                Destroy(glassParticles[i]);
            }
        }

        public bool IsGoalHouse()
        {
            return m_isGoalHouse;
        }

        private void Update()
        {
            if (m_isGoalHouse)
            {
                overlay.color = new Color(1, 1, 1, Mathf.PingPong(.5f, Time.time+1));
                arrows.localPosition = new Vector3(0, Mathf.PingPong(.16f, Time.time+1));
            }
        }

        public void OnBreak()
        {
            for (var i = 0; i < 30; i++)
            {
                var randomOffset = new Vector3(Random.Range(-2f, 2f), Random.Range(-1f, 1f));
                var pos = transform.position + randomOffset;
                var glass = Instantiate(glassParticlePrefab, pos, Quaternion.identity);
                glass.SetDirection( Vector2.down );
                glass.transform.SetParent(transform);
                glassParticles.Add(glass);
            }

            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/brokenGlass");
            // server papers!
            animator.SetFrame("animations", 1);
            ResetHouse();
        }
    }
}