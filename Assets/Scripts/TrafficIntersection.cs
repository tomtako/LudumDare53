using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class TrafficIntersection : MonoBehaviour
    {
        public float minTimeToSwitch=2;
        public float maxTimeToSwitch=5;
        public float switchPauseDelay=2;

        public TrafficNode[] horizontalNodes;
        public TrafficNode[] verticalNodes;

        private float m_switchTimer;
        private float m_switchPauseTimer;
        private int m_currentFlow; // 0 = horz, 1 = vert

        private void Awake()
        {
            SetCurrentFlow(Random.Range(0,2));
        }

        private void Update()
        {
            if (m_switchPauseTimer > 0)
            {
                m_switchPauseTimer -= Time.deltaTime;

                if (m_switchPauseTimer <= 0)
                {
                    SetCurrentFlow(m_currentFlow);
                }

                return;
            }

            m_switchTimer -= Time.deltaTime;

            if (m_switchTimer <= 0)
            {
                m_switchTimer = Random.Range(minTimeToSwitch, maxTimeToSwitch);
                m_switchPauseTimer = switchPauseDelay;
                m_currentFlow = m_currentFlow == 0 ? 1 : 0;

                for (var i = 0; i < horizontalNodes.Length; i++)
                {
                    horizontalNodes[i].SetFlow(false);
                }

                for (var i = 0; i < verticalNodes.Length; i++)
                {
                    verticalNodes[i].SetFlow(false);
                }
            }
        }

        public void SetCurrentFlow(int flow)
        {
            m_currentFlow = flow;

            for (var i = 0; i < horizontalNodes.Length; i++)
            {
                horizontalNodes[i].SetFlow(m_currentFlow == 0);
            }

            for (var i = 0; i < verticalNodes.Length; i++)
            {
                verticalNodes[i].SetFlow(m_currentFlow == 1);
            }
        }
    }
}