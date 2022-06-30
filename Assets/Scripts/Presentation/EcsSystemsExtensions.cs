using Leopotam.EcsLite;

namespace Presentation
{
    public static class EcsSystemsExtensions
    {
        public static PresentationSharedData GetPresentationData(this EcsSystems ecsSystems) =>
            ecsSystems.GetShared<PresentationSharedData>();
    }
}