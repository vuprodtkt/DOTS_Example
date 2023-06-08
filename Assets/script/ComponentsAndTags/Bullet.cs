using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.script.ComponentsAndTags
{
    public struct BulletSpawn : IComponentData
    {
        public Entity BulletPrefab;
        public Entity position;
        public float3 translation;
    }

    public struct BulletMove : IComponentData
    {
        public int state;
        public float range;
        public float speed;

    }

    public struct Bullet : IComponentData
    {
        public int damage;

    }
}