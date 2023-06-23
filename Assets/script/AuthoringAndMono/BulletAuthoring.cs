using Unity.Entities;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace Assets.script.AuthoringAndMono
{
    public class BulletAuthoring : MonoBehaviour
    {
        public float speed = 10f;
        public float maxRange = 15f;
        public float damage = 50f;
    }

    public class BulletAuthoringBake : Baker<BulletAuthoring>
    {
        public override void Bake(BulletAuthoring authoring)
        {
            Entity Entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(Entity, new ComponentsAndTags.BulletMove
            {
                speed = authoring.speed,
            });
            AddComponent(Entity, new ComponentsAndTags.BulletRange
            {
                range = authoring.maxRange,
            });
            AddComponent(Entity, new ComponentsAndTags.BulletComponent
            {
            
            });
            AddComponent(Entity, new ComponentsAndTags.BulletDamage
            {
                damage = authoring.damage,
            });
        }
    }

}