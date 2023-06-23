using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.script.AuthoringAndMono
{
    public class SpawnAuthoring : MonoBehaviour
    {
        public GameObject PlayerPrefab;
        public float3 initPositionPlayer = new float3(1,-8,0);
    }

    public class SpawnerBake : Baker<SpawnAuthoring>
    {
        public override void Bake(SpawnAuthoring authoring)
        {
            Entity SpawnEntity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(SpawnEntity, new ComponentsAndTags.SpawnEnemyTag
            {
            });
            AddComponent(SpawnEntity, new ComponentsAndTags.SpawnPlayer
            {
                PlayerPrefab = GetEntity(authoring.PlayerPrefab, TransformUsageFlags.Dynamic),
                position = authoring.initPositionPlayer,
                isSpawn = false,
            });
        }
    }
}