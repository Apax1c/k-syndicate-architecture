using CodeBase.Infrastructure.Factory;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Enemy
{
    public class AgentMoveToPlayer : Follow
    {
        private const float MinimalDistance = 1;

        public NavMeshAgent Agent;

        private Transform _heroTransform;
        private IGameFactory _gameFactory;

        public void Construct(Transform heroTransform) => 
            _heroTransform = heroTransform;

        private void Update() => 
            SetDestination();

        private void SetDestination()
        {
            if (_heroTransform) 
                Agent.destination = _heroTransform.position;
        }
    }
}
