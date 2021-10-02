using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "TargetTransform", order = 0)]
    public class TargetTransform : ScriptableObject
    {
        public Transform targetTransform;
    }
}