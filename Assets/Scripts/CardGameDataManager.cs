using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardFindingGame
{
    public class CardGameDataManager : MonoBehaviour
    {
        public CardGameData CardGameData;

        void Start()
        {

        }
        
        void Update()
        {

        }

        public void SetGameData(float bidAmount)
        {

            if(CardGameData.gameDifficulty == GameDifficulty.Easy)
            {
                CardGameData.requiredSuccessfulSelectionCount = 1;
                CardGameData.guessChanceCount = 1;
            }
            else if(CardGameData.gameDifficulty == GameDifficulty.Medium)
            {
                CardGameData.requiredSuccessfulSelectionCount = 1;
                CardGameData.guessChanceCount = 1;
            }
            else
            {
                CardGameData.requiredSuccessfulSelectionCount = 2;
                CardGameData.guessChanceCount = 2;
            }

            if (bidAmount < CardGameData.middleBiddingThreshold)
                CardGameData.shuffleSpeed = CardGameData.minShuffleSpeed;
            else if (bidAmount < CardGameData.maxBiddingThreshold)
                CardGameData.shuffleSpeed = CardGameData.middleShuffleSpeed;
            else
                CardGameData.shuffleSpeed = CardGameData.maxShuffleSpeed;
        }

        public GameDifficulty GetDifficulty()
        {
            return CardGameData.gameDifficulty;
        }

        public void SetDifficulty(GameDifficulty difficulty)
        {
            CardGameData.gameDifficulty = difficulty;
        }
        
    }
}

