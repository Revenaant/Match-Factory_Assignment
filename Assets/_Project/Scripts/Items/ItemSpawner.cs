using UnityEngine;
using Random = UnityEngine.Random;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Revenaant.Project.Messages;

namespace Revenaant.Project
{
    public class ItemSpawner : MonoBehaviour
    {
        [SerializeField] private LevelConfig config;
        [SerializeField] private VisualMatcher baseItemPrefab;

        private Collider area;
        private ItemAssetLoader loader;

        private void Awake()
        {
            area = transform.GetComponentInChildren<Collider>();
            area.isTrigger = true;

            loader = new ItemAssetLoader();
        }

        private async void Start()
        {
            config = LevelConfigProvider.Instance.GetConfig();

            List<Task> loadTasks = new List<Task>();
            foreach (string theme in config.SpawnableThemes)
            {
                loadTasks.Add(loader.LoadItemSets(theme));
            }
            await Task.WhenAll(loadTasks);

            SpawnItems();
            CentralMessageBus.Instance.Raise(new ItemsSpawnedMessage());
        }

        private void OnDestroy()
        {
            // TODO don't unload if level restarts
            loader.UnloadAllItems();
        }

        private void SpawnItems()
        {
            int itemsToSpawn = config.ItemCount;
            IList<GameObject> spawnablePrefabs = loader.ItemAssets;

            foreach (ItemTypeToCount itemGoal in config.Goals)
            {
                GameObject goalObject = spawnablePrefabs.Single(asset => asset.name == itemGoal.VisualType.name);
                for (int i = 0; i < itemGoal.Count; i++)
                {
                    SpawnSceneObject(goalObject);
                }
                itemsToSpawn -= itemGoal.Count;

                spawnablePrefabs.Remove(goalObject);
            }

            while (itemsToSpawn > 0)
            {
                int randomVisual = Random.Range(0, loader.ItemAssets.Count);
                for (int i = 0; i < config.MatchTarget; i++)
                {
                    SpawnSceneObject(loader.ItemAssets[randomVisual]);
                }
                itemsToSpawn -= config.MatchTarget;
            }
        }

        private void SpawnSceneObject(GameObject visual)
        {
            Vector3 position = GetRandomPositionInArea();
            VisualMatcher spawnedItem = Instantiate(baseItemPrefab, position, Quaternion.identity, transform);
            spawnedItem.AddVisual(visual);
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
