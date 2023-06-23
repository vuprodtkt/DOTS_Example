using Unity.Entities;
using UnityEngine;

namespace Assets.script.AuthoringAndMono
{
    public class LevelAuthoring : MonoBehaviour
    {
        public int maxLevel = 0;
    }

    public class LevelBake : Baker<LevelAuthoring>
    {
        public override void Bake(LevelAuthoring authoring)
        {
            Entity Entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(Entity, new ComponentsAndTags.LevelComponent
            {
                maxLevel = authoring.maxLevel,
                currentLevel = 0,
                nextLevel = 0,
            });
        }
    }
}