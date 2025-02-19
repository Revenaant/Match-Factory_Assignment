using DG.Tweening;
using Revenaant.Project.Messages;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Revenaant.Project.Assets._Project.Scripts.UI
{
    public class LivesUI : MonoBehaviour
    {
        [SerializeField] private Image lifeUIPrefab;
        [SerializeField] private Transform livesHolder;

        private List<Image> lifeUIs;

        private int lives;

        private void Start()
        {
            lives = LevelConfigProvider.Instance.GetConfig().Lives;

            lifeUIs = new List<Image>();
            for (int i = 0; i < lives; i++)
            {
                lifeUIs.Add(Instantiate(lifeUIPrefab, livesHolder));
            }

            CentralMessageBus.Instance.SubscribeTo<ItemsSwipedMessage>(ProcessItemsSwipedMessage);
        }

        private void OnDestroy()
        {
            CentralMessageBus.Instance.UnsubscribeFrom<ItemsSwipedMessage>(ProcessItemsSwipedMessage);
        }

        private void ProcessItemsSwipedMessage(ref ItemsSwipedMessage eventData)
        {
            lives--;

            if (lives < 0)
            {
                CentralMessageBus.Instance.Raise(new GameOverMessage(GameOverType.OutOfLives));
                return;
            }

            // Having a 1 second duration before we trigger things is unsafe. The user may be quick enough to call this
            // before the duration has finished. This is okay enough as a quick implementation.
            lifeUIs[lives].DOFade(0, 1).OnComplete(() =>
            {
                lifeUIs.RemoveAt(lives);
            });
        }
    }
}
