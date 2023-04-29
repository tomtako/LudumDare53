using UnityEngine;

namespace DefaultNamespace
{
    public class WheelParticle : MonoBehaviour
    {
        public float emissionRateOnTurn = 2;
        public float emissionRateOnBreak = 3;

        private CarController m_carController;
        private ParticleSystem m_particleSystem;
        private ParticleSystem.EmissionModule m_emissionModule;
        private float m_particleEmissionRate;

        private void Awake()
        {
            m_carController = GetComponentInParent<CarController>();
            m_particleSystem = GetComponent<ParticleSystem>();
            m_emissionModule = m_particleSystem.emission;
            m_emissionModule.rateOverTime = 0;
        }

        private void Update()
        {
            m_particleEmissionRate = Mathf.Lerp(m_particleEmissionRate, 0, Time.deltaTime * 5f);
            m_emissionModule.rateOverTime = m_particleEmissionRate;

            if (m_carController.IsTireScreeching(out float lateralVelocity, out bool isBreaking))
            {
                if (isBreaking)
                {
                    m_particleEmissionRate = emissionRateOnBreak;
                }
                else
                {
                    m_particleEmissionRate = Mathf.Abs(lateralVelocity) *emissionRateOnTurn;
                }
            }
        }
    }
}