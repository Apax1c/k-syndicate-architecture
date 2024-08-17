using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Logic.EnemySpawners
{
    public class SpawnPoint : MonoBehaviour, ISavedProgress
    {
        public MonsterTypeId MonsterTypeId;

        public string Id { get; set; }

        private bool _slain;
        
        private IGameFactory _factory;

        public void Construct(IGameFactory factory) => 
            _factory = factory;
        
        public void LoadProgress(PlayerProgress progress)
        {
            if (!progress.KillData.ClearedSpawners.Contains(Id))
                Spawn();
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            if(_slain)
                progress.KillData.ClearedSpawners.Add(Id);
        }

        private void Spawn() =>
            _factory
                .CreateMonster(MonsterTypeId, transform)
                .GetComponent<EnemyDeath>()
                .Happened += Slay;

        private void Slay() => 
            _slain = true;
    }
}