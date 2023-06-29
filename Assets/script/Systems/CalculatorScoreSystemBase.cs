using Assets.script.ComponentsAndTags;
using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

[BurstCompile]
public partial class CalculatorScoreSystemBase : SystemBase
{
    public Action<int> onCalculatorScore;

    [BurstCompile]
    protected override void OnUpdate()
    {
        foreach (var stateGanmecomponent in SystemAPI.Query<RefRO<StateGameComponent>>())
        {
            if (stateGanmecomponent.ValueRO.state != 1)
            {
                return;
            }
        }

        var ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach (var (addScoreComponent, entity) in SystemAPI.Query<RefRO<AddScoreComponent>>().WithEntityAccess())
        {
            foreach (var scoreComponent in SystemAPI.Query<RefRW<ScoreComponent>>())
            {
                scoreComponent.ValueRW.score += 5;
                onCalculatorScore?.Invoke(scoreComponent.ValueRO.score);
            }
                
            ecb.DestroyEntity(entity);
        }

        ecb.Playback(EntityManager);
        ecb.Dispose();
    }
}