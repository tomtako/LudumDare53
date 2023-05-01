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
        public float deliveryTimePerMeter = 1;
        public bool disableGameOvers;

        public GameObject houses;
        public GameObject titleUi;
        public GameHud gameHud;
        public TimesUpScreen timesUpUi;
        public GameObject newDeliveryUi;
        public GameObject servedUi;
        public GameObject gameOverUi;

        public TextMeshProUGUI gameTimer;
        public PlayerInput player;
        public DeliveryArrow arrow;
        public TextMeshProUGUI moneyLabel;

        private int m_currentVillain;
        private List<string> m_currentTextQueues;

        private float m_currentGameTime;
        private int m_currentDeliveryHouse;
        //private Vector2 m_currentDeliveryPosition;

        private float m_targetMoney;
        private float m_currentMoney;

        private int m_pedestriansKilled;
        private int m_carsDestroyed;
        private int m_offendersServed;

        private List<GameObject> m_menus;
        private List<Transform> m_houses;

        public GameState m_currentGameState;

        //audio stuff

        private FMOD.Studio.EventInstance bgMusic;
        private bool bgMusicSwitch = true;
        private FMOD.Studio.EventInstance ambientAudio;

        private void Awake()
        {
            Instance = this;

            m_currentGameTime = maxGameTime;
            m_currentGameState = GameState.Gameplay;

            m_menus = new List<GameObject>();
            m_menus.Add(titleUi);

            if (gameHud) m_menus.Add(gameHud.gameObject);
            if (timesUpUi) m_menus.Add(timesUpUi.gameObject);

            timesUpUi.OnContinue += OnTimesUpContinued;

            ambientAudio = FMODUnity.RuntimeManager.CreateInstance("event:/Ambient/ambient");
            ambientAudio.start();

        }

        private void Start()
        {
            m_houses = new List<Transform>();

            for (var i = 0; i < houses.transform.childCount; i++)
            {
                m_houses.Add(houses.transform.GetChild(i));
            }

            m_houses.Shuffle();
            NewDelivery();

            bgMusic = FMODUnity.RuntimeManager.CreateInstance("event:/Music/gameplayMusic");
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
                if (bgMusicSwitch)
                {
                    bgMusic.start();
                    bgMusicSwitch = false;
                }

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

                if (houses != null)
                {
                    var house = m_houses[m_currentDeliveryHouse];
                    arrow.SetArrowPointer(player.transform.position, house.position);
                }

                if (m_currentGameTime <= 0)
                {
                    GameOver();
                    bgMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                    bgMusicSwitch = true;
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Music/gameOver");
                }
            }
        }

        private List<string> crimes = new List<string>
        {
            "Public urination",
            "Walking your pet scorpion",
            "Plant theft",
            "Selling bath salts to minors",
            "Costco sample hoarding",
            "Asked to try too many ice cream samples",
            "Bird watching after curfew",
            "Eating popcorn too loud in the movie theater",
            "Farting at dinner",
            "Farting in a public place",
            "Unsolicited door knob licking",
            "Putting trash in the recycle and recyle into the trash",
            "Smuggling candy into a movie theater",
            "Sexual relations with a ladybug",
            "Attempted murder of a scam caller",
            "J-walking in a circular motion",
            "Stealing someone's thunder",
            "Eating someones leftovers",
            "Engaging in an impromptu game of hopscotch",
            "Failing to prove you're not actually a robot",
            "Wearing crocks with socks",
            "Smuggling mints from restaurants",
            "Escaping from custody of the hall monitor",
            "Indecent exposure of the forehead",
            "Driving under the influence of asbestos",
            "Attempted butterfly stalking",
            "Aggravated lizard kidnapping",
            "Mosquito hit and run",
            "Unlawfully carrying a cucumber",
            "Burglary of dog turds",
            "Throwing bananas at cars",
            "Skateboarding in traffic",
        };

        private void NewDelivery()
        {
            if (gameHud)
            {
                gameHud.AddText( -1, "You have a new criminal to serve!");
                gameHud.AddText(Random.Range(0,3), crimes[Random.Range(0,crimes.Count)]);
            }

            if (m_houses.Count <= 1)
            {
                return;
            }

            var lastHouse = m_houses[m_currentDeliveryHouse];

            while (Vector2.Distance(lastHouse.position, player.transform.position) < minimumDistanceBetweenDeliveries)
            {
                var x = Random.Range(-256, 256);
                var y = Random.Range(-256, 256);

                m_currentDeliveryHouse = Random.Range(0, m_houses.Count);
                lastHouse = m_houses[m_currentDeliveryHouse];
            }

            lastHouse.GetComponent<GoalHouse>().SetAsGoalHouse();
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
                    //SceneManager.LoadScene("Main");
                    return;
                }

                if (state == GameState.TimesUp)
                {
                    timesUpUi.SetActive(true);
                }
            }

            m_currentGameState = state;
        }

        public void OnGoalHit()
        {
            NewDelivery();
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