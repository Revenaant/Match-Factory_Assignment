using System.Collections.Generic;
using UnityEngine;

namespace Revenaant.Project
{
    public class Highlighter : MonoBehaviour
    {
        [SerializeField] private Material highlightMaterial;

        private Material highlightInstance;
        private readonly int highlightEnabledProperty = Shader.PropertyToID("_IsVisible");

        public void SetupHighlight()
        {
            AddHighlightMaterial(transform.GetComponentInChildren<Renderer>());
        }

        public void ToggleHighlight(bool value)
        {
            if (highlightInstance == null)
                return;

            transform.localScale = Vector3.one * (value ? 1.15f : 1f);
            highlightInstance.SetFloat(highlightEnabledProperty, value ? 1 : 0);

            if (value)
                HapticController.Instance.TriggerLight();
        }

        private void AddHighlightMaterial(Renderer renderer)
        {
            List<Material> materialList = new List<Material>();
            renderer.GetMaterials(materialList);

            highlightInstance = new Material(highlightMaterial);
            materialList.Add(highlightInstance);
            renderer.materials = materialList.ToArray();
        }
    }
}
