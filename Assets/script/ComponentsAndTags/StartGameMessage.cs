using CortexDeveloper.ECSMessages.Components;
using Unity.Entities;

namespace Assets.script.ComponentsAndTags
{
    public partial struct StateGameMessage : IComponentData, IMessageComponent
    {
        public int state;
    }
}