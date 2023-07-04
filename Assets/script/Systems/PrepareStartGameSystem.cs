using Assets.script.ComponentsAndTags;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

[BurstCompile]
[UpdateBefore(typeof(SpawnSystem))]
public partial struct PrepareStartGameSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<StateGameComponent>();
    }

    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.TempJob);

        foreach (var stateGameComponent in SystemAPI.Query<RefRW<StateGameComponent>>())
        {
            if (stateGameComponent.ValueRO.state == 0 || stateGameComponent.ValueRO.state == 5)
            {
                // prepare start game
                foreach (var (levelComponent, scoreComponent, spawnerPlayer) 
                    in SystemAPI.Query<RefRW<LevelComponent>, RefRW<ScoreComponent>, RefRW<SpawnPlayer>>()
                                .WithAll<SpawnEnemyTag>())
                {
                    //reset level
                    levelComponent.ValueRW.currentLevel = 0;
                    levelComponent.ValueRW.nextLevel = 0;
                    //reset score
                    scoreComponent.ValueRW.score = 0;
                    //reset spawner player
                    spawnerPlayer.ValueRW.isSpawn = false;
                }
                new DestroyAllEnemyJob { ECB = ecb }.Schedule();
                new DestroyPlayerJob { ECB = ecb }.Schedule();
                new DestroyAllBulletJob { ECB = ecb }.Schedule();

                //restart game
                if (stateGameComponent.ValueRO.state == 5)
                {
                    stateGameComponent.ValueRW.state = 1;
                }
            }
        }

        state.Dependency.Complete();
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}

[BurstCompile]
public partial struct DestroyAllEnemyJob : IJobEntity
{
    public EntityCommandBuffer ECB;
    void Execute(RefRO<EnemyComponent> enemyComponent, Entity e)
    {
        ECB.DestroyEntity(e);
    }
}

[BurstCompile]
public partial struct DestroyPlayerJob : IJobEntity
{
    public EntityCommandBuffer ECB;
    void Execute(RefRO<PlayerComponent> playerComponent, Entity e)
    {
        ECB.DestroyEntity(e);
    }
}

[BurstCompile]
public partial struct DestroyAllBulletJob : IJobEntity
{
    public EntityCommandBuffer ECB;
    void Execute(RefRO<BulletComponent> bulletComponent, Entity e)
    {
        ECB.DestroyEntity(e);
    }
}