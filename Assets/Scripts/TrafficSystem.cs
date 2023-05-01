using System;
using System.Collections.Generic;
using System.Linq;
using KennethDevelops.ProLibrary.Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class TrafficSystem : MonoBehaviour
    {
        public enum RoadDirection
        {
            Right,
            Left,
            Down,
            Up
        }

        public RoadDirection direction;
        public int preloadedCardsMinAmount = 4;
        public int preloadedCardsMaxAmount = 10;
        public List< PoolManager > carPools;
        private List<TrafficNode> m_nodes;

        private void Awake()
        {
            m_nodes = GetComponentsInChildren<TrafficNode>().ToList();

            for (var i = 0; i < m_nodes.Count; i++)
            {
                m_nodes[i].SetSystem(this);
            }
        }

        private void Start()
        {
            var carsToPreloadWith = Random.Range(preloadedCardsMinAmount, preloadedCardsMaxAmount);

            Debug.Log(m_nodes.Count);

            for (var i = 0; i < carsToPreloadWith; i++)
            {
                var randomIndex = Random.Range(1, m_nodes.Count - 1);
                var randomFlowNode = GetNode(randomIndex);
                var position = randomFlowNode.transform.position;
                position -= (Vector3)GetDirection() * Random.Range(0, 64);
                var car = GetCar(position, Quaternion.identity);
                car.SetSystem(this, randomIndex);
            }
        }

        public NpcInput GetCar(Vector3 position, Quaternion rotation)
        {
            var randomPool = carPools[Random.Range(0, carPools.Count)];
            return randomPool.AcquireObject<NpcInput>(position, rotation);
        }

        public TrafficNode GetNode(int index)
        {
            if (m_nodes.Count <= index)
            {
                return null;
            }

            return m_nodes[index];
        }

        public Vector2 GetDirection()
        {
            switch (direction)
            {
                case RoadDirection.Right: return Vector2.right;
                case RoadDirection.Left: return Vector2.left;
                case RoadDirection.Up: return Vector2.up;
                case RoadDirection.Down: return Vector2.down;
            }
            return Vector2.zero;
        }
    }
}