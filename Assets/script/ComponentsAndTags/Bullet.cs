using Unity.Entities;
using Unity.Mathematics;

namespace Assets.script.ComponentsAndTags
{
    public partial struct SpawnBullet : IComponentData
    {
        public Entity BulletPrefab;
        public float spawnSpeed;
        public float lastSpawnTime;
        public float3 direction_bullet;
    }

    public partial struct BulletComponent : IComponentData, IEnableableComponent
    {
        public bool bulletOfPlayer;
    }

    public partial struct BulletMove : IComponentData
    {
        public float speed;
    }

    public partial struct BulletDirection : IComponentData
    {
        public float3 direction;
    }

    public partial struct BulletRange : IComponentData
    {
        public float range;
    }

    public partial struct BulletDamage : IComponentData
    {
        public float damage;
    }
}