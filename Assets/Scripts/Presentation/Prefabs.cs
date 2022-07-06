using System;
using DELTation.LeoEcsExtensions.Views;
using Simulation.Ids;

namespace Presentation
{
    [Serializable]
    public struct Prefabs
    {
        public EntityView PaddlePrefab;
        public EntityView BallPrefab;
        public EntityView HorizontalWallPrefab;

        public EntityView Resolve(ViewType viewType) =>
            viewType switch
            {
                ViewType.Paddle => PaddlePrefab,
                ViewType.Ball => BallPrefab,
                ViewType.HorizontalWall => HorizontalWallPrefab,
                _ => throw new ArgumentOutOfRangeException(nameof(viewType), viewType, null),
            };
    }
}