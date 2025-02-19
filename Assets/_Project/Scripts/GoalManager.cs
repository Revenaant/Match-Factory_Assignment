using Revenaant.Project.Messages;
using Revenaant.Project.UI;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace Revenaant.Project
{
    public class GoalManager : MonoBehaviour
    {
        [SerializeField] private LevelConfig config;
        [SerializeField] private int destructionLives = 3;

        [SerializeField] private CountdownTimer countdownTimer;
        [SerializeField] private LevelLostWindow levelLostWindow;
        [SerializeField] private LevelWonWindow levelWonWindow;

        [SerializeField] private GoalDisplay display;

        private List<ItemTypeToCount> goalItemsRemaining;

        private void Start()
        {
            countdownTimer.TimerFinishedEvent += OnTimerFinished;

            CentralMessageBus.Instance.SubscribeTo<ItemsSpawnedMessage>(ProcessItemsSpawnedMessage);
            CentralMessageBus.Instance.SubscribeTo<MatchMadeMessage>(ProcessMatchMadeMessage);
            CentralMessageBus.Instance.SubscribeTo<GameOverMessage>(ProcessGameOverMessage);
            CentralMessageBus.Instance.SubscribeTo<GameWonMessage>(ProcessGameWonMessage);

            config = LevelConfigProvider.Instance.GetConfig();
            display.Initialize(config.Goals);

            goalItemsRemaining = new List<ItemTypeToCount>(config.Goals);
        }

        private void OnDestroy()
        {
            countdownTimer.TimerFinishedEvent -= OnTimerFinished;

            CentralMessageBus.Instance.UnsubscribeFrom<ItemsSpawnedMessage>(ProcessItemsSpawnedMessage);
            CentralMessageBus.Instance.UnsubscribeFrom<MatchMadeMessage>(ProcessMatchMadeMessage);
            CentralMessageBus.Instance.UnsubscribeFrom<GameOverMessage>(ProcessGameOverMessage);
            CentralMessageBus.Instance.UnsubscribeFrom<GameWonMessage>(ProcessGameWonMessage);
        }

        private void OnTimerFinished()
        {
            CentralMessageBus.Instance.Raise(new GameOverMessage(GameOverType.OutOfTime));
        }

        private void ProcessItemsSpawnedMessage(ref ItemsSpawnedMessage eventData)
        {
            countdownTimer.Restart();
            display.CallForAttention();
        }

        private void ProcessMatchMadeMessage(ref MatchMadeMessage eventData)
        {
            int totalItemsRemaining = 0;
            for (int i = 0; i < goalItemsRemaining.Count; i++)
            {
                ItemTypeToCount item = goalItemsRemaining[i];
                if (eventData.MatchObjectType == CreateGuidFromString(item.VisualType.name))
                {
                    int newCount = item.Count - eventData.MatchSize;
                    item.Count = newCount;
                    goalItemsRemaining[i] = item;
                }

                totalItemsRemaining += item.Count;
            }

            display.UpdateAmounts(goalItemsRemaining);

            if (totalItemsRemaining <= 0)
            {
                double timePercentage = countdownTimer.CurrentSeconds / countdownTimer.TotalSeconds;
                int stars = timePercentage > 0.50 ? 3 
                    : timePercentage > 0.20 ? 2 
                    : 1;

                CentralMessageBus.Instance.Raise(new GameWonMessage(stars, false));
            }
        }

        private void ProcessGameWonMessage(ref GameWonMessage eventData)
        {
            levelWonWindow.SetupEndWindow(eventData.Stars, eventData.IsFullClear);
            levelWonWindow.Show(this);
        }

        private void ProcessGameOverMessage(ref GameOverMessage eventData)
        {
            levelLostWindow.Show(this);
            levelLostWindow.SetupEndWindow(eventData.GameOverType);
        }

        private Guid CreateGuidFromString(string text)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(text));
                return new Guid(hashBytes);
            }
        }
    }
}
