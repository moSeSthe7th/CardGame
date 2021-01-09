using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardFindingGame
{
    public enum GameDifficulty
    {
        Easy = 0,
        Medium = 1,
        Hard = 2,
    }

    [CreateAssetMenu(fileName = "CardGameData", menuName = "ScriptableObjects/CardFindingGame")]
    public class CardGameData : ScriptableObject
    {
        
        public GameDifficulty gameDifficulty;
        public float shuffleSpeed;
        public int guessChanceCount;
        public int requiredSuccessfulSelectionCount;
        public float gamblersMoney;
        public int wonHandsCount;

        //Bidding thresholds
        [Space]
        [Header("Bidding")]
        public float middleBiddingThreshold;
        public float maxBiddingThreshold;

        //Shuffle Speeds
        [Space]
        [Header("Shuffling")]
        public float minShuffleSpeed;
        public float middleShuffleSpeed;
        public float maxShuffleSpeed;
        public float maxPossibleShuffleSpeed;

        //Gain Ratios
        [Space]
        [Header("Ratios")]
        public float easyRatio;
        public float mediumRatio;
        public float hardRatio;
        public float currentRatio;

        //CardStealingPossibilities
        [Space]
        [Header("Card Stealing")]
        public int minRequiredWinsForSteal;
        public int startStealPossibility;
        public int maxStealPossibility;
        public int stealPossibilityIncrease;
        public int currentStealPossibility;
        
       
    }
}

