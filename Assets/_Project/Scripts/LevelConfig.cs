using System;
using System.Collections.Generic;
using UnityEngine;

namespace Revenaant.Project
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "Configs/CreateLevelConfig")]
    public class LevelConfig : ScriptableObject
    {
        [SerializeField] private int level = 0;
        [SerializeField] private int itemCount = 200;
        [SerializeField] private List<ItemTypeToCount> goals;
        [SerializeField] private List<MatchTypeToPrefab> spawnableItems;

        public int Level => level;
        public int ItemCount => itemCount;
        public List<ItemTypeToCount> Goals => goals;
        public List<MatchTypeToPrefab> SpawnableItems => spawnableItems;

        private void OnValidate()
        {
            HashSet<MatchType> seenTypes = new HashSet<MatchType>();
            for (int i = spawnableItems.Count - 1; i >= 0; i--)
            {
                if (seenTypes.Contains(spawnableItems[i].Type))
                    spawnableItems.RemoveAt(i);
                else
                    seenTypes.Add(spawnableItems[i].Type);
            }
        }
    }

    [Serializable]
    public struct ItemTypeToCount
    {
        [SerializeField] private MatchTypeToPrefab type;
        [SerializeField] private int itemCount;

        public MatchTypeToPrefab Type => type;
        public int Count => itemCount;
    }

    [Serializable]
    public struct MatchTypeToPrefab
    {
        [SerializeField] private MatchType matchType;
        [SerializeField] private Interactable itemPrefab;

        public MatchType Type => matchType;
        public Interactable ItemPrefab => itemPrefab;
    }
}
