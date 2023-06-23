using Unity.Collections;
using Unity.Entities;

namespace Assets.script.ComponentsAndTags
{
    public partial struct LevelComponent : IComponentData
    {
        public int maxLevel;
        public int currentLevel;
        public int nextLevel;
    }
}