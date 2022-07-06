using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Simulation.Physics.Components.Physics.Shapes;
using UnityEditor;
using UnityEngine;
using Mesh = Simulation.Physics.Components.Physics.Shapes.Mesh;
using Pose = Simulation.Physics.Components.Physics.Pose;

namespace Simulation.Debugging
{
    public class ShapesGizmosSystem : IEcsInitSystem, IEcsDestroySystem
    {
        private readonly EcsFilterInject<Inc<Pose, Circle>> _circleFilter = default;
        private readonly EcsFilterInject<Inc<Pose, Mesh>> _meshFilter = default;
        private readonly EcsFilterInject<Inc<Segment>> _segmentFilter = default;
        private GizmosDrawer _gizmosDrawer;

        public void Destroy(EcsSystems systems)
        {
#if UNITY_EDITOR
            if (_gizmosDrawer == null) return;
            Object.Destroy(_gizmosDrawer.gameObject);
            _gizmosDrawer = null;
#endif
        }

        public void Init(EcsSystems systems)
        {
#if UNITY_EDITOR
            _gizmosDrawer = new GameObject("Gizmos Drawer").AddComponent<GizmosDrawer>();
            _gizmosDrawer.DrawAction = () =>
            {
                Gizmos.color = Color.green;
                Handles.color = Color.green;

                foreach (var i in _circleFilter)
                {
                    ref var pose = ref _circleFilter.Pools.Inc1.Get(i);
                    ref var circle = ref _circleFilter.Pools.Inc2.Get(i);
                    Handles.DrawWireDisc(pose.Position.ToUnityVector(), Vector3.back, circle.Radius);
                }

                foreach (var i in _meshFilter)
                {
                    ref var pose = ref _meshFilter.Pools.Inc1.Get(i);
                    ref var mesh = ref _meshFilter.Pools.Inc2.Get(i);

                    foreach (var triangle in mesh.Triangles)
                    {
                        Handles.DrawLine((triangle.A + pose.Position).ToUnityVector(),
                            (triangle.B + pose.Position).ToUnityVector()
                        );
                        Handles.DrawLine((triangle.A + pose.Position).ToUnityVector(),
                            (triangle.C + pose.Position).ToUnityVector()
                        );
                        Handles.DrawLine((triangle.B + pose.Position).ToUnityVector(),
                            (triangle.C + pose.Position).ToUnityVector()
                        );
                    }
                }

                foreach (var i in _segmentFilter)
                {
                    ref var segment = ref _segmentFilter.Pools.Inc1.Get(i);
                    Handles.DrawLine(segment.A.ToUnityVector(), segment.B.ToUnityVector());
                }
            };
#endif
        }
    }
}