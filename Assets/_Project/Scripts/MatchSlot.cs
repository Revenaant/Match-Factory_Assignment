using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Revenaant.Project
{
    public class MatchSlot : MonoBehaviour
    {
        [SerializeField] private Transform holder;
        [SerializeField] private Transform visualBase;

        [SerializeField] private TextMeshProUGUI debugText;

        private Matchable heldItem;
        private Sequence transformSequence;

        public Transform Holder => holder;
        public Matchable HeldItem => heldItem;
        public bool IsEmpty => heldItem == null;

        public void Initialize(Vector3 position, float angle, float scale)
        {
            transform.position = position;

            holder.rotation = Quaternion.Euler(angle, 0, 0);

            holder.localScale *= scale;
            visualBase.localScale *= scale;
        }

        private void Update()
        {
            if (heldItem == null)
            {
                debugText.text = "Empty";
                return;
            }
            debugText.text = heldItem.transform.name;
        }

        public void SetHeldItem(Matchable item)
        {
            heldItem = item;

            if (heldItem == null)
            {
                debugText.text = "Empty";
                return;
            }
            debugText.text = heldItem.transform.name;

            item.transform.SetParent(null, true);

            // TODO move from here, I think interpolation should be on item, requesting holder transform.

            item.transform.DOKill();

            const float duration = 1;
            transformSequence = DOTween.Sequence();
            transformSequence.Join(item.transform.DOJump(holder.position, jumpPower: 3, numJumps: 1, duration));
            transformSequence.Join(item.transform.DORotate(holder.rotation.eulerAngles, duration, RotateMode.FastBeyond360));
            transformSequence.Join(item.transform.DOScale(holder.transform.localScale.z, duration));
            transformSequence.AppendCallback(() => item.transform.SetParent(this.transform, true));
            transformSequence.Play();
        }

        public void ClearHeldItem()
        {
            // TODO wait for sequence to complete (or start another, but do clear the ref).
            transformSequence.Kill();
            heldItem.transform.DOKill();
            
            heldItem = null;
        }

        public void PerformMatch()
        {
            transformSequence.Kill();
            heldItem.transform.DOKill();

            Destroy(heldItem.gameObject);
            heldItem = null;
        }
    }
}
