using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Revenaant.Project
{
    public class MatchRow : MonoBehaviour
    {
        [SerializeField] private int slotsSize = 8;
        [SerializeField] private MatchSlot slotPrefab;

        private List<MatchSlot> matchSlots;
        public IReadOnlyCollection<MatchSlot> MatchSlots => matchSlots;

        //private Action<MatchType> matchMadeEvent;
        //public event Action<MatchType> MatchMadeEvent;

        private void Awake()
        {
            matchSlots = new List<MatchSlot>(slotsSize);
            for (int i = 0; i < slotsSize; i++) 
            {
                // TODO proper position.
                MatchSlot newSlot = Instantiate(slotPrefab, this.transform);
                newSlot.transform.position += Vector3.right * (i * slotsSize) * 0.15f;
                matchSlots.Add(newSlot);
            }
        }

        public void AddItem(Interactable item)
        {
            MatchSlot slot = matchSlots.First(slot => slot.IsEmpty);
            if (slot != null) 
                slot.SetHeldItem(item);
        }

        private void CheckMatches()
        {
            foreach (MatchSlot slot in matchSlots) 
            {
                // have a counter for type seen, if type > 3, propagate back to all type items that they need to pop. Send event.
            }
        }
    }
}
