using Assets.script.ComponentsAndTags;
using Assets.script.Systems;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

[BurstCompile]
public partial struct TargetDamagedSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<TargetDamagedComponent>();
    }
    
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach (var (targetDamagedComponent, healthComponent ,entity) in SystemAPI.Query<RefRW<TargetDamagedComponent>, RefRW<HealthComponent>>().WithEntityAccess())
        {
            healthComponent.ValueRW.health -= targetDamagedComponent.ValueRO.damaged;
            ecb.RemoveComponent<TargetDamagedComponent>(entity);
            if(healthComponent.ValueRO.health <= 0)
            {
                ecb.AddComponent<DestroyComponent>(entity);
            }
        }
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }

    public void OnDestroy(ref SystemState state)
    {

    }
}