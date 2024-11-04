using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine;
using System.Linq;

namespace Revenaant.Project
{
    public class LevelConfigProvider
    {
        private const string CONFIGS_LABEL = "LevelConfigs";

        private LevelConfig[] configs;
        private int currentConfigIndex;

        public static LevelConfigProvider Instance { get; private set; }

        public static void InitializeProvider()
        {
            new LevelConfigProvider();
        }

        private LevelConfigProvider()
        {
            if (Instance == null)
            {
                Instance = this;
                LoadConfigs();
            }
        }

        private async Task LoadConfigs()
        {
            AsyncOperationHandle<IList<LevelConfig>> handle = Addressables.LoadAssetsAsync<LevelConfig>(CONFIGS_LABEL, null);
            await handle.Task;

            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError("Failed to load assets from group: " + CONFIGS_LABEL);
                return;
            }

            configs = handle.Result.ToArray();
        }

        public void RandomizeLevel()
        {
            currentConfigIndex = Random.Range(0, configs.Length);
        }

        public void IncreaseLevel()
        {
            currentConfigIndex = (currentConfigIndex + 1) % configs.Length;
        }

        public LevelConfig GetConfig()
        {
            return configs[currentConfigIndex];
        }
    }
}
