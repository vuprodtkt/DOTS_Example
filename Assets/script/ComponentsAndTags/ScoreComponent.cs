using Unity.Collections;
using Unity.Entities;

namespace Assets.script.ComponentsAndTags
{
    public partial struct ScoreComponent : IComponentData
    {
        public int score;
    }
}