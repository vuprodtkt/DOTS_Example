using Unity.Entities;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace Assets.script.AuthoringAndMono
{
    public class HealthAuthoring : MonoBehaviour
    {
        public float health = 100f;
    }

    public class HealthAuthoringBake : Baker<HealthAuthoring>
    {
        public override void Bake(HealthAuthoring authoring)
        {
            Entity Entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(Entity, new ComponentsAndTags.HealthComponent
            {
                health = authoring.health,
            });
        }
    }

}