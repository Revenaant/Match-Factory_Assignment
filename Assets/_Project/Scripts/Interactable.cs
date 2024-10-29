using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Revenaant.Project
{
    [SelectionBase]
    public class Interactable : MonoBehaviour
    {
        private Rigidbody body;

        private void Awake()
        {
            body = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void Interact()
        {
            body.isKinematic = true;
            body.detectCollisions = false;

            body.DOMoveZ(-10, 1);
        }
    }
}
