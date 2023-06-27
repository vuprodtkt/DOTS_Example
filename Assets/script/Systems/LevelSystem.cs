using Assets.script.ComponentsAndTags;
using CortexDeveloper.ECSMessages.Service;
using System;
using System.Linq;
using Unity.Burst;
using Unity.Entities;

[BurstCompile]
public partial class LevelSystem : SystemBase
{
    public Action<int> onLevel;
    protected override void OnUpdate()
    {
        foreach (var enemyComponent in SystemAPI.Query<RefRW<EnemyComponent>>())
        {
            return;
        }
        foreach (var levelComponent in SystemAPI.Query<RefRW<LevelComponent>>().WithAll<SpawnEnemyTag>())
        {
            levelComponent.ValueRW.currentLevel = levelComponent.ValueRO.nextLevel;
            onLevel?.Invoke(levelComponent.ValueRW.currentLevel);
        }
    }
}