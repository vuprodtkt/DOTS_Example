using Unity.Entities;
using Unity.Mathematics;

namespace Assets.script.ComponentsAndTags
{
    public partial struct SpawnBullet : IComponentData
    {
        public Entity BulletPrefab;
        public float spawnSpeed;
        public float lastSpawnTime;
    }

    public partial struct BulletComponent : IComponentData, IEnableableComponent
    { 
    }

    public partial struct BulletMove : IComponentData
    {
        public float speed;
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