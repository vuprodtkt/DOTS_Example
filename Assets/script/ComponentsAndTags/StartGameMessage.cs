using CortexDeveloper.ECSMessages.Components;
using Unity.Entities;

namespace Assets.script.ComponentsAndTags
{
    public partial struct StartGameMessage : IComponentData, IMessageComponent
    {
        public bool isStart;
    }
}