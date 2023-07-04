using Assets.script.ComponentsAndTags;
using Assets.script.Systems;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

[BurstCompile]
[UpdateAfter(typeof(BulletCollideSystem))]
public partial struct EnemyCollideDamageSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<EnemyCollideDamageComponent>();
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

        foreach (var (enemyDamageComponent, entity) in SystemAPI.Query<RefRW<EnemyCollideDamageComponent>>().WithEntityAccess())
        {
            var enemyEntity = enemyDamageComponent.ValueRW.enemy;
            var targetEntity = enemyDamageComponent.ValueRW.target;

            var enemyDamage = state.EntityManager.GetComponentData<EnemyDamage>(enemyEntity);
            ComponentLookup<PlayerComponent> PlayerLookup = SystemAPI.GetComponentLookup<PlayerComponent>();

            if (PlayerLookup.HasComponent(targetEntity))
            {
                ecb.AddComponent(targetEntity, new TargetDamagedComponent { damaged = enemyDamage.damage });
            }

            ecb.DestroyEntity(entity);
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}