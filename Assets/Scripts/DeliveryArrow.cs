using UnityEngine;

namespace DefaultNamespace
{
    public class DeliveryArrow : MonoBehaviour
    {
        public Transform arrow;
        public float distanceFromSource = 2;

        public void SetArrowPointer(Vector2 playerPosition, Vector2 endPoint)
        {
            transform.position = playerPosition;

            var direction = (endPoint - playerPosition).normalized;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            arrow.eulerAngles = new Vector3(0, 0, angle);
            arrow.localPosition = distanceFromSource * direction;
        }
    }
}