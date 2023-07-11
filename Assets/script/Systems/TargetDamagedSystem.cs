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

    public void OnUpdate(ref SystemState state)
    {
        StateGameComponent stateGameSingleton;
        var isStateGame = SystemAPI.TryGetSingleton(out stateGameSingleton);
        if (!isStateGame || stateGameSingleton.state != 1) {
            return;
        }

        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.TempJob);

        new TargetDamageEnemyJob { ECB = ecb }.Schedule();
        new TargetDamagePlayerJob { ECB = ecb }.Schedule();

        state.Dependency.Complete();
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }

}

[BurstCompile]
public partial struct TargetDamageEnemyJob : IJobEntity
{
    public EntityCommandBuffer ECB;
    void Execute(RefRW<TargetDamagedComponent> targetDamagedComponent, RefRW<HealthComponent> healthComponent
                , RefRO<EnemyComponent> enemyComponent, Entity entity)
    {
        healthComponent.ValueRW.health -= targetDamagedComponent.ValueRO.damaged;
        ECB.RemoveComponent<TargetDamagedComponent>(entity);
        if (healthComponent.ValueRO.health <= 0)
        {
            var e = ECB.CreateEntity();
            ECB.AddComponent(e, new AddScoreComponent { score = 5 });
            ECB.AddComponent<DestroyComponent>(entity);
        }
    }
}

[BurstCompile]
public partial struct TargetDamagePlayerJob : IJobEntity
{
    public EntityCommandBuffer ECB;
    void Execute(RefRW<TargetDamagedComponent> targetDamagedComponent, RefRW<HealthComponent> healthComponent
                , RefRO<PlayerComponent> playerComponet, Entity entity)
    {
        healthComponent.ValueRW.health -= targetDamagedComponent.ValueRO.damaged;
        ECB.RemoveComponent<TargetDamagedComponent>(entity);
    }
}