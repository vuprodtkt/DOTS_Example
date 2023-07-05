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
        foreach (var stateGanmecomponent in SystemAPI.Query<RefRO<StateGameComponent>>())
        {
            if (stateGanmecomponent.ValueRO.state != 1)
            {
                return;
            }
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