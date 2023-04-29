using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class GameManager : MonoBehaviour
    {
        public int maxGameTime = 100;
        public float minimumDistanceBetweenDeliveries;
        public TextMeshProUGUI gameTimer;
        public PlayerInput player;
        public DeliveryArrow arrow;

        private float m_currentGameTime;
        private Vector2 m_currentDeliveryPosition;

        private void Awake()
        {
            m_currentGameTime = maxGameTime;

            NewDelivery();
        }

        private void Update()
        {
            m_currentGameTime -= Time.deltaTime;
            gameTimer.text = $"{(int)m_currentGameTime+1}";

            arrow.SetArrowPointer( player.transform.position, m_currentDeliveryPosition );

            if (m_currentGameTime <= 0)
            {
                GameOver();
            }
        }

        private void NewDelivery()
        {
            m_currentDeliveryPosition = player.transform.position;

            while (Vector2.Distance(m_currentDeliveryPosition, player.transform.position) <
                   minimumDistanceBetweenDeliveries)
            {
                var x = Random.Range(-256, 256);
                var y = Random.Range(-256, 256);

                m_currentDeliveryPosition = new Vector2(x, y);
            }
        }

        private void GameOver()
        {

        }
    }
}