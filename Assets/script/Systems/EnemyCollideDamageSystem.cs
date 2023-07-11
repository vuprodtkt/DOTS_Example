using Assets.script.ComponentsAndTags;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

[BurstCompile]
public partial struct EnemyCollideDamageSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<EnemyCollideDamageComponent>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        StateGameComponent stateGameSingleton;
        var isStateGame = SystemAPI.TryGetSingleton(out stateGameSingleton);
        if (!isStateGame || stateGameSingleton.state != 1)
        {
            return;
        }

        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.TempJob);

        new EnemyDamagePlayerJob { 
            ECB = ecb, 
            Manager = state.EntityManager, 
            PlayerLookup = SystemAPI.GetComponentLookup<PlayerComponent>() 
        }.Schedule();

        state.Dependency.Complete();
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}

[BurstCompile]
public partial struct EnemyDamagePlayerJob : IJobEntity
{
    public EntityCommandBuffer ECB;
    public EntityManager Manager;
    public ComponentLookup<PlayerComponent> PlayerLookup;

    void Execute(RefRO<EnemyCollideDamageComponent> enemyDamageComponent, Entity entity)
    {
        var enemyEntity = enemyDamageComponent.ValueRO.enemy;
        var targetEntity = enemyDamageComponent.ValueRO.target;

        var enemyDamage = Manager.GetComponentData<EnemyDamage>(enemyEntity);

        if (PlayerLookup.HasComponent(targetEntity))
        {
            ECB.AddComponent(targetEntity, new TargetDamagedComponent { damaged = enemyDamage.damage });
        }

        ECB.DestroyEntity(entity);
    }
}