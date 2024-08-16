using UnityEngine;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "MonsterData", menuName = "StaticData/Monster")]
    public class MonsterStaticData : ScriptableObject
    {
        public MonsterTypeId MonsterTypeId;
        
        [Range(1, 100)]
        public int Hp;
        
        [Range(1f, 30f)]
        public float Damage;

        public int MaxLoot;
        public int MinLoot;
        
        [Range(1f, 30f)]
        public float MoveSpeed;

        [Range(0.5f, 1)]
        public float EffectiveDistance;
        
        [Range(0.5f, 1)]
        public float Cleavage;

        public GameObject Prefab;
    }
}