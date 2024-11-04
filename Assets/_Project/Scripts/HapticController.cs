using Revenaant.Project.Messages;
using UnityEngine;

namespace Revenaant.Project
{
    public class HapticController
    {
        private static HapticController instance;
        public static HapticController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new HapticController();

                    if (instance.HapticsEnabled)
                        instance.Initialize();
                    else 
                        instance.Uninitialize();
                }

                return instance;
            }
        }

        public bool HapticsEnabled => PlayerPrefs.GetInt(nameof(HapticsEnabled)) == 1;

        public void Initialize()
        {
            Vibration.Init();
            PlayerPrefs.SetInt(nameof(HapticsEnabled), 1);

            CentralMessageBus.Instance.SubscribeTo<MatchMadeMessage>(ProcessMatchMadeMessage);
        }

        public void Uninitialize()
        {
            // The Vibration package does not have an Uninitialize, so we just set hapticsEnabled.
            PlayerPrefs.SetInt(nameof(HapticsEnabled), 0);

            CentralMessageBus.Instance.UnsubscribeFrom<MatchMadeMessage>(ProcessMatchMadeMessage);
        }

        public void TriggerLight()
        {
            if (HapticsEnabled)
                Vibration.VibrateAndroid(milliseconds: 1);
        }

        public void TriggerMedium()
        {
            if (HapticsEnabled)
                Vibration.VibrateAndroid(milliseconds: 5);
        }

        public void TriggerHeavy()
        {
            if (HapticsEnabled)
                Vibration.VibrateAndroid(milliseconds: 25);
        }

        private void ProcessMatchMadeMessage(ref MatchMadeMessage eventData)
        {
            TriggerHeavy();
        }
    }
}
