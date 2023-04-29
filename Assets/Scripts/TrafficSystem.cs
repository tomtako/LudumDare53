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