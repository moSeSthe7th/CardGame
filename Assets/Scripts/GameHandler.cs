﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEditor;

namespace CardFindingGame
{
    public class GameHandler : MonoBehaviour
    {
        private CardGameDataManager _cardGameDataManager;
        public CardGameDataManager CardGameDataManager => _cardGameDataManager ?? (_cardGameDataManager = GetComponent<CardGameDataManager>());

        public static GameHandler instance;

        [SerializeField] private GambleCard gambleCardPrefab;
        [SerializeField] private NPCSpeakingPanel speakingPanel;
        [SerializeField] private CardShuffleManager shuffleManager;
        [SerializeField] private Gambler gambler;

        private List<GambleCard> createdCards;
        private int totalCardCount;
        private int createdCardCount;
        private int guessCounter;

        private bool isCardSelectionInputLocked;

        void Awake()
        {

            // if it doesn't exist
            if (instance == null)
            {
                print("assigning GameManager instance");
                // Set the instance to the current object (this)
                instance = this;
            }

            // There can only be a single instance of the game manager
            else if (instance != this)
            {
                print("GameManager instance already exists");
                // Destroy the current object, so there is just one manager
                Destroy(gameObject);
                return;
            }
            

        }

        void Start()
        {
            isCardSelectionInputLocked = true;
            speakingPanel.Init();
            gambler.Init();
            createdCardCount = 0;
            guessCounter = 0;
            createdCards = new List<GambleCard>();
            speakingPanel.OnDifficultySelected += CardGameDataManager.SetDifficulty;
            speakingPanel.OnBidButtonPresed += OnBidMade;
        }
        
        void Update()
        {
            ControlInput();
        }

        private void ControlInput()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (EditorApplication.isPlaying)
                {
                    UnityEditor.EditorApplication.isPlaying = false;
                }
            }
#endif

            

            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
               
                if(hit.collider != null)
                {
                    if (hit.collider.gameObject.CompareTag("GambleCard"))
                    {
                        if (isCardSelectionInputLocked)
                            return;

                        GambleCard gambleCard = hit.collider.gameObject.GetComponent<GambleCard>();
                        gambler.TurnCardToFront(gambleCard);
                        OnCardSelected(gambleCard.cardType);
                    }

                    /*else if (hit.collider.gameObject.CompareTag("Gambler"))
                    {
                        Gambler gambler = hit.collider.gameObject.GetComponent<Gambler>();
                        gambler.CloseGamblerCollider();
                        speakingPanel.OpenPanel();
                        speakingPanel.OnBidButtonPresed += OnBidMade;
                    }*/
                }
            }
        }
        
        private void CreateCardAtPosition(CardType type , Vector2 position)
        {

            if (type == CardType.Light)
            {
                GambleCard gambleCard = Instantiate(gambleCardPrefab, position, Quaternion.identity);
                gambleCard.Init(CardType.Light);
                createdCards.Add(gambleCard);
                gambleCard.OnCardSpawnFinished += OnCardSpawnFinished;
            }
            else
            {
                GambleCard gambleCard = Instantiate(gambleCardPrefab, position, Quaternion.identity);
                gambleCard.Init(CardType.Dark);
                createdCards.Add(gambleCard);
                gambleCard.OnCardSpawnFinished += OnCardSpawnFinished;

            }
        }

        private void OnShuffleCompleted()
        {
            shuffleManager.OnShuffleCompleted -= OnShuffleCompleted;
            isCardSelectionInputLocked = false;
        }

        private void OnCardSpawnFinished(GambleCard gambleCard)
        {
            gambleCard.OnCardSpawnFinished -= OnCardSpawnFinished;
            createdCardCount++;

            if(createdCardCount == totalCardCount)
            {
                shuffleManager.Init(this);
                shuffleManager.OnShuffleCompleted += OnShuffleCompleted;
                shuffleManager.ShuffleCards(createdCards);
            }
        }

        private List<Vector2> CalculateCardSpawnPositions()
        {
            List<Vector2> cardSpawnPosList = new List<Vector2>();
            Vector2 spawnPos;
            float xIncreaseAmount;
            float yIncreaseAmount = 0;

            if(CardGameDataManager.GetDifficulty() == GameDifficulty.Easy)
            {
                spawnPos = new Vector2(-5.5f,0);
                xIncreaseAmount = 5.5f;
                
                for(int i = 0; i<3; i++)
                {
                    cardSpawnPosList.Add(spawnPos);
                    spawnPos.x += xIncreaseAmount;
                }
            }
            else if(CardGameDataManager.GetDifficulty() == GameDifficulty.Medium)
            {
                spawnPos = new Vector2(-13.75f, 0);
                xIncreaseAmount = 5.5f;

                for (int i = 0; i < 6; i++)
                {
                    cardSpawnPosList.Add(spawnPos);
                    spawnPos.x += xIncreaseAmount;
                }
            }
            else if(CardGameDataManager.GetDifficulty() == GameDifficulty.Hard)
            {
                spawnPos = new Vector2(-5.5f, 4f);
                xIncreaseAmount = 5.5f;
                yIncreaseAmount = -8f;

                for (int i = 0; i < 6; i++)
                {
                    cardSpawnPosList.Add(spawnPos);
                    spawnPos.x += xIncreaseAmount;

                    if(i == 2)
                    {
                        spawnPos.x = -5.5f;
                        spawnPos.y += yIncreaseAmount;
                    }
                }
            }
            return cardSpawnPosList.OrderBy(x => Random.value).ToList();
        }

        private void OnBidMade(float bidAmount)
        {
            speakingPanel.OnBidButtonPresed -= OnBidMade;
            CardGameDataManager.SetGameData(bidAmount);
            StartCoroutine(CR_CreateCards());
        }

        private void OnCardSelected(CardType cardType)
        {
            if(cardType == CardType.Light)
            {
                OnLevelFailed();
                return;
            }

            guessCounter++;
            if (guessCounter >= CardGameDataManager.CardGameData.guessChanceCount)
                OnLevelSucceeded();
        }

        private void OnLevelSucceeded()
        {
            CardGameDataManager.OnGameWon();
            isCardSelectionInputLocked = true;
        }

        private void OnLevelFailed()
        {
            Debug.Log("Fail");
            CardGameDataManager.CardGameData.wonHandsCount = 0;
            isCardSelectionInputLocked = true;
        }

        private IEnumerator CR_CreateCards()
        {
            List<Vector2> cardSpawnPositions = CalculateCardSpawnPositions();
            int darkCardCount = 0;

            if (CardGameDataManager.GetDifficulty() == GameDifficulty.Easy)
            {
                darkCardCount = 1;
            }
            else if (CardGameDataManager.GetDifficulty() == GameDifficulty.Medium)
            {
                darkCardCount = 2;
            }
            else if (CardGameDataManager.GetDifficulty() == GameDifficulty.Hard)
            {
                darkCardCount = 2;
            }

            for (int i = 0; i < cardSpawnPositions.Count; i++)
            {
                totalCardCount++;
                if (i < darkCardCount)
                    CreateCardAtPosition(CardType.Dark, cardSpawnPositions[i]);
                else
                    CreateCardAtPosition(CardType.Light, cardSpawnPositions[i]);
                yield return new WaitForSeconds(0.3f);
            }
        }

        private void OnDestroy()
        {
            shuffleManager.OnShuffleCompleted -= OnShuffleCompleted;
        }
    }
}

