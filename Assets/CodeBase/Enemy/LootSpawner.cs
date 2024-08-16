using CodeBase.Data;
using CodeBase.Infrastructure.Factory;
using UnityEngine;
using Random = System.Random;

namespace CodeBase.Enemy
{
    public class LootSpawner : MonoBehaviour
    {
        public EnemyDeath EnemyDeath;
        private IGameFactory _factory;
        private int _lootMin;
        private int _lootMax;

        public void Construct(IGameFactory factory)
        {
            _factory = factory;
        }
        
        private void Start()
        {
            EnemyDeath.Happened += SpawnLoot;
        }

        private void SpawnLoot()
        {
            LootPiece loot = _factory.CreateLoot();
            loot.transform.position = transform.position;
            
            Loot lootItem = LootGenerate();
            loot.Initialize(lootItem);
        }

        private Loot LootGenerate()
        {
            Random rand = new Random();
            return new Loot()
            {
                Value = rand.Next(_lootMin, _lootMax)
            };
        }

        public void SetLoot(int min, int max)
        {
            _lootMin = min;
            _lootMax = max;
        }
    }
}