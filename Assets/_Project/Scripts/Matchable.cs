using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Revenaant.Project
{
    public enum MatchType
    {
        A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z
    }

    public class Matchable : MonoBehaviour
    {
        [SerializeField] private MatchType matchType;
        public MatchType MatchType => matchType;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
