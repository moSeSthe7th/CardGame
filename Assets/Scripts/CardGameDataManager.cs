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

        public void SetDifficulty(int difficulty)
        {
            if ((GameDifficulty)difficulty == GameDifficulty.Easy)
                CardGameData.currentRatio = CardGameData.easyRatio;
            else if ((GameDifficulty)difficulty == GameDifficulty.Medium)
                CardGameData.currentRatio = CardGameData.mediumRatio;
            else if ((GameDifficulty)difficulty == GameDifficulty.Hard)
                CardGameData.currentRatio = CardGameData.hardRatio;

            CardGameData.gameDifficulty = (GameDifficulty)difficulty;
        }

        public void OnGameWon()
        {
            CardGameData.wonHandsCount++;
            if(CardGameData.maxShuffleSpeed < CardGameData.maxPossibleShuffleSpeed)
            {
                CardGameData.minShuffleSpeed++;
                CardGameData.middleShuffleSpeed++;
                CardGameData.maxShuffleSpeed++;
            }
        }

        public void SetGamblersMoney(float money)
        {
            CardGameData.gamblersMoney = money;
        }

        public float GetGamblersMoney()
        {
            return CardGameData.gamblersMoney;
        }

        public float GetRatio()
        {
            return CardGameData.currentRatio;
        }

        public int GetStealingPossibility()
        {
            return CardGameData.currentStealPossibility;
        }

        public void SetStealingPossibility()
        {
            if (CardGameData.wonHandsCount < CardGameData.minRequiredWinsForSteal)
            {
                CardGameData.currentStealPossibility = 0;
                return;
            }

            int possibility = ((CardGameData.wonHandsCount - CardGameData.minRequiredWinsForSteal) * CardGameData.stealPossibilityIncrease) + CardGameData.startStealPossibility;

            if (possibility > CardGameData.maxStealPossibility)
                possibility = CardGameData.maxStealPossibility;

            CardGameData.currentStealPossibility = possibility;
        }
        
    }
}

