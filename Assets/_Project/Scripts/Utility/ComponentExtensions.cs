using UnityEngine;

namespace Revenaant.Project
{
    public static class ComponentExtensions
    {
        public static bool TryGetComponentInParent<T>(this Component origin, out T component) where T : Component
        {
            component = origin.GetComponentInParent<T>();
            return component != null;
        }

        public static bool TryGetComponentInChildren<T>(this Component origin, out T component) where T : Component
        {
            component = origin.GetComponentInChildren<T>();
            return component != null;
        }
    }
}
