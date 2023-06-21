using Unity.Entities;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace Assets.script.ComponentsAndTags
{
    public struct SpawnEnemy : IComponentData
    {
        public Entity EnemyPrefab;
        public float3 position;
        public float3 translation;
        public int totalEnemy;
        public int rowSpawnEnemy;
    }

    public struct EnemyComponent: IComponentData, IEnableableComponent
    {

    }

    public struct EnemyMove : IComponentData
    {
        public float speed;
        public float3 direction;
        public float maxTimeMoveVertical;
        public float timeMoveVertical;
    }

    public struct EnemyRange : IComponentData
    {
        public float minHorizontal;
        public float maxHorizontal;
        public float minVertical;
        public float maxVertical;
    }
}