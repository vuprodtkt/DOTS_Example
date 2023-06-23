using Unity.Entities;
using UnityEngine;

namespace Assets.script.AuthoringAndMono
{
    public class ScoreAuthoring : MonoBehaviour
    {
        public int score = 0;
    }

    public class ScoreBake : Baker<ScoreAuthoring>
    {
        public override void Bake(ScoreAuthoring authoring)
        {
            Entity Entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(Entity, new ComponentsAndTags.ScoreComponent
            {
                score = authoring.score,
            });
        }
    }
}