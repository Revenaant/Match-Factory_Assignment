using System;
using UnityEngine;

namespace Revenaant.Project
{
    public class DestructionZone : MonoBehaviour
    {
        private Action<VisualMatcher> itemDestroyedEvent;
        public event Action<VisualMatcher> ItemDestroyedEvent
        {
            add { itemDestroyedEvent += value; }
            remove { itemDestroyedEvent -= value; }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponentInParent(out VisualMatcher item))
            {
                itemDestroyedEvent?.Invoke(item);
                Destroy(item.gameObject);
            }
        }
    }
}
