using Assets.script.ComponentsAndTags;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;

[BurstCompile]
[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(SimulationSystemGroup))]
public partial struct BulletCollideSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<EnemyComponent>();
        state.RequireForUpdate<BulletComponent>();
        state.RequireForUpdate<PlayerComponent>();
        state.RequireForUpdate<SimulationSingleton>();

    }

    [BurstCompile]
    public partial struct JobCheckCollide : ITriggerEventsJob
    {
        public ComponentLookup<EnemyComponent> enemyLookup;
        public ComponentLookup<BulletComponent> bulletLookup;
        public ComponentLookup<PlayerComponent> playerLookup;
        public EntityCommandBuffer ecb;

        private bool IsEnemy(Entity e)
        {
            return enemyLookup.HasComponent(e);
        }

        private bool IsBullet(Entity e)
        {
            return bulletLookup.HasComponent(e);
        }

        private bool IsPlayer(Entity e)
        {
            return playerLookup.HasComponent(e);
        }

        public void Execute(TriggerEvent triggerEvent)
        {
            var isEnemyA = IsEnemy(triggerEvent.EntityA);
            var isBulletA = IsBullet(triggerEvent.EntityA);
            var isPlayerA = IsPlayer(triggerEvent.EntityA);

            var isEnemyB = IsEnemy(triggerEvent.EntityB);
            var isBulletB = IsBullet(triggerEvent.EntityB);
            var isPlayerB = IsPlayer(triggerEvent.EntityB);

            if (isBulletA == isBulletB)
            {
                return;
            }

            if (isEnemyA == isEnemyB && isPlayerA == isPlayerB)
            {
                return;
            }

            var newEntity = ecb.CreateEntity();
            if (isBulletA)
            {
                ecb.AddComponent(newEntity,
                    new BulletDamageComponent
                    {
                        bullet = triggerEvent.EntityA,
                        target = triggerEvent.EntityB
                    });
            }
            else
            {
                ecb.AddComponent(newEntity,
                    new BulletDamageComponent
                    {
                        bullet = triggerEvent.EntityB,
                        target = triggerEvent.EntityA
                    });
            }
        }
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
        state.Dependency = new JobCheckCollide
        {
            ecb = ecb,
            enemyLookup = SystemAPI.GetComponentLookup<EnemyComponent>(),
            bulletLookup = SystemAPI.GetComponentLookup<BulletComponent>(),
            playerLookup = SystemAPI.GetComponentLookup<PlayerComponent>(),
        }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);

        state.Dependency.Complete();
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}