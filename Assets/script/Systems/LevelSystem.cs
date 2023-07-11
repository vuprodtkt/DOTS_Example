using Assets.script.ComponentsAndTags;
using Unity.Burst;
using Unity.Entities;

[BurstCompile]
[UpdateAfter(typeof(SpawnSystem))]
public partial struct LevelSystem : ISystem
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        StateGameComponent stateGameSingleton;
        var isStateGame = SystemAPI.TryGetSingleton(out stateGameSingleton);
        if (!isStateGame || stateGameSingleton.state != 1)
        {
            return;
        }

        foreach (var enemyComponent in SystemAPI.Query<RefRW<EnemyComponent>>())
        {
            return;
        }
        foreach (var levelComponent in SystemAPI.Query<RefRW<LevelComponent>>().WithAll<SpawnEnemyTag>())
        {
            if (levelComponent.ValueRO.currentLevel < levelComponent.ValueRO.maxLevel)
            {
                levelComponent.ValueRW.currentLevel = levelComponent.ValueRO.nextLevel;
            }
        }

    }
}