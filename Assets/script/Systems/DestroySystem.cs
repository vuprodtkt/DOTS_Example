using Assets.script.ComponentsAndTags;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

public partial struct DestroySystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {

    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);

        //destroy bullet out of range
        foreach (var (rangeComponent, entity) in SystemAPI.Query<RefRW<BulletRange>>().WithEntityAccess())
        {
            if (rangeComponent.ValueRO.range <= 0)
            {
                ecb.DestroyEntity(entity);
            }
        }

        ecb.Playback(state.EntityManager);
    }

    public void OnDestroy(ref SystemState state)
    {

    }
}