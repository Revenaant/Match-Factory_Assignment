using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Revenaant.Project
{
    public class ItemAssetLoader
    {
        private List<AsyncOperationHandle> loadedAssetHandles = new List<AsyncOperationHandle>();

        private IList<GameObject> loadedAssets = new List<GameObject>();
        public IList<GameObject> ItemAssets => loadedAssets;

        public async Task LoadItemSets(string group)
        {
            AsyncOperationHandle<IList<GameObject>> handle = Addressables.LoadAssetsAsync<GameObject>(group, null);
            await handle.Task;

            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError("Failed to load assets from group: " + group);
                return;
            }

            IList<GameObject> assets = handle.Result;
            foreach (GameObject asset in assets)
            {
                loadedAssets.Add(asset);
            }
            loadedAssetHandles.Add(handle);
        }

        public void UnloadAllItems()
        {
            foreach (AsyncOperationHandle asset in loadedAssetHandles)
                Addressables.Release(asset);

            loadedAssetHandles.Clear();
        }
    }
}
