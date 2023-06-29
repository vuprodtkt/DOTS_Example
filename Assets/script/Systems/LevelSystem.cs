﻿using Assets.script.ComponentsAndTags;
using System;
using Unity.Burst;
using Unity.Entities;

[BurstCompile]
public partial class LevelSystem : SystemBase
{
    public Action<int> onLevel;
    protected override void OnUpdate()
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
            if(levelComponent.ValueRO.currentLevel < levelComponent.ValueRO.maxLevel)
            {
                levelComponent.ValueRW.currentLevel = levelComponent.ValueRO.nextLevel;
                onLevel?.Invoke(levelComponent.ValueRW.currentLevel);
            }
            
        }
    }
}