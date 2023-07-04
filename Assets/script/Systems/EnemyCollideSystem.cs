using Assets.script.ComponentsAndTags;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;

namespace Assets.script.Systems
{
    [BurstCompile]
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateAfter(typeof(SimulationSystemGroup))]
    public partial struct EnemyCollideSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EnemyComponent>();
            state.RequireForUpdate<PlayerComponent>();
            state.RequireForUpdate<SimulationSingleton>();

        }

        [BurstCompile]
        public partial struct JobCheckCollide : ITriggerEventsJob
        {
            public ComponentLookup<EnemyComponent> enemyLookup;
            public ComponentLookup<PlayerComponent> playerLookup;
            public EntityCommandBuffer ecb;

            private bool IsEnemy(Entity e)
            {
                return enemyLookup.HasComponent(e);
            }

            private bool IsPlayer(Entity e)
            {
                return playerLookup.HasComponent(e);
            }

            public void Execute(TriggerEvent triggerEvent)
            {
                var isEnemyA = IsEnemy(triggerEvent.EntityA);
                var isPlayerA = IsPlayer(triggerEvent.EntityA);

                var isEnemyB = IsEnemy(triggerEvent.EntityB);
                var isPlayerB = IsPlayer(triggerEvent.EntityB);

                if(isEnemyA == isEnemyB || isPlayerA == isPlayerB)
                {
                    return;
                }

                var newEntity = ecb.CreateEntity();
                if (isEnemyA)
                {
                    ecb.AddComponent(newEntity, 
                        new EnemyCollideDamageComponent 
                        { 
                            enemy = triggerEvent.EntityA, 
                            target = triggerEvent.EntityB 
                        });
                }
                else
                {
                    ecb.AddComponent(newEntity,
                        new EnemyCollideDamageComponent
                        {
                            enemy = triggerEvent.EntityB,
                            target = triggerEvent.EntityA
                        });
                }
            }
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {

            foreach (var stateGanmecomponent in SystemAPI.Query<RefRO<StateGameComponent>>())
            {
                if (stateGanmecomponent.ValueRO.state != 1)
                {
                    return;
                }
            }

            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.TempJob);
            state.Dependency = new JobCheckCollide
            {
                ecb = ecb,
                enemyLookup = SystemAPI.GetComponentLookup<EnemyComponent>(),
                playerLookup = SystemAPI.GetComponentLookup<PlayerComponent>(),
            }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);

            state.Dependency.Complete();
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}