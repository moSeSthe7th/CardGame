using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;
using TMPro;

namespace CardFindingGame
{
    public class NPCSpeakingPanel : MonoBehaviour
    {
        public UnityAction<float> OnBidButtonPresed;
        public UnityAction<int> OnDifficultySelected;

        [SerializeField] private Button playButton;
        [SerializeField] private Button makeBidButton;
        [SerializeField] private RectTransform difficultySelectionPanel;
        [SerializeField] private TextMeshProUGUI gamblerSpeech;
        [SerializeField] private TextMeshProUGUI bidAmountText;
        [SerializeField] private GameObject bidPanel;

        private Vector3 initialDifficultyPanelScale;

        public void Init()
        {
            initialDifficultyPanelScale = difficultySelectionPanel.transform.localScale;
            difficultySelectionPanel.transform.localScale = Vector3.zero;

            playButton.transform.localScale = Vector3.zero;
            gamblerSpeech.gameObject.SetActive(false);
            makeBidButton.gameObject.SetActive(false);
            bidAmountText.text = 1.ToString();
            bidPanel.gameObject.SetActive(false);
            difficultySelectionPanel.gameObject.SetActive(false);

            OpenPanel();
        }

        public void OpenPanel()
        {
            gameObject.SetActive(true);
            playButton.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
            playButton.interactable = true;
        }

        public void MakeBidButtonPressed()
        {
            float bidAmount = 0;
            float.TryParse(bidAmountText.text, out bidAmount);
            ClosePanel();
            OnBidButtonPresed?.Invoke(bidAmount);
        }

        public void DifficultySelectorButtonPressed(int gameDifficulty)
        {
            difficultySelectionPanel.DOScale(initialDifficultyPanelScale, 0.3f).OnComplete(() =>
            {
                difficultySelectionPanel.gameObject.SetActive(false);
                GamblerSpeech("So, how much you bid?");
                makeBidButton.gameObject.SetActive(true);
                bidPanel.gameObject.SetActive(true);
            });
            
            OnDifficultySelected?.Invoke(gameDifficulty);
        }

        public void PlayButtonPressed()
        {
            playButton.interactable = false;

            playButton.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack).OnComplete(() => {
                difficultySelectionPanel.gameObject.SetActive(true);
                difficultySelectionPanel.DOScale(initialDifficultyPanelScale, 0.3f);
            });
        }

        public void BidIncreaseButtonPressed()
        {
            CardGameDataManager cardGameDataManager = GameHandler.instance.CardGameDataManager;
            float bidAmount = 0;
            float.TryParse(bidAmountText.text, out bidAmount);

            if (bidAmount >= cardGameDataManager.GetGamblersMoney() / cardGameDataManager.GetRatio())
                return;

            bidAmount++;
            bidAmountText.text = bidAmount.ToString();
        }

        public void BidDecreaseButtonPressed()
        {
            float bidAmount = 0;
            float.TryParse(bidAmountText.text, out bidAmount);

            if (bidAmount == 1)
                return;

            bidAmount--;
            bidAmountText.text = bidAmount.ToString();
        }


        private void ClosePanel()
        {
            gameObject.SetActive(false);
        }

        private void GamblerSpeech(string speech)
        {
            gamblerSpeech.gameObject.SetActive(true);
            gamblerSpeech.text = speech;
        }
    }

}
