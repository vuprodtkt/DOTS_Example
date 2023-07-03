using Assets.script.ComponentsAndTags;
using Assets.script.Systems;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

[BurstCompile]
[UpdateAfter(typeof(BulletCollideSystem))]
public partial struct BulletDamageSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<BulletDamageComponent>();
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

        foreach (var (bulletDamageComponent, entity) in SystemAPI.Query<RefRW<BulletDamageComponent>>().WithEntityAccess())
        {
            var bulletEntity = bulletDamageComponent.ValueRW.bullet;
            var targetEntity = bulletDamageComponent.ValueRW.target;

            BulletComponent bulletComponent = state.EntityManager.GetComponentData<BulletComponent>(bulletEntity);
            ComponentLookup<EnemyComponent> enemyLookup = SystemAPI.GetComponentLookup<EnemyComponent>();
            ComponentLookup<PlayerComponent> PlayerLookup = SystemAPI.GetComponentLookup<PlayerComponent>();

            BulletDamage damgeComponet = state.EntityManager.GetComponentData<BulletDamage>(bulletEntity);

            if (
                (bulletComponent.bulletOfPlayer && enemyLookup.HasComponent(targetEntity))
                || (!bulletComponent.bulletOfPlayer && PlayerLookup.HasComponent(targetEntity))
                )
            {
                ecb.AddComponent(targetEntity, new TargetDamagedComponent { damaged = damgeComponet.damage });
                ecb.DestroyEntity(bulletEntity);
            }

            ecb.DestroyEntity(entity);
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}