using Assets.script.ComponentsAndTags;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

[BurstCompile]
[UpdateInGroup(typeof(LateSimulationSystemGroup))]
public partial struct ChangeStateGameSystem : ISystem
{
    enum StateGame
    {
        MenuGame = 0,
        GameLoop = 1,
        PauseGame = 2,
        GameOver = 3,
        WinGame = 4,
        RestartGame = 5
    }

    public void OnUpdate(ref SystemState state)
    {
        

        StateGameComponent stateGameSingleton;
        var isStateGame = SystemAPI.TryGetSingleton(out stateGameSingleton);
        if (!isStateGame)
        {
            return;
        }
        var stateGame = stateGameSingleton.state;

        foreach (var (tfComponent, moveRangeComponent)
            in SystemAPI.Query<RefRW<LocalTransform>, RefRW<EnemyRange>>().WithAll<EnemyComponent>())
        {
            if (tfComponent.ValueRO.Position.y <= moveRangeComponent.ValueRO.minVertical)
            {
                //game over
                stateGame = (int)StateGame.GameOver;
            }
        }

        foreach (var levelComponent in SystemAPI.Query<RefRW<LevelComponent>>().WithAll<SpawnEnemyTag>())
        {
            if (levelComponent.ValueRW.currentLevel == levelComponent.ValueRO.maxLevel)
            {
                stateGame = (int)StateGame.WinGame;
            }
        }

        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach (var (healthComponent, entity) in SystemAPI.Query<RefRW<HealthComponent>>().WithAll<PlayerComponent>().WithEntityAccess())
        {
            if (healthComponent.ValueRO.health <= 0)
            {
                stateGame = (int)StateGame.GameOver;
                ecb.AddComponent<DestroyComponent>(entity);
            }
        }

        if(stateGame != stateGameSingleton.state)
        {
            var newEntity = ecb.CreateEntity();
            ecb.AddComponent(newEntity, new ChangeStateGameComponent { newState = stateGame });
        }
        

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}