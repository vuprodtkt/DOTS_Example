using Assets.script.ComponentsAndTags;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

[DisableAutoCreation]
[BurstCompile]
public partial struct GetMessageSystem : ISystem
{
    public void onCreate(ref SystemState state)
    {
        state.RequireForUpdate<StateGameMessage>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var stateGameMessage in SystemAPI.Query<RefRO<StateGameMessage>>())
        {
            foreach (var stateGameComponent in SystemAPI.Query<RefRW<StateGameComponent>>())
            {
                stateGameComponent.ValueRW.state = stateGameMessage.ValueRO.state;
            }
        }

        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach (var (changeStateGameComponent, entity) in SystemAPI.Query<RefRO<ChangeStateGameComponent>>().WithEntityAccess())
        {
            ecb.DestroyEntity(entity);
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}