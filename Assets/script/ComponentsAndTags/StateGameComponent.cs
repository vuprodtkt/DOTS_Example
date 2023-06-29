﻿using CortexDeveloper.ECSMessages.Components;
using Unity.Entities;

namespace Assets.script.ComponentsAndTags
{
    public partial struct StateGameComponent : IComponentData
    {
        // 1: start game
        // 2: pause game
        // 3: game over
        // 4: win game
        // 0 || any: stop game
        public int state;
    }
}