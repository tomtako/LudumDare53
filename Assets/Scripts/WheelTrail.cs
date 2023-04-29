using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class WheelTrail : MonoBehaviour
    {
        private CarController m_controller;
        private TrailRenderer m_trailRenderer;

        private void Awake()
        {
            m_controller = GetComponentInParent<CarController>();
            m_trailRenderer = GetComponent<TrailRenderer>();

            m_trailRenderer.emitting = false;
        }

        private void Update()
        {
            if (m_controller.IsTireScreeching(out float lateralVelocity, out bool isBreaking))
            {
                m_trailRenderer.emitting = true;
            }
            else
            {
                m_trailRenderer.emitting = false;
            }
        }
    }
}