using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class PedestrianController : MonoBehaviour
    {
        public SpriteAnimation m_bloodAnimator;

        public bool flips = true;
        public float minFlipTime = 3;
        public float maxFlipTime = 10;

        private BoxCollider2D m_collider;
        private SpriteAnimation m_renderer;
        private float m_flipXTimer;

        private void Awake()
        {
            m_collider = GetComponent<BoxCollider2D>();
            m_renderer = GetComponent<SpriteAnimation>();
            m_renderer.SetFrame("anims", Random.Range(0, m_renderer.GetFrameCount()));
            m_flipXTimer = Random.Range(minFlipTime, maxFlipTime);
        }

        public void Hit()
        {
            m_collider.enabled=false;
            m_renderer.enabled = false;
            m_renderer.renderer.enabled = false;
            m_bloodAnimator.SetActive(true);
            m_bloodAnimator.SetFrame("anims", Random.Range(0, m_bloodAnimator.GetFrameCount()));
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/hitBlood");
        }

        private void Update()
        {
            if (!flips) return;

            m_flipXTimer -= Time.deltaTime;

            if (m_flipXTimer <= 0)
            {
                m_renderer.renderer.flipX = !m_renderer.renderer.flipX;
                m_flipXTimer = Random.Range(minFlipTime, maxFlipTime);
            }
        }
    }
}