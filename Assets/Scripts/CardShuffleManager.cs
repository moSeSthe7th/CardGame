using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardFindingGame
{
    public class CardShuffleManager : MonoBehaviour
    { 
        [HideInInspector] public GameHandler gameHandler;
        private int shuffleCount = 20;
        private bool isCurrentShuffleCompleted;
        private int shuffledCardCount;

        public void Init(GameHandler handler)
        {
            gameHandler = handler;
        }

        public void ShuffleCards(List<GambleCard> gambleCards)
        {
            
            StartCoroutine(CR_ShuffleCards(gambleCards));
        }

        private IEnumerator CR_ShuffleCards(List<GambleCard> gambleCards)
        {
            yield return new WaitForSeconds(0.5f / gameHandler.CardGameDataManager.CardGameData.shuffleSpeed);
            for(int i = 0; i< shuffleCount; i++)
            {
                isCurrentShuffleCompleted = false;
                int index1 = Random.Range(0, gambleCards.Count);
                int index2 = Random.Range(0, gambleCards.Count);

                while (index1 == index2)
                {
                    index2 = Random.Range(0, gambleCards.Count);
                }

                GambleCard card1 = gambleCards[index1];
                GambleCard card2 = gambleCards[index2];

                card1.OnCardAtSendedPosition += OnCardAtSendedPosition;
                card2.OnCardAtSendedPosition += OnCardAtSendedPosition;

                Vector2 card1Pos = card1.transform.position;
                Vector2 card2Pos = card2.transform.position;

                card1.SendCardToPosition(card2Pos, gameHandler.CardGameDataManager.CardGameData.shuffleSpeed);
                card2.SendCardToPosition(card1Pos, gameHandler.CardGameDataManager.CardGameData.shuffleSpeed);

                yield return new WaitUntil(()=>isCurrentShuffleCompleted);
            }
        }

        private void OnCardAtSendedPosition(GambleCard gambleCard)
        {
            gambleCard.OnCardAtSendedPosition -= OnCardAtSendedPosition;
            shuffledCardCount++;

            if(shuffledCardCount == 2)
            {
                shuffledCardCount = 0;
                isCurrentShuffleCompleted = true;
            }
        }
    }
}

