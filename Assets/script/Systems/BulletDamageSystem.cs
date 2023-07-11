using Assets.script.ComponentsAndTags;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

[BurstCompile]
public partial struct BulletDamageSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<BulletDamageComponent>();
    }

    public void OnUpdate(ref SystemState state)
    {
        StateGameComponent stateGameSingleton;
        var isStateGame = SystemAPI.TryGetSingleton(out stateGameSingleton);
        if (!isStateGame || stateGameSingleton.state != 1)
        {
            return;
        }

        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.TempJob);
        state.Dependency =  new BulletDamageJob { ECB = ecb 
                                                , EM = state.EntityManager
                                                , enemyLookup = SystemAPI.GetComponentLookup<EnemyComponent>()
                                                , PlayerLookup = SystemAPI.GetComponentLookup<PlayerComponent>()
                                                }.Schedule(state.Dependency);

        state.Dependency.Complete();
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}

[BurstCompile]
public partial struct BulletDamageJob : IJobEntity
{
    public EntityCommandBuffer ECB;
    public EntityManager EM;
    public ComponentLookup<EnemyComponent> enemyLookup;
    public ComponentLookup<PlayerComponent> PlayerLookup;

    void Execute(RefRW<BulletDamageComponent> bulletDamageComponent, Entity entity)
    {
        var bulletEntity = bulletDamageComponent.ValueRW.bullet;
        var targetEntity = bulletDamageComponent.ValueRW.target;

        BulletComponent bulletComponent = EM.GetComponentData<BulletComponent>(bulletEntity);
        BulletDamage damgeComponet = EM.GetComponentData<BulletDamage>(bulletEntity);

        if (
            (bulletComponent.bulletOfPlayer && enemyLookup.HasComponent(targetEntity))
            || (!bulletComponent.bulletOfPlayer && PlayerLookup.HasComponent(targetEntity))
            )
        {
            ECB.AddComponent(targetEntity, new TargetDamagedComponent { damaged = damgeComponet.damage });
            ECB.DestroyEntity(bulletEntity);
        }
        ECB.DestroyEntity(entity);
    }
}