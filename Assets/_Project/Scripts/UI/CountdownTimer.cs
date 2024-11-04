using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Revenaant.Project.UI
{
    public class CountdownTimer : MonoBehaviour
    {
        [Tooltip("In seconds")]
        [SerializeField] private float time;
        [SerializeField] private TextMeshProUGUI text;

        [SerializeField] private bool runOnStart = true;
        [SerializeField] private bool deactivateOnEnd = true;

        [SerializeField] private Image fillImage;
        [SerializeField] private RectTransform clockTransform;
        [SerializeField] private float clockTickAnimAngle;

        private TimeSpan timeLeft;
        private Quaternion clockRotation;

        private bool isRunning;
        private double totalSeconds;

        public bool IsRunning => isRunning;
        public double CurrentSeconds => timeLeft.TotalSeconds;
        public double TotalSeconds => totalSeconds;

        private Action timerFinishedEvent;
        public event Action TimerFinishedEvent
        {
            add => timerFinishedEvent += value;
            remove => timerFinishedEvent -= value;
        }

        private void Start()
        {
            if (runOnStart)
                timeLeft = TimeSpan.FromSeconds(time);

            if (clockTransform != null)
                clockRotation = clockTransform.localRotation;
        }

        private void Update()
        {
            if (timeLeft.TotalMilliseconds <= 0)
            {
                OnTimerFinished();
                return;
            }

            timeLeft = timeLeft.Subtract(TimeSpan.FromSeconds(Time.deltaTime));
            text.text = $"{timeLeft.Minutes}:{timeLeft.Seconds:00}";

            if (fillImage != null && !DOTween.IsTweening(fillImage))
            {
                clockTransform.localRotation = clockRotation * Quaternion
                    .Euler(0, 0, timeLeft.Seconds % 2 == 0 
                    ? -clockTickAnimAngle 
                    : clockTickAnimAngle);

                fillImage.fillAmount = (float)timeLeft.TotalSeconds / time;
            }

            if (timeLeft.TotalSeconds <= 15)
            {
                text.color = timeLeft.Seconds % 2 == 0
                    ? Color.white 
                    : Color.red;
            }
        }

        public void SetTimerFromSeconds(float time)
        {
            gameObject.SetActive(true);
            timeLeft = TimeSpan.FromSeconds(time);
            text.text = $"{timeLeft.Minutes}:{timeLeft.Seconds:00}";

            totalSeconds = timeLeft.TotalSeconds;
            isRunning = true;
        }

        public void Restart()
        {
            timeLeft = TimeSpan.FromSeconds(time);
            text.text = $"{timeLeft.Minutes}:{timeLeft.Seconds:00}";

            if (fillImage != null)
            {
                fillImage.fillAmount = 0;
                fillImage.DOFillAmount(1, duration: 1.25f).SetEase(Ease.InOutCubic);
            }

            totalSeconds = timeLeft.TotalSeconds;
            isRunning = true;
        }

        private void OnTimerFinished()
        {
            if (!isRunning)
                return;

            isRunning = false;
            timerFinishedEvent?.Invoke();

            if (deactivateOnEnd)
                gameObject.SetActive(false);
        }
    }
}
