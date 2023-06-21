using Unity.Entities;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace Assets.script.AuthoringAndMono
{
    public class SpawnAuthoring : MonoBehaviour
    {
        public GameObject EnemyPrefab;
        public int totalEnemy = 20;
        public int rowSpawnEnemy = 4;

        public GameObject PlayerPrefab;
        public float3 initPositionPlayer = new float3(1,-8,0);
    }

    public class SpawnEnemyBake : Baker<SpawnAuthoring>
    {
        public override void Bake(SpawnAuthoring authoring)
        {
            Entity SpawnEntity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(SpawnEntity, new ComponentsAndTags.SpawnEnemy
            {
                EnemyPrefab = GetEntity(authoring.EnemyPrefab),
                totalEnemy = authoring.totalEnemy,
                rowSpawnEnemy = authoring.rowSpawnEnemy,
            });
        }
    }

    public class SpawnPlayerBake : Baker<SpawnAuthoring>
    {
        public override void Bake(SpawnAuthoring authoring)
        {
            Entity SpawnEntity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(SpawnEntity, new ComponentsAndTags.SpawnPlayer
            {
                PlayerPrefab = GetEntity(authoring.PlayerPrefab),
                position = authoring.initPositionPlayer,
                isSpawn = false,
            });
        }
    }
}