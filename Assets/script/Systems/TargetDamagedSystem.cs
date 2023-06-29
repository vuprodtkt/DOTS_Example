using Assets.script.ComponentsAndTags;
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

        foreach (var (targetDamagedComponent, healthComponent ,entity) 
            in SystemAPI.Query<RefRW<TargetDamagedComponent>, RefRW<HealthComponent>>()
            .WithAll<EnemyComponent>()
            .WithEntityAccess())
        {
            healthComponent.ValueRW.health -= targetDamagedComponent.ValueRO.damaged;
            ecb.RemoveComponent<TargetDamagedComponent>(entity);
            if(healthComponent.ValueRO.health <= 0)
            {
                var e = ecb.CreateEntity();
                ecb.AddComponent(e, new AddScoreComponent { score = 5 });
                ecb.AddComponent<DestroyComponent>(entity);
            }
        }

        foreach (var (targetDamagedComponent, healthComponent, entity) 
            in SystemAPI.Query<RefRW<TargetDamagedComponent>, RefRW<HealthComponent>>()
            .WithAll<PlayerComponent>()
            .WithEntityAccess())
        {
            healthComponent.ValueRW.health -= targetDamagedComponent.ValueRO.damaged;
            ecb.RemoveComponent<TargetDamagedComponent>(entity);
        }
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }

}