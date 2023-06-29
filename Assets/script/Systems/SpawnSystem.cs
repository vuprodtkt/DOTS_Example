using Assets.script.ComponentsAndTags;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
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

        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.TempJob);
        //spawn enemy
        var singletonComponent = SystemAPI.GetSingleton<ConfigComponent>();
        new EnemySpawnJob {ConfigComponent = singletonComponent, ECB = ecb}.Schedule();
        state.Dependency.Complete();

        //spawn player
        foreach (var spawnPlayerComponent in SystemAPI.Query<RefRW<SpawnPlayer>>())
        {
            if (!spawnPlayerComponent.ValueRO.isSpawn)
            {
                var PlayerEntity = ecb.Instantiate(spawnPlayerComponent.ValueRO.PlayerPrefab);
                ecb.SetComponent(PlayerEntity,
                    new LocalTransform
                    {
                        Position = spawnPlayerComponent.ValueRO.position
                        ,Rotation = quaternion.LookRotation(new float3(1, 0, 0), new float3(0, -1, 0))
                        ,Scale = (float)30
                    });
                spawnPlayerComponent.ValueRW.isSpawn = true;
            }
        }
        
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}

[BurstCompile]
public partial struct EnemySpawnJob : IJobEntity
{
    public EntityCommandBuffer ECB;
    public ConfigComponent ConfigComponent;

    void Execute(RefRW<LevelComponent> levelComponent, RefRO<SpawnEnemyTag> spawnEnemyTag)
    {
        if (levelComponent.ValueRO.currentLevel == levelComponent.ValueRO.nextLevel
            && levelComponent.ValueRO.currentLevel < ConfigComponent.maxLevel)
        {
            if (levelComponent.ValueRO.currentLevel == 0)
            {
                spawnLevel0(ConfigComponent.lstSpawnEnemy[0].totalEnemy
                        , levelComponent.ValueRO.currentLevel % 2 == 0 ? ConfigComponent.enemy1Prefab : ConfigComponent.enemy2Prefab);
            }else if(levelComponent.ValueRO.currentLevel == 1)
            {
                //level 1
                spawnGraphHeart(ConfigComponent.lstSpawnEnemy[0].totalEnemy
                        , levelComponent.ValueRO.currentLevel % 2 == 0 ? ConfigComponent.enemy1Prefab : ConfigComponent.enemy2Prefab);
            }
            levelComponent.ValueRW.nextLevel++;
        }
        if(levelComponent.ValueRO.maxLevel != ConfigComponent.maxLevel)
        {
            levelComponent.ValueRW.maxLevel = ConfigComponent.maxLevel;
        }
    }

    private void spawnLevel0(int totalEnemy, Entity e)
    {
        int row = 4;
        int col = (int)math.ceil((float)totalEnemy / row);

        for (var r = 0; r < row; r++)
        {
            var c = 0;
            while (totalEnemy > 0 && c < col)
            {
                float3 position = new float3(1, (float)(9 - 2 * r), (float)(-15 + 2 * c));
                var newEntity = ECB.Instantiate(e);
                ECB.AddComponent(newEntity,
                    new LocalTransform
                    {
                        Position = new float3(1, (float)(9 - 2 * r), (float)(-15 + 2 * c)),
                        Scale = 2f,
                        Rotation = quaternion.identity
                    });
                c++;
            }
        }
    }

    private void spawnGraphHeart(int totalEnemy, Entity e)
    {
        float angleStep = (2 * math.PI) / totalEnemy;
        for (var i = 0; i < totalEnemy; i++) {
            float t = i * angleStep;
            float z = 16 * math.pow(math.sin(t), 3);
            float y = 13 * math.cos(t) - 5 * math.cos(2 * t) - 2 * math.cos(3 * t) - math.cos(4 * t);

            var newEntity = ECB.Instantiate(e);
                ECB.AddComponent(newEntity,
                new LocalTransform
                {
                    Position = new float3(1, y * 5f / 16f + 4f, z * 5f / 13f - 10f),
                    Scale = 2f,
                    Rotation = quaternion.identity,
                });
        }
    }

}