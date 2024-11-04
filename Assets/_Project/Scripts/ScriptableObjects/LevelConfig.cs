using System;
using System.Collections.Generic;
using UnityEngine;

namespace Revenaant.Project
{
    // A nice next step could be to make level/goal generation dynamic
    // by randomly generating goals and selecting themes.

    [CreateAssetMenu(fileName = "LevelConfig", menuName = "Configs/CreateLevelConfig")]
    public class LevelConfig : ScriptableObject
    {
        [SerializeField] private int levelId = 0;
        [SerializeField] private int itemCount = 200;
        [SerializeField] private int matchTarget = 3;
        [SerializeField] private List<ItemTypeToCount> goals;

        [Header("Addressable labels to load")]
        [SerializeField] private List<string> itemThemes;

        public int LevelID => levelId;
        public int ItemCount => itemCount;
        public int MatchTarget => matchTarget;
        public List<ItemTypeToCount> Goals => goals;
        public List<string> SpawnableThemes => itemThemes;
    }

    [Serializable]
    public struct ItemTypeToCount
    {
        [SerializeField] private GameObject visualType;
        [SerializeField] private int itemCount;

        public GameObject VisualType => visualType;
        public int Count
        {
            get => itemCount;
            set => itemCount = value;
        }
    }
}
