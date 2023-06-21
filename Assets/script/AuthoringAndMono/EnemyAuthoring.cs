using Unity.Entities;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace Assets.script.AuthoringAndMono
{
    public class EnemyAuthoring : MonoBehaviour
    {
        public float speed = 10f;
        public float maxTimeMoveVertical = 10f;
        public float minHorizontal = -15;
        public float maxHorizontal = 15;
        public float minVertical = -9;
        public float maxVertical = 9;
    }

    public class EnemyAuthoringBake : Baker<EnemyAuthoring>
    {
        public override void Bake(EnemyAuthoring authoring)
        {
            Entity Entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(Entity, new ComponentsAndTags.EnemyMove
            {
                speed = authoring.speed,
                direction = new float3(0,0,1f),
                maxTimeMoveVertical = authoring.maxTimeMoveVertical,
                timeMoveVertical = 0f,
            });
            AddComponent(Entity, new ComponentsAndTags.EnemyRange
            {
                minHorizontal = authoring.minHorizontal,
                maxHorizontal = authoring.maxHorizontal,
                minVertical = authoring.minVertical,
                maxVertical = authoring.maxVertical,
            });
        }
    }
}