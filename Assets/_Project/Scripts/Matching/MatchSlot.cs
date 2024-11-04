using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Revenaant.Project
{
    public class MatchSlot : MonoBehaviour
    {
        [SerializeField] private Transform holder;
        [SerializeField] private Transform visualBase;

        [Header("Material Highlight")]
        [SerializeField] private Material normalMaterial;
        [SerializeField] private Material warningMateiral;

        private Renderer baseRenderer;
        private VisualMatcher heldItem;
        private Sequence transformSequence;

        public Transform Holder => holder;
        public VisualMatcher HeldItem => heldItem;
        public bool IsEmpty => heldItem == null;

        public void Initialize(Vector3 position, float angle, float scale)
        {
            transform.position = position;

            holder.rotation = Quaternion.Euler(angle, 0, 0);

            holder.localScale *= scale;
            visualBase.localScale *= scale;

            baseRenderer = visualBase.GetComponent<Renderer>();
        }

        public void SetHeldItem(VisualMatcher item)
        {
            heldItem = item;
            if (item == null)
                return;

            item.transform.SetParent(null, true);
            item.transform.DOKill();
            ResetBasePosition();

            const float duration = 0.75f;
            transformSequence = DOTween.Sequence();
            transformSequence.Join(item.transform.DOJump(holder.position, jumpPower: 3, numJumps: 1, duration));
            transformSequence.Join(item.transform.DORotate(holder.rotation.eulerAngles, duration, RotateMode.FastBeyond360));
            transformSequence.Join(item.transform.DOScale(holder.transform.localScale.z, duration));

            transformSequence.Join(visualBase.transform.DOPunchPosition(-visualBase.transform.up * 0.2f, duration * 0.5f, 1, 1)
                .SetEase(Ease.OutBack)
                .SetDelay(duration * 0.80f));

            transformSequence.AppendCallback(() => item.transform.SetParent(this.transform, true));
            transformSequence.Play();
        }

        public void ClearHeldItem()
        {
            // TODO wait for sequence to complete (or start another, but do clear the ref).
            transformSequence.Kill();
            heldItem.transform.DOKill();
            ResetBasePosition();

            heldItem = null;
        }

        public void PerformMatch()
        {
            // TODO wait on sequence and trigger match animation before destroying.
            transformSequence.Kill();
            heldItem.transform.DOKill();
            ResetBasePosition();

            Destroy(heldItem.gameObject);
            heldItem = null;
        }

        public void ShowWarning()
        {
            baseRenderer.material = warningMateiral;

            visualBase.DOPunchPosition(-visualBase.up * 0.1f, 0.5f).SetLoops(-1);
        }

        public void HideWarning()
        {
            baseRenderer.material = normalMaterial;
            ResetBasePosition();
        }

        private void ResetBasePosition()
        {
            visualBase.transform.DOKill(true);
            Vector3 basePosition = visualBase.localPosition;
            basePosition.z = 0;
            basePosition.y = 0;
            visualBase.localPosition = basePosition;
        }
    }
}
