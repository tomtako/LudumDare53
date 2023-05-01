
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class TrafficNode : MonoBehaviour
    {
        public enum NodeType
        {
            None,
            Spawn, // spawns new cars
            Flow, // controls if the car is able to pass or not
            Despawn // despawns the car
        }

        public NodeType nodeType;

        private TrafficSystem m_system;
        private bool m_isFlowing;
        private float m_spawnTimer;
        private float m_minSpawnDelay = 6;
        private float m_maxSpawnDelay = 12;

        private void Awake()
        {
            m_spawnTimer = 0;
        }

        public void SetSystem(TrafficSystem system)
        {
            m_system = system;
        }

        public void SetFlow(bool isFlowing)
        {
            m_isFlowing = isFlowing;
        }

        public bool IsFlowing()
        {
            return m_isFlowing;
        }

        private void Update()
        {
            if (nodeType == NodeType.Spawn)
            {
                m_spawnTimer -= Time.deltaTime;

                if (m_spawnTimer <= 0)
                {
                    SpawnNewCar();
                    m_spawnTimer = Random.Range(m_minSpawnDelay,m_maxSpawnDelay);
                }
            }
        }

        public void SpawnNewCar()
        {
            var car = m_system.GetCar(transform.position, Quaternion.identity);
            car.SetSystem(m_system);
        }
    }
}