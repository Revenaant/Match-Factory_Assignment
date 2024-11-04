using Revenaant.Project.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Revenaant.Project
{
    public class MatchRow : MonoBehaviour
    {
        [SerializeField] private MatchSlot slotPrefab;

        [Header("Slot line setup")]
        [SerializeField] private int slotCount = 7;
        [SerializeField] private float slotSpacing = 8f;
        [SerializeField] private float slotAngle = 85f;
        [SerializeField] private float slotScale = 0.65f;

        private List<MatchSlot> matchSlots;
        private List<int> emptySlotIndexes;

        private int matchTarget;
        private bool isOnLastSlot = false;

        public IReadOnlyCollection<MatchSlot> MatchSlots => matchSlots;

        private void Awake()
        {
            matchTarget = LevelConfigProvider.Instance.GetConfig().MatchTarget;

            matchSlots = new List<MatchSlot>(slotCount);
            emptySlotIndexes = new List<int>(slotCount);

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
            InsertItem(item.VisualMatcher);

            if (!TryCheckMatches() && isOnLastSlot)
            {
                TriggerGameLost();
                return;
            }

            isOnLastSlot = false;
            ShiftRemainingItemsToBeginning();
        }

        private void InsertItem(VisualMatcher item)
        {
            if (TryFindMatchingSlot(item, out int matchingIndex) && matchSlots.Any(x => x.IsEmpty))
            {
                int index = matchingIndex + 1;

                ShiftItems(index, 1);
                matchSlots[index].SetHeldItem(item);
                return;
            }
            
            emptySlotIndexes.Clear();
            for (int i = 0; i < matchSlots.Count; i++)
            {
                if (matchSlots[i].IsEmpty)
                    emptySlotIndexes.Add(i);
            }

            if (emptySlotIndexes.Count > 0)
            {
                matchSlots[emptySlotIndexes.First()].SetHeldItem(item);

                if (emptySlotIndexes.Count == 2)
                    matchSlots[^1].ShowWarning();
                else if (emptySlotIndexes.Count == 1)
                    isOnLastSlot = true;
            }
        }

        private bool TryFindMatchingSlot(IMatcher matcher, out int matchingSlotIndex)
        {
            MatchSlot matchingSlot = matchSlots.Find(slot => !slot.IsEmpty && matcher.IsMatch(slot.HeldItem));
            if (matchingSlot != null) 
            { 
                matchingSlotIndex = matchSlots.IndexOf(matchingSlot);
                return true;
            }

            matchingSlotIndex = -1;
            return false;
        }

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

        private bool TryCheckMatches()
        {
            // We take the assumption that matching items will always be next to each other
            for (int i = 0; i <= slotCount - matchTarget; i++)
            {
                // Since we shift items to the front, there's no more items to check if we find an empty slot
                if (matchSlots[i].IsEmpty)
                    break;

                bool isMatch = true;

                Guid lookupType = matchSlots[i].HeldItem.MatchID;
                for (int j = 1; j < matchTarget; j++)
                {
                    MatchSlot nextSlot = matchSlots[i + j];
                    if (nextSlot.IsEmpty || !nextSlot.HeldItem.IsMatch(lookupType))
                    {
                        isMatch = false; 
                        break;
                    }
                }

                if (!isMatch)
                    continue;
                
                TriggerMatchMade(matchTarget, matchSlots[i].HeldItem.MatchID);
                for (int j = 0; j < matchTarget; j++)
                {
                    matchSlots[i + j].PerformMatch();
                }
                return true;
            }

            return false;
        }

        private void TriggerGameLost()
        {
            CentralMessageBus.Instance.Raise(new GameOverMessage(GameOverType.OutOfMoves));
        }

        private void TriggerMatchMade(int matchSize, Guid objectType)
        {
            CentralMessageBus.Instance.Raise(new MatchMadeMessage(matchSize, objectType));
            matchSlots[^1].HideWarning();
        }
    }
}
