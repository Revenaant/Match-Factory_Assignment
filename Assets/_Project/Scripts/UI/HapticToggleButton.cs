namespace Revenaant.Project.UI
{
    public class HapticToggleButton : ToggleButton
    {
        private void OnEnable()
        {
            if (HapticController.Instance.HapticsEnabled)
                ToggleOn();
            else
                ToggleOff();
        }

        public override void Toggle()
        {
            base.Toggle();

            if (HapticController.Instance.HapticsEnabled)
            {
                HapticController.Instance.Uninitialize();
            }
            else
            {
                HapticController.Instance.Initialize();
                HapticController.Instance.TriggerHeavy();
            }
        }

        public override void ToggleOn()
        {
            base.ToggleOn();
            HapticController.Instance.Initialize();
            HapticController.Instance.TriggerHeavy();
        }

        public override void ToggleOff()
        {
            base.ToggleOff();
            HapticController.Instance.Uninitialize();
        }
    }
}
