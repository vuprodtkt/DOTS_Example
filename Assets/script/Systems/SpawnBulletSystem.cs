using Assets.script.ComponentsAndTags;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct SpawnBulletSystem : ISystem
{

    public void OnUpdate(ref SystemState state)
    {
        StateGameComponent stateGameSingleton;
        var isStateGame = SystemAPI.TryGetSingleton(out stateGameSingleton);
        if (!isStateGame || stateGameSingleton.state != 1)
        {
            return;
        }

        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.TempJob);

        //spawn bullet
        var isPress = Input.GetKey(KeyCode.Space);
        foreach (var (localToWorldComponent, BulletSpawnComponent) in SystemAPI.Query<RefRO<LocalToWorld>, RefRW<SpawnBullet>>().WithNone<EnemyComponent>())
        {
            if(!isPress)
            {
                BulletSpawnComponent.ValueRW.lastSpawnTime = 0;
            }
            else
            {
                if (BulletSpawnComponent.ValueRO.lastSpawnTime <= 0)
                {
                    var bulletEntity = ecb.Instantiate(BulletSpawnComponent.ValueRO.BulletPrefab);
                    ecb.SetComponent(bulletEntity, 
                        new LocalTransform { 
                            Position = localToWorldComponent.ValueRO.Position, 
                            Scale = 1f, 
                            Rotation = quaternion.identity 
                        });
                    ecb.AddComponent(bulletEntity,
                        new BulletDirection
                        {
                            direction = BulletSpawnComponent.ValueRO.direction_bullet,
                        });
                    ecb.AddComponent(bulletEntity,
                        new BulletComponent
                        {
                            bulletOfPlayer = true,
                        });
                    BulletSpawnComponent.ValueRW.lastSpawnTime = BulletSpawnComponent.ValueRO.spawnSpeed;
                }
                else
                {
                    BulletSpawnComponent.ValueRW.lastSpawnTime -= SystemAPI.Time.DeltaTime;
                }
            }
        }

        new SpawnBulletEnemyJob { ECB = ecb, delta_Time = SystemAPI.Time.DeltaTime}.Schedule();

        state.Dependency.Complete();
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}

[BurstCompile]
public partial struct SpawnBulletEnemyJob : IJobEntity
{
    public EntityCommandBuffer ECB;
    public float delta_Time;

    void Execute(RefRW<SpawnBullet> spawner, RefRW<LocalToWorld> localToWorld
                , RefRO<EnemyComponent> enemyComponet, Entity entity)
    {
        if (spawner.ValueRO.lastSpawnTime >= spawner.ValueRO.spawnSpeed)
        {
            var position = new float3(1f, localToWorld.ValueRO.Position.y, localToWorld.ValueRO.Position.z);
            var bulletEntity = ECB.Instantiate(spawner.ValueRO.BulletPrefab);
            ECB.SetComponent(bulletEntity,
                new LocalTransform
                {
                    Position = position,
                    Scale = 1f,
                    Rotation = quaternion.identity
                });
            ECB.AddComponent(bulletEntity,
                new BulletDirection
                {
                    direction = spawner.ValueRO.direction_bullet,
                });
            ECB.AddComponent(bulletEntity,
                new BulletComponent
                {
                    bulletOfPlayer = false,
                });
            spawner.ValueRW.lastSpawnTime = 0f;
        }
        else
        {
            spawner.ValueRW.lastSpawnTime += delta_Time;
        }
    }
}