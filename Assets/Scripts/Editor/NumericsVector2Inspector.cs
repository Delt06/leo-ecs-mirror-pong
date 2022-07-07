using System.Numerics;
using JetBrains.Annotations;
using Leopotam.EcsLite.UnityEditor;
using Simulation;
using UnityEditor;

namespace Editor
{
    [UsedImplicitly]
    internal sealed class NumericsVector2Inspector : EcsComponentInspectorTyped<Vector2>
    {
        public override bool OnGuiTyped(string label, ref Vector2 value, EcsEntityDebugView entityView)
        {
            var newValue = EditorGUILayout.Vector2Field(label, value.ToUnityVector()).ToNumericsVector();
            if (newValue == value) return false;
            value = newValue;
            return true;
        }
    }
}