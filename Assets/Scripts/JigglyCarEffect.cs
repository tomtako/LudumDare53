using System;
using FMOD;
using UnityEngine;

namespace DefaultNamespace
{
    public class JigglyCarEffect : MonoBehaviour
    {
        public int deformPixels = 2;
        public float scaleDelay = 0.08f;
        private float m_scaleTimer = 0.08f;
        private int scaleIndex;
        public const float PixelSize = 1 / 32f;

        private CarController m_controller;
        private NpcInput m_npcInput;
        private Vector3 m_initialScale;

        private void Awake()
        {
            m_scaleTimer = scaleDelay;
            m_controller = GetComponentInParent<CarController>();
            m_npcInput = GetComponentInParent<NpcInput>();
        }

        private bool IsMoving()
        {
            if (m_npcInput != null)
            {
                return m_npcInput.IsMoving();
            }

            if (m_controller != null)
            {
                return m_controller.IsMoving();
            }

            // always jigggggggggle!
            return true;
        }

        private void Update()
        {
            if (!IsMoving())
            {
                transform.localScale = Vector3.one;
                scaleIndex = 0;
                m_scaleTimer = scaleDelay;
                return;
            }

            m_scaleTimer -= Time.deltaTime;

            if (m_scaleTimer <= 0)
            {
                m_scaleTimer = scaleDelay;

                if (scaleIndex == 0)
                {
                    transform.localScale = new Vector3(1-PixelSize*deformPixels, 1+PixelSize*deformPixels, 1);
                }
                else
                {
                    transform.localScale = Vector3.one;
                }

                scaleIndex = scaleIndex > 0 ? 0 : 1;
            }
        }
    }
}