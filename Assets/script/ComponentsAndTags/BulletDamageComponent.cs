using Unity.Entities;

namespace Assets.script.ComponentsAndTags
{
    public partial struct BulletDamageComponent : IComponentData
    {
        public Entity bullet;
        public Entity target;
    }
}