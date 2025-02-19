using System.Collections.Generic;
using UnityEngine;

namespace Revenaant.Project.Messages
{
    public class ItemsSwipedMessage : IMessage
    {
        private List<GameObject> itemPrefabs = new List<GameObject>();
        public List<GameObject> ItemPrefabs => itemPrefabs;

        public ItemsSwipedMessage(List<GameObject> itemPrefabs)
        {
            this.itemPrefabs = itemPrefabs;
        }
    }
}
