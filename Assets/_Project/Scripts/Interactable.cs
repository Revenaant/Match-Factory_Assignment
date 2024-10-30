using System.Collections.Generic;
using UnityEngine;

namespace Revenaant.Project
{
    [SelectionBase]
    public class Interactable : MonoBehaviour
    {
        private readonly int highlightEnabledProperty = Shader.PropertyToID("_IsVisible");

        [SerializeField] private Material highlightMaterial;
        private Material highlightInstance;

        private Rigidbody body;
        public Rigidbody Body => body;

        private void Awake()
        {
            body = GetComponent<Rigidbody>();

            Renderer renderer = GetComponentInChildren<Renderer>();
            AddHighlightMaterial(renderer);
        }

        private void Start()
        {
            body.AddTorque(Random.onUnitSphere * 100, ForceMode.Acceleration);
        }

        public void Interact()
        {
            body.isKinematic = true;

            // TODO use events
            MatchRow matchRow = FindObjectOfType<MatchRow>();
            if (matchRow != null)
            {
                matchRow.AddItem(this);
            }
        }

        public void ToggleHighlight(bool value)
        {
            transform.localScale = Vector3.one * (value ? 1.15f : 1f);
            highlightInstance.SetFloat(highlightEnabledProperty, value ? 1 : 0);
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
