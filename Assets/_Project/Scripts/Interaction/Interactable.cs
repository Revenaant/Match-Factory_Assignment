using UnityEngine;

namespace Revenaant.Project
{
    [SelectionBase]
    public class Interactable : MonoBehaviour
    {
        [SerializeField] private Highlighter highlighter;
        [SerializeField] private VisualMatcher visualMatcher;

        private MatchRow matchRow;
        private Rigidbody body;

        public Rigidbody Body => body;
        public Highlighter Highlighter => highlighter;
        public VisualMatcher VisualMatcher => visualMatcher;

        private void Awake()
        {
            body = GetComponent<Rigidbody>();
            matchRow = FindObjectOfType<MatchRow>();
        }

        private void Start()
        {
            body.AddTorque(Random.onUnitSphere * 100, ForceMode.Acceleration);
        }

        public void Interact()
        {
            body.isKinematic = true;

            if (matchRow != null)
                matchRow.AddItem(this);

            HapticController.Instance.TriggerMedium();
        }
    }
}
