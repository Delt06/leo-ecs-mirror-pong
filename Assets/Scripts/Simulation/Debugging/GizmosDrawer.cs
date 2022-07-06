using System;
using UnityEngine;

namespace Simulation.Debugging
{
    public class GizmosDrawer : MonoBehaviour
    {
        public Action DrawAction { get; set; }

        private void OnDrawGizmos()
        {
            DrawAction?.Invoke();
        }
    }
}