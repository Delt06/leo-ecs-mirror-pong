using Presentation.Interpolation;
using UnityEngine;

namespace Presentation
{
    [CreateAssetMenu]
    public class PresentationConfig : ScriptableObject
    {
        public InterpolationSettings InterpolationSettings;
        public Prefabs Prefabs;
    }
}