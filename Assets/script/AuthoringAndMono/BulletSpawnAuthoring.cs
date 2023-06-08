using Assets.script.ComponentsAndTags;
using System.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.script.AuthoringAndMono
{
    public class BulletSpawnẠuthoring : MonoBehaviour
    {
        public GameObject position;
        public float3 translation;
        public GameObject BulletPrefab;
    }

    public class SpawnBulletBaker : Baker<BulletSpawnẠuthoring>
    {
        public override void Bake(BulletSpawnẠuthoring authoring)
        {
            Entity BulletSpawnEntity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(BulletSpawnEntity, new ComponentsAndTags.BulletSpawn
            {
                BulletPrefab = GetEntity(authoring.BulletPrefab),
                position = GetEntity(authoring.position),
                translation = authoring.translation,
            });
        }
    }
}