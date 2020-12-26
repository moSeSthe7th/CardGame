using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardFindingGame
{
    public enum GameDifficulty
    {
        Easy,
        Medium,
        Hard,
        Custom
    }

    [CreateAssetMenu(fileName = "CardGameData", menuName = "ScriptableObjects/CardFindingGame")]
    public class CardGameData : ScriptableObject
    {
        public GameDifficulty gameDifficulty;

        public float shuffleSpeed;
        public int guessChanceCount;
        public int requiredSuccessfulSelectionCount;

        public float middleBiddingThreshold;
        public float maxBiddingThreshold;

        public float minShuffleSpeed;
        public float middleShuffleSpeed;
        public float maxShuffleSpeed;

    }
}

