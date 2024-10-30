using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Revenaant.Project
{
    public class ItemSpawner : MonoBehaviour
    {
        [SerializeField] private LevelConfig config;

        private Collider area;

        private void Awake()
        {
            area = transform.GetComponentInChildren<Collider>();
            area.isTrigger = true;
        }

        private void Start()
        {
            SpawnItems();
        }

        private void SpawnItems()
        {
            // TODO guaranteed at least X to complete config.Goals
            for (int i = 0; i < config.ItemCount; i++) 
            {
                int randomIndex = Random.Range(0, config.SpawnableItems.Count);
                Interactable itemToSpawn = config.SpawnableItems[randomIndex].ItemPrefab;

                Vector3 position = GetRandomPositionInArea();
                Instantiate(itemToSpawn, position, Quaternion.identity);
            }
        }

        private Vector3 GetRandomPositionInArea()
        {
            Bounds bounds = area.bounds;

            Vector3 randomPoint = bounds.center;
            randomPoint.x += Random.Range(-bounds.extents.x, bounds.extents.x);
            randomPoint.y += Random.Range(-bounds.extents.y, bounds.extents.y);
            randomPoint.z += Random.Range(-bounds.extents.z, bounds.extents.z);
            return randomPoint;
        }
    }
}
