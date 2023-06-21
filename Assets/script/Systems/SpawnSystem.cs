using Assets.script.ComponentsAndTags;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct SpawnSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {

    }

    public void OnUpdate(ref SystemState state)
    {
        state.Enabled = false;
        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
        //spawn enemy
        foreach (var (localToWorldComponent, spawnEnemyComponent) in SystemAPI.Query<RefRW<LocalToWorld>, RefRW<SpawnEnemy>>())
        {
            int row = spawnEnemyComponent.ValueRO.rowSpawnEnemy;
            int col = (int)math.ceil((float)spawnEnemyComponent.ValueRO.totalEnemy / row);
            for (var r = 0; r < row; r++)
            {
                var c = 0;
                while (spawnEnemyComponent.ValueRO.totalEnemy > 0 && c < col)
                {
                    var EnemyEntity = ecb.Instantiate(spawnEnemyComponent.ValueRO.EnemyPrefab);
                    float3 position = new float3(1, (float)(9 - 1.5 * r), (float)(-15 + 2 * c));
                    ecb.SetComponent(EnemyEntity, new LocalTransform { Position = position, Scale = (float)1 });
                    ecb.AddComponent(EnemyEntity, new EnemyComponent { });
                    spawnEnemyComponent.ValueRW.totalEnemy--;
                    c++;
                }
            }
        }
        //new SpawnEnemyJob { ecb = ecb}.ScheduleParallel();
        //state.Dependency.Complete();
        
        //spawn player
        foreach (var (localToWorldComponent, spawnPlayerComponent) in SystemAPI.Query<RefRW<LocalToWorld>, RefRW<SpawnPlayer>>())
        {
            if(!spawnPlayerComponent.ValueRO.isSpawn)
            {
                var PlayerEntity = ecb.Instantiate(spawnPlayerComponent.ValueRO.PlayerPrefab);
                ecb.SetComponent(PlayerEntity, new LocalTransform { Position = spawnPlayerComponent.ValueRO.position, Scale = (float)1 });
                ecb.AddComponent(PlayerEntity, new PlayerComponent { });
                spawnPlayerComponent.ValueRW.isSpawn = true;
            }
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }

    public void OnDestroy(ref SystemState state)
    {

    }
}

public partial struct SpawnEnemyJob : IJobEntity
{
    public EntityCommandBuffer ecb;

    void Execute(ref SpawnEnemy spawner)
    {
        int row = spawner.rowSpawnEnemy;
        int col = (int)math.ceil((float)spawner.totalEnemy / row);
        for (var r = 0; r < row; r++)
        {
            var c = 0;
            while (spawner.totalEnemy > 0 && c < col)
            {
                var EnemyEntity = ecb.Instantiate(spawner.EnemyPrefab);
                float3 position = new float3(1, (float)(9 - 1.5 * r), (float)(-15 + 1.5 * c));
                ecb.SetComponent(EnemyEntity, new LocalTransform { Position = position, Scale = (float)1 });
                ecb.AddComponent(EnemyEntity, new EnemyComponent { });
                spawner.totalEnemy--;
                c++;
            }
        }
    }
}