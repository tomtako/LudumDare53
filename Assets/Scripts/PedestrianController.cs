using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class PedestrianController : MonoBehaviour
    {
        private ParticleSystem[] m_splats;
        private BoxCollider2D m_collider;
        private SpriteRenderer m_renderer;

        private void Awake()
        {
            m_collider = GetComponent<BoxCollider2D>();
            m_renderer = GetComponent<SpriteRenderer>();
            m_splats = GetComponentsInChildren<ParticleSystem>();
        }

        public void Hit()
        {
            m_collider.enabled=false;
            m_renderer.enabled = false;

            for (var i = 0; i < m_splats.Length; i++)
            {
                m_splats[i].Play(true);
            }
        }
    }
}