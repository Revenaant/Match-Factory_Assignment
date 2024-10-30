using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Revenaant.Project
{
    public class MatchRow : MonoBehaviour
    {
        [SerializeField] private MatchSlot slotPrefab;
        [SerializeField] private int matchTarget = 3;

        [Header("Slot line setup")]
        [SerializeField] private int slotCount = 7;
        [SerializeField] private float slotSpacing = 8f;
        [SerializeField] private float slotAngle = 85f;
        [SerializeField] private float slotScale = 0.65f;

        private List<MatchSlot> matchSlots;
        public IReadOnlyCollection<MatchSlot> MatchSlots => matchSlots;

        //private Action<MatchType> matchMadeEvent;
        //public event Action<MatchType> MatchMadeEvent;

        private void Awake()
        {
            matchSlots = new List<MatchSlot>(slotCount);

            float totalSpacing = (slotCount - 1) * slotSpacing;
            float halfWidth = totalSpacing * 0.5f;

            for (int i = 0; i < slotCount; i++)
            {
                MatchSlot newSlot = Instantiate(slotPrefab, this.transform);

                Vector3 position = newSlot.transform.position;
                position.x += (i * slotSpacing) - halfWidth;

                newSlot.Initialize(position, slotAngle, slotScale);

                matchSlots.Add(newSlot);
            }
        }

        public void AddItem(Interactable item)
        {
            Matchable matchable = item.GetComponent<Matchable>();
            InsertItem(matchable);

            // TODO Wait for tween to finish (or free up practially and queue the anim to finish).
            CheckMatches();
            ShiftRemainingItemsToBeginning();
        }

        private void InsertItem(Matchable item)
        {
            if (TryFindMatchingSlot(item, out int matchingIndex) && matchSlots.Any(x => x.IsEmpty))
            {
                int index = matchingIndex + 1;

                ShiftItems(index, 1);
                matchSlots[index].SetHeldItem(item);
                return;
            }
            
            MatchSlot slot = matchSlots.First(slot => slot.IsEmpty);
            if (slot != null)
                slot.SetHeldItem(item);
        }

        private bool TryFindMatchingSlot(Matchable matchable, out int matchingSlotIndex)
        {
            // TODO maybe iterate backwards, since we'd want to insert at end, not from start.
            // TODO find index directly
            MatchSlot matchingSlot = matchSlots.Find(slot => !slot.IsEmpty && slot.HeldItem.MatchType == matchable.MatchType);
            if (matchingSlot != null) 
            { 
                matchingSlotIndex = matchSlots.IndexOf(matchingSlot);
                return true;
            }

            matchingSlotIndex = -1;
            return false;
        }

        // TODO be able to move items back to front.
        private void ShiftItems(int startingIndex, int displacement)
        {
            Debug.Assert(displacement >= 0);

            for (int i = slotCount - 1; i >= startingIndex + displacement; i--)
                matchSlots[i].SetHeldItem(matchSlots[i - displacement].HeldItem);
        }

        private void ShiftRemainingItemsToBeginning()
        {
            bool recurse = false;

            for (int i = slotCount - 1; i > 0; i--)
            {
                MatchSlot current = matchSlots[i];
                MatchSlot previous = matchSlots[i - 1];

                if (!current.IsEmpty && previous.IsEmpty)
                {
                    previous.SetHeldItem(current.HeldItem);
                    current.ClearHeldItem();
                    recurse = true;
                }
            }

            if (recurse)
                ShiftRemainingItemsToBeginning();
        }

        private void CheckMatches()
        {
            // We take the assumption that matching items will always be next to each other
            for (int i = 0; i <= slotCount - matchTarget; i++)
            {
                // Since we shift items to the front, there's no more items to check if we find an empty slot
                if (matchSlots[i].IsEmpty)
                    break;

                bool isMatch = true;

                MatchType lookupType = matchSlots[i].HeldItem.MatchType;
                for (int j = 1; j < matchTarget; j++)
                {
                    MatchSlot nextSlot = matchSlots[i + j];
                    if (nextSlot.IsEmpty || nextSlot.HeldItem.MatchType != lookupType)
                    {
                        isMatch = false; 
                        break;
                    }
                }

                if (!isMatch)
                    continue;
                
                // TODO separate into method
                Debug.LogError("WEEEE HAVE A MATCHHHHHHH");
                for (int j = 0; j < matchTarget; j++)
                {
                    matchSlots[i + j].PerformMatch();
                }
                break;
            }
        }
    }
}
