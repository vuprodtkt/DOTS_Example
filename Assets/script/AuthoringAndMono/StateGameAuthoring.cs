using Unity.Entities;
using UnityEngine;

namespace Assets.script.AuthoringAndMono
{
    public class StateGameAuthoring : MonoBehaviour
    {
        public int state = 0;
    }

    public class StateGameBake : Baker<StateGameAuthoring>
    {
        public override void Bake(StateGameAuthoring authoring)
        {
            Entity Entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(Entity, new ComponentsAndTags.StateGameComponent
            {
                state = authoring.state,
            });
        }
    }
}