using Unity.Entities;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace Assets.script.AuthoringAndMono
{
    public class SpawnBulletAuthoring : MonoBehaviour
    {
        public GameObject BulletPrefab;
        public float spawnSpeed = 0.1f;
    }

    public class SpawnBulletBake : Baker<SpawnBulletAuthoring>
    {
        public override void Bake(SpawnBulletAuthoring authoring)
        {
            Entity SpawnEntity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(SpawnEntity, new ComponentsAndTags.SpawnBullet
            {
                BulletPrefab = GetEntity(authoring.BulletPrefab, TransformUsageFlags.Dynamic),
                lastSpawnTime = authoring.spawnSpeed,
                spawnSpeed = authoring.spawnSpeed,
            });

            AddComponent(SpawnEntity, new ComponentsAndTags.PlayerCannon {});
        }
    }
}