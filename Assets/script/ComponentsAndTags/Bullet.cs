using Unity.Entities;
using Unity.Mathematics;

namespace Assets.script.ComponentsAndTags
{
    public struct SpawnBullet : IComponentData
    {
        public Entity BulletPrefab;
        public float spawnSpeed;
        public float lastSpawnTime;
    }

    public struct BulletComponent : IComponentData, IEnableableComponent
    { 
    }

    public struct BulletMove : IComponentData
    {
        public float speed;
    }

    public struct BulletRange : IComponentData
    {
        public float range;
    }

    public struct BulletDamage : IComponentData
    {
        public float damage;
    }
}