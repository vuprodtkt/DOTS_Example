using CortexDeveloper.ECSMessages.Components;
using Unity.Entities;

namespace Assets.script.ComponentsAndTags
{
    public partial struct StateGameComponent : IComponentData
    {
        // 0: menu game
        // 1: game loop
        // 2: pause game
        // 3: game over
        // 4: win game
        public int state;
    }
}