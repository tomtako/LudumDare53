using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class CameraFollower : MonoBehaviour
    {
        public static CameraFollower Instance;
        public float screenShake = 2;

        [SerializeField] private Transform target;
        [SerializeField] private float smoothSpeed = 0.125f;
        [SerializeField] private Vector3 offset;

        private float m_shakeIntensity;
        private float m_shakeDuration;

        private void Awake()
        {
            Instance = this;
        }

        private void LateUpdate()
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            if (m_shakeDuration > 0)
            {
                m_shakeDuration -= Time.deltaTime;
                transform.position = smoothedPosition + Random.insideUnitSphere * m_shakeIntensity;
                m_shakeIntensity = Mathf.Lerp(m_shakeIntensity, 0, Time.deltaTime * 10f);
            }
            else
            {
                transform.position = smoothedPosition;
            }
        }

        public void Shake()
        {
            m_shakeDuration = 0.5f;
            m_shakeIntensity = screenShake;
        }
    }
}