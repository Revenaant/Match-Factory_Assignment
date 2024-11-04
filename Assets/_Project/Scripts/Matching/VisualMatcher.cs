using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Revenaant.Project
{
    // Ideally, have a lookup table of model-guid
    public class VisualMatcher : MonoBehaviour, IMatcher
    {
        [SerializeField] private Transform visualsPivot;
        [SerializeField] private Highlighter highlighter;

        private Guid matchID;
        
        public Guid MatchID => matchID;

        public void AddVisual(GameObject itemModel)
        {
            InitializeMatchID(itemModel);

            GameObject visual = Instantiate(itemModel, visualsPivot);
            highlighter.SetupHighlight();
        }

        private void InitializeMatchID(GameObject visualObject)
        {
            matchID = CreateGuidFromString(visualObject.name);
        }

        public bool IsMatch(Guid idToCheck)
        {
            return matchID.Equals(idToCheck);
        }

        bool IMatcher.IsMatch(IMatcher otherMatcher)
        {
            return MatchID.Equals(otherMatcher.MatchID);
        }

        private Guid CreateGuidFromString(string text)
        {
            using (MD5 md5 = MD5.Create()) 
            {
                byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(text));
                return new Guid(hashBytes);
            }
        }
    }
}
