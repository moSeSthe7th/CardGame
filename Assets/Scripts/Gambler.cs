using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CardFindingGame
{
    public class Gambler : MonoBehaviour
    {
        public UnityAction OnCardStolen;

        private int stealPossibility;
        private CardGameDataManager CardGameDataManager;

        private bool isStealingActive;

        public void Init()
        {
            CardGameDataManager = GameHandler.instance.CardGameDataManager;
            isStealingActive = false;
            CardGameDataManager.SetStealingPossibility();
            stealPossibility = CardGameDataManager.GetStealingPossibility();

            int randomNum = Random.Range(1, 101);
            if (randomNum < stealPossibility)
                isStealingActive = true;
        }

        public void TurnCardToFront(GambleCard gambleCard)
        {
            if (isStealingActive && gambleCard.cardType == CardType.Dark)
            {
                gambleCard.ChangeCardType(CardType.Light);
                Debug.Log("Stolen");
                OnCardStolen?.Invoke();
            }

            gambleCard.TurnCardToFront();
        }


    }
}

