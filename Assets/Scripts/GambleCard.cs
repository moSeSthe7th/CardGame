using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

namespace CardFindingGame
{

    public enum CardType
    {
        Dark,
        Light
    }

    public class GambleCard : MonoBehaviour
    {
        public UnityAction<GambleCard> OnCardSpawnFinished;
        public UnityAction<GambleCard> OnCardAtSendedPosition;

        public CardType cardType;
        [SerializeField] private List<Sprite> darkSprites;
        [SerializeField] private List<Sprite> lightSprites;

        [SerializeField] private SpriteRenderer cardFace;

        private Vector3 initialLocalScale;

        public void Init(CardType type)
        {
            cardType = type;
            SelectCardSprite();
            CardSpawnAnimations();
        }

        public void SendCardToPosition(Vector2 positionToGo, float speed)
        {
            transform.DOMove(positionToGo, 1f/speed).OnComplete(()=>OnCardAtSendedPosition?.Invoke(this));
        }

        private void CardSpawnAnimations()
        {
            initialLocalScale = transform.localScale;
            transform.localScale = Vector3.zero;
            Sequence sequence = DOTween.Sequence();
            sequence
                .Append(transform.DOScale(initialLocalScale, 0.6f).SetEase(Ease.OutBack))
                .AppendInterval(3f)
                .Append(transform.DORotate(new Vector3(0, 180f, 0), 0.3f).SetEase(Ease.InCubic))
                .OnComplete(()=>OnCardSpawnFinished?.Invoke(this));
            
        }

        private void SelectCardSprite()
        {
            if(cardType == CardType.Dark)
            {
                cardFace.sprite = darkSprites[Random.Range(0,darkSprites.Count)];
            }
            else if(cardType == CardType.Light)
            {
                cardFace.sprite = lightSprites[Random.Range(0, lightSprites.Count)];
            }
        }

        public void TurnCardToFront()
        {
            transform.DORotate(new Vector3(0,0,0),0.3f).SetEase(Ease.InCubic);
        }

        public void TurnCardToBack()
        {
            transform.DORotate(new Vector3(0, 180f, 0), 0.3f).SetEase(Ease.InCubic);
        }
    }
}

