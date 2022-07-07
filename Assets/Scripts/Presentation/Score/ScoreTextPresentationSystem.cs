using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Simulation;
using Simulation.Ids;

namespace Presentation.Score
{
    public class ScoreTextPresentationSystem : IEcsRunSystem, IEcsInitSystem
    {
        private readonly EcsFilterInject<Inc<ScoreText>> _scoreTextFilter = default;
        private readonly EcsFilterInject<Inc<SimulationState>> _stateFilter = default;
        private IReadOnlyDictionary<SyncedEntityId, int> _viewEntityIds;

        public void Init(EcsSystems systems)
        {
            _viewEntityIds = systems.GetPresentationData().ViewEntityIds;
        }


        public void Run(EcsSystems systems)
        {
            foreach (var i in _stateFilter)
            {
                ref var simulationState = ref _stateFilter.Pools.Inc1.Get(i);
                foreach (var score in simulationState.Scores)
                {
                    if (!_viewEntityIds.TryGetValue(score.EntityId, out var view)) continue;
                    if (!_scoreTextFilter.Contains(view)) continue;

                    ref var scoreText = ref _scoreTextFilter.Pools.Inc1.Get(view);
                    if (scoreText.LastShownValue == score.Score) continue;

                    scoreText.Text.SetText("{0:0}", score.Score);
                    scoreText.LastShownValue = score.Score;
                }
            }
        }
    }
}