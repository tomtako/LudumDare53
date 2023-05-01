using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class BreakableObject : MonoBehaviour
    {
        public enum ObjectType
        {
            Metal,
            Glass,
            Foliage,
        }

        public ObjectType objectType;
        public int particlesSpawnedWhenDestroyed = 10;
        public SpriteRenderer spriteRenderer;
        public List<Particle> particlePrefabs;

        public void OnHit(CarController carController)
        {
            // sfx here.
            switch (objectType)
            {
                case ObjectType.Metal:
                    break;
                case ObjectType.Glass:
                    break;
                case ObjectType.Foliage:
                    break;
            }

            for (var i = 0; i < particlesSpawnedWhenDestroyed; i++)
            {
                var direction = Random.insideUnitCircle;
                var randomOffset = Random.Range(0.1f, 0.3f);
                var particle = Instantiate(particlePrefabs[Random.Range(0, particlePrefabs.Count)],
                    transform.position + (Vector3)direction, Quaternion.identity);
                particle.SetDirection(direction);
            }

            gameObject.SetActive(false);
        }
    }
}