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

        [SerializeField] private Button talkButton;
        [SerializeField] private Button playButton;
        [SerializeField] private Button makeBidButton;
        [SerializeField] private TextMeshProUGUI gamblerSpeech;
        [SerializeField] private TMP_InputField bidInputField;

        public void Init()
        {
            talkButton.transform.localScale = Vector3.zero;
            playButton.transform.localScale = Vector3.zero;
            gamblerSpeech.gameObject.SetActive(false);
            makeBidButton.gameObject.SetActive(false);
            bidInputField.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }

        public void OpenPanel()
        {
            gameObject.SetActive(true);
            talkButton.transform.DOScale(Vector3.one,0.3f).SetEase(Ease.OutBack);
            playButton.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
            playButton.interactable = true;
            talkButton.interactable = true;
        }

        public void MakeBidButtonPressed()
        {
            float bidAmount = 0;
            float.TryParse(bidInputField.text, out bidAmount);
            ClosePanel();
            OnBidButtonPresed?.Invoke(bidAmount);
        }

        public void PlayButtonPressed()
        {
            playButton.interactable = false;
            talkButton.interactable = false;
            GamblerSpeech("So, how much you bid?");
            makeBidButton.gameObject.SetActive(true);
            bidInputField.gameObject.SetActive(true);
            //OnPlayButtonPresed?.Invoke();
            CloseButtons();
        }

        private void CloseButtons()
        {
            talkButton.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack);
            playButton.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack);
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
