using UnityEngine;
using UnityEngine.UI;

namespace Revenaant.Project.UI
{
    public class ToggleButton : GenericButton
    {
        [SerializeField] private Image buttonImage;
        [SerializeField] private Sprite onTexture;
        [SerializeField] private Sprite offTexture;

        protected bool isEnabled = false;

        protected override void OnButtonClicked()
        {
            Toggle();
            base.OnButtonClicked();
        }

        public virtual void Toggle()
        {
            isEnabled = !isEnabled;
            buttonImage.sprite = isEnabled ? onTexture : offTexture;
        }

        public virtual void ToggleOn()
        {
            isEnabled = true;
            buttonImage.sprite = onTexture;
        }

        public virtual void ToggleOff() 
        { 
            isEnabled = false;
            buttonImage.sprite = offTexture;
        }
    }
}
