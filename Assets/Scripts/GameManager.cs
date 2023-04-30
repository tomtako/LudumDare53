using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.GameCenter;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class GameManager : MonoBehaviour
    {
        public enum GameState
        {
            Title,
            Gameplay,
            TimesUp,
            NewDelivery,
            Served,
            GameOver,
        }

        public static GameManager Instance;
        public int maxGameTime = 100;
        public float minimumDistanceBetweenDeliveries;
        public float moneyDisplayAddSpeed = 5;
        public float moneyGainedFromHittingAPedestrian = .5f;
        public float moneyGainedFromHittingACar = 1f;
        public bool disableGameOvers;

        public GameObject titleUi;
        public GameObject gameplayUi;
        public TimesUpScreen timesUpUi;
        public GameObject newDeliveryUi;
        public GameObject servedUi;
        public GameObject gameOverUi;

        public TextMeshProUGUI gameTimer;
        public PlayerInput player;
        public DeliveryArrow arrow;
        public TextMeshProUGUI moneyLabel;

        private float m_currentGameTime;
        private Vector2 m_currentDeliveryPosition;

        private float m_targetMoney;
        private float m_currentMoney;

        private int m_pedestriansKilled;
        private int m_carsDestroyed;
        private int m_offendersServed;

        private List<GameObject> m_menus;

        private GameState m_currentGameState;

        private void Awake()
        {
            Instance = this;

            m_currentGameTime = maxGameTime;
            m_currentGameState = GameState.Gameplay;

            m_menus = new List<GameObject>();
            m_menus.Add(titleUi);
            m_menus.Add(gameplayUi);
            m_menus.Add(timesUpUi.gameObject);

            timesUpUi.OnContinue += OnTimesUpContinued;

            NewDelivery();
        }

        private void OnTimesUpContinued()
        {
            if (m_currentGameState == GameState.TimesUp)
            {
                //todo: animate out
                timesUpUi.SetActive(false);

                SetGameState( GameState.Gameplay );
            }
        }

        private void Update()
        {
            if (m_currentGameState == GameState.Gameplay)
            {
                m_currentMoney =
                    Mathf.MoveTowards(m_currentMoney, m_targetMoney, moneyDisplayAddSpeed * Time.deltaTime);
                moneyLabel.text =
                    m_currentMoney.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("en-US"));

                m_currentGameTime -= Time.deltaTime;

                if (m_currentGameTime < 0)
                {
                    m_currentGameTime = 0;
                }

                if (m_currentGameTime < 1)
                {
                    gameTimer.text = (m_currentGameTime * 1000f).ToString("F2") + " ms";
                }
                else
                {
                    gameTimer.text = $"{(int)m_currentGameTime + 1}";
                }

                arrow.SetArrowPointer(player.transform.position, m_currentDeliveryPosition);

                if (m_currentGameTime <= 0)
                {
                    GameOver();
                }
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

        public void AddMoney(float money)
        {
            m_targetMoney += money;
        }

        public void OnHitPedestrian()
        {
            m_pedestriansKilled++;
            AddMoney(moneyGainedFromHittingAPedestrian);
        }

        public void OnHitCar()
        {
            m_carsDestroyed++;
            AddMoney(moneyGainedFromHittingACar);
        }

        public GameState GetCurrentState()
        {
            return m_currentGameState;
        }

        public void SetGameState(GameState state)
        {
            if (m_currentGameState != state)
            {
                if (state == GameState.Gameplay)
                {
                    SceneManager.LoadScene("Main");
                    return;
                }

                if (state == GameState.TimesUp)
                {
                    timesUpUi.SetActive(true);
                }
            }

            m_currentGameState = state;
        }

        private void GameOver()
        {
            gameTimer.text = "0";

            if (!disableGameOvers)
            {
                SetGameState(GameState.TimesUp);
            }
        }
    }
}