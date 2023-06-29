using Assets.script.ComponentsAndTags;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

[BurstCompile]
public partial struct DestroySystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var stateGanmecomponent in SystemAPI.Query<RefRO<StateGameComponent>>())
        {
            if (stateGanmecomponent.ValueRO.state != 1)
            {
                return;
            }
        }

        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach (var (destroyComponent, entity) in SystemAPI.Query<RefRW<DestroyComponent>>().WithEntityAccess())
        {
            ecb.DestroyEntity(entity);
        }

        //destroy bullet out of range
        foreach (var (rangeComponent, entity) in SystemAPI.Query<RefRW<BulletRange>>().WithEntityAccess())
        {
            if (rangeComponent.ValueRO.range <= 0)
            {
                ecb.DestroyEntity(entity);
            }
        }
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}