using Assets.script.ComponentsAndTags;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

[BurstCompile]
public partial struct CalculatorScoreSystem : ISystem
{

    public void OnUpdate(ref SystemState state)
    {
        StateGameComponent stateGameSingleton;
        var isStateGame = SystemAPI.TryGetSingleton(out stateGameSingleton);
        if (!isStateGame || stateGameSingleton.state != 1)
        {
            return;
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