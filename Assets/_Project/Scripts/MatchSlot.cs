using DG.Tweening;
using UnityEngine;

namespace Revenaant.Project
{
    public class MatchSlot : MonoBehaviour
    {
        [SerializeField] private Transform holder;

        private Interactable heldItem;

        public Transform Holder => holder;
        public bool IsEmpty => heldItem == null;

        public void SetHeldItem(Interactable item)
        {
            heldItem = item;

            // TODO move from here, I think interpolation should be on item, requesting holder transform.
            

            const float duration = 1;
            Sequence transformSequence = DOTween.Sequence();
            transformSequence.Join(item.transform.DOJump(holder.position, jumpPower: 3, numJumps: 1, duration));
            transformSequence.Join(item.transform.DORotate(holder.rotation.eulerAngles, duration, RotateMode.FastBeyond360));
            transformSequence.Join(item.transform.DOScale(holder.transform.localScale.z, duration));
            transformSequence.Play();
        }
    }
}
