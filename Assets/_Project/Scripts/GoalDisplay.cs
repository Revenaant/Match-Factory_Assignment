using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Revenaant.Project
{
    public class GoalDisplay : MonoBehaviour
    {
        [SerializeField] private Transform[] goalSlot;
        [SerializeField] private TextMeshProUGUI[] goalTexts;
        [SerializeField] private float spacing = 2f;
        [SerializeField] private float displayHeight;

        [Header("Tween parameters")]
        [SerializeField] private float attentionDuration = 1f;
        [SerializeField] private float attentionInterval = 10f;
        [SerializeField] private float attentionScale = 1.2f;
        [SerializeField] private float attentionShake = 10f;

        private Sequence attentionSequence;

        public void Initialize(List<ItemTypeToCount> goals)
        {
            Debug.Assert(goals.Count <= goalSlot.Length, "Not enough slots to display goals");
            for (int i = 0; i < goals.Count; i++) 
            {
                float positionX = (i - (goals.Count - 1) * 0.5f) * spacing;
                goalSlot[i].localPosition = new Vector3(positionX, 0, 0);
                goalSlot[i].gameObject.SetActive(true);

                GameObject goalVisual = Instantiate(goals[i].VisualType, goalSlot[i]);
                goalVisual.transform.localPosition = Vector3.up * displayHeight;

                goalTexts[i].text = goals[i].Count.ToString();
            }

            CreateAttentionSequence();
            CallForAttention();
        }

        private void CreateAttentionSequence()
        {
            attentionSequence = DOTween.Sequence();

            foreach (Transform parent in goalSlot)
            {
                if (!parent.gameObject.activeInHierarchy)
                    continue;

                var item = parent.GetChild(2);

                Vector3 originalScale = item.localScale;
                Quaternion originalRotation = item.localRotation;

                Sequence sequence = DOTween.Sequence();
                sequence.Join(item.DOScale(originalScale * attentionScale, attentionDuration * 0.5f).SetEase(Ease.OutBounce));
                sequence.Join(item.DOShakeRotation(attentionDuration, attentionShake, randomnessMode: ShakeRandomnessMode.Harmonic));
                sequence.Join(item.DOScale(originalScale, attentionDuration * 0.5f)
                    .SetEase(Ease.InBounce)
                    .SetDelay(attentionDuration * 0.5f));

                attentionSequence.Append(sequence).Join(DOTween.Sequence().AppendInterval(attentionDuration * 0.66f));
            }
        }

        public void CallForAttention()
        {
            if (!attentionSequence.IsPlaying())
                attentionSequence.Restart();
        }

        public void UpdateAmounts(List<ItemTypeToCount> goalItemsRemaining)
        {
            for (int i = 0; i < goalItemsRemaining.Count; i++)
            {
                ItemTypeToCount item = goalItemsRemaining[i];

                // Ideally should use events to update single item, instead of all and using this check
                if (!string.Equals(goalTexts[i].text, item.Count.ToString()))
                {
                    goalTexts[i].text = item.Count.ToString();
                    goalTexts[i].transform.DOPunchScale(Vector3.one * 1.2f, 1, vibrato: 5);
                }
            }
        }
    }
}
