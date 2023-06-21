using Assets.script.ComponentsAndTags;
using System.Diagnostics;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;


namespace Assets.script.Systems
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateAfter(typeof(SimulationSystemGroup))]
    public partial struct CollideSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EnemyComponent>();
            state.RequireForUpdate<PlayerComponent>();
            state.RequireForUpdate<BulletComponent>();

        }

        public partial struct JobCheckCollide : ITriggerEventsJob
        {
            public ComponentLookup<EnemyComponent> enemyLookup;
            public ComponentLookup<BulletComponent> bulletLookup;
            public EntityCommandBuffer ecb;

            private bool IsEnemy(Entity e)
            {
                return enemyLookup.HasComponent(e);
            }

            private bool IsBullet(Entity e)
            {
                return bulletLookup.HasComponent(e);
            }

            public void Execute(TriggerEvent triggerEvent)
            {
                UnityEngine.Debug.Log($"Trigger Event Happened {triggerEvent.EntityA} {triggerEvent.EntityB}");
                var isEnemyA = IsEnemy(triggerEvent.EntityA);
                var isBulletA = IsBullet(triggerEvent.EntityA);

                var isEnemyB = IsEnemy(triggerEvent.EntityB);
                var isBulletB = IsBullet(triggerEvent.EntityB);

                var validA = (isEnemyA != isBulletA);
                if (!validA)
                {
                    return;
                }

                var validB = (isEnemyB != isBulletB);
                if (!validB)
                {
                    return;
                }

                var v = (isEnemyA == isBulletB) || (isBulletA == isEnemyB);
                if (!v)
                {
                    return;
                }


                //addtag(hitted
                var destroyableA = false;
                var destroyableB = false;
                if (enemyLookup.HasComponent(triggerEvent.EntityA))
                {
                    if (enemyLookup.IsComponentEnabled(triggerEvent.EntityA))
                    {
                        ecb.SetComponentEnabled<EnemyComponent>(triggerEvent.EntityA, false);
                        destroyableA = true;
                    }
                    //a is enemy
                }
                else if (bulletLookup.HasComponent(triggerEvent.EntityA))
                {
                    if (bulletLookup.IsComponentEnabled(triggerEvent.EntityA))
                    {
                        //a is bullet
                        ecb.SetComponentEnabled<BulletComponent>(triggerEvent.EntityA, false);
                        destroyableA = true;
                    }
                }

                if (enemyLookup.HasComponent(triggerEvent.EntityB))
                {
                    if (enemyLookup.IsComponentEnabled(triggerEvent.EntityB))
                    {
                        ecb.SetComponentEnabled<EnemyComponent>(triggerEvent.EntityB, false);
                        destroyableB = true;
                    }
                    //b is enemy
                }
                else if (bulletLookup.HasComponent(triggerEvent.EntityB))
                {
                    if (bulletLookup.IsComponentEnabled(triggerEvent.EntityB))
                    {
                        ecb.SetComponentEnabled<BulletComponent>(triggerEvent.EntityB, false);
                        destroyableB = true;
                    }
                    //b is bullet
                }

                if (destroyableA)
                {
                    ecb.AddComponent<DestroyComponent>(triggerEvent.EntityA);
                }

                if (destroyableB)
                {
                    ecb.AddComponent<DestroyComponent>(triggerEvent.EntityB);
                }
            }
        }

        public void Update(ref SystemState state) {
            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.TempJob);
            state.Dependency = new JobCheckCollide
            {
                ecb = ecb,
                enemyLookup = SystemAPI.GetComponentLookup<EnemyComponent>(),
                bulletLookup = SystemAPI.GetComponentLookup<BulletComponent>(),
            }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);

            state.Dependency.Complete();
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}