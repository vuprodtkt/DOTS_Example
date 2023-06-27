using Assets.script.ComponentsAndTags;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;

[BurstCompile]
public partial struct SpawnSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<StateGameComponent>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        //state.Enabled = false;
        foreach (var stateGanmecomponent in SystemAPI.Query<RefRO<StateGameComponent>>())
        {
            if(stateGanmecomponent.ValueRO.state != 1 )
            {
                return;
            }
        }

        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
        //spawn enemy
        var singletonComponent = SystemAPI.GetSingleton<ConfigComponent>();
        // spanwn using config (list component)
        foreach (var levelComponent in SystemAPI.Query<RefRW<LevelComponent>>().WithAll<SpawnEnemyTag>())
        {
            if (levelComponent.ValueRO.currentLevel == levelComponent.ValueRO.nextLevel && levelComponent.ValueRO.currentLevel < levelComponent.ValueRO.maxLevel)
            {
                //Entity e = singletonComponent.lstSpawnEnemyWithLevel.ElementAt(levelComponent.ValueRO.currentLevel).EnemyPrefab;
                Entity e = levelComponent.ValueRO.currentLevel % 2 == 0 ? singletonComponent.enemy1Prefab : singletonComponent.enemy2Prefab;
                int row = singletonComponent.lstSpawnEnemyWithLevel.ElementAt(levelComponent.ValueRO.currentLevel).rowSpawnEnemy;
                int totalEnemy = singletonComponent.lstSpawnEnemyWithLevel.ElementAt(levelComponent.ValueRO.currentLevel).totalEnemy;
                int col = (int)math.ceil((float)totalEnemy / row);
                for (var r = 0; r < row; r++)
                {
                    var c = 0;
                    while (totalEnemy > 0 && c < col)
                    {
                        var EnemyEntity = ecb.Instantiate(e);
                        float3 position = new float3(1, (float)(9 - 2 * r), (float)(-15 + 2 * c));
                        ecb.SetComponent(EnemyEntity
                            , new LocalTransform { Position = position
                                                , Scale = 2f
                                                , Rotation = quaternion.identity
                                                });
                        c++;
                    }
                }
                levelComponent.ValueRW.nextLevel++;
            }
        }

        //spawn player
        foreach (var spawnPlayerComponent in SystemAPI.Query<RefRW<SpawnPlayer>>())
        {
            if(!spawnPlayerComponent.ValueRO.isSpawn)
            {
                var PlayerEntity = ecb.Instantiate(spawnPlayerComponent.ValueRO.PlayerPrefab);
                ecb.SetComponent(PlayerEntity, new LocalTransform { Position = spawnPlayerComponent.ValueRO.position, Scale = (float)30, Rotation = quaternion.identity });
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

