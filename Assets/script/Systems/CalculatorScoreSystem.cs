using Assets.script.ComponentsAndTags;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

[BurstCompile]
public partial struct CalculatorScoreSystem : ISystem
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

        var ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach (var (addScoreComponent, entity) in SystemAPI.Query<RefRO<AddScoreComponent>>().WithEntityAccess())
        {
            foreach (var scoreComponent in SystemAPI.Query<RefRW<ScoreComponent>>())
            {
                scoreComponent.ValueRW.score += addScoreComponent.ValueRO.score;
            }
            ecb.DestroyEntity(entity);
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}