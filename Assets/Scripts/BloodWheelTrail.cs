using UnityEngine;

namespace DefaultNamespace
{
    public class BloodWheelTrail : MonoBehaviour
    {
        private PlayerInput m_playerInput;
        private CarController m_controller;
        private TrailRenderer m_trailRenderer;

        private void Awake()
        {
            m_playerInput = GetComponentInParent<PlayerInput>();
            m_controller = GetComponentInParent<CarController>();
            m_trailRenderer = GetComponent<TrailRenderer>();

            m_trailRenderer.emitting = false;
        }

        private void Update()
        {
            if (m_playerInput == null) return;

            m_trailRenderer.emitting = m_playerInput.DidHitPedestrian();
        }
    }
}