using Assets.script.ComponentsAndTags;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

[BurstCompile]
[UpdateInGroup(typeof(LateSimulationSystemGroup))]
public partial struct StateGameSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (tfComponent, moveRangeComponent)
            in SystemAPI.Query<RefRW<LocalTransform>, RefRW<EnemyRange>>().WithAll<EnemyComponent>())
        {
            if (tfComponent.ValueRO.Position.y <= moveRangeComponent.ValueRO.minVertical)
            {
                foreach (var stateGameComponent in SystemAPI.Query<RefRW<StateGameComponent>>())
                {
                    //game over
                    stateGameComponent.ValueRW.state = 3;
                    return;
                }
            }
        }

        foreach (var levelComponent in SystemAPI.Query<RefRW<LevelComponent>>().WithAll<SpawnEnemyTag>())
        {
            if (levelComponent.ValueRW.currentLevel == levelComponent.ValueRO.maxLevel)
            {
                foreach (var stateGameComponent in SystemAPI.Query<RefRW<StateGameComponent>>())
                {
                    //win game
                    stateGameComponent.ValueRW.state = 4;
                    return;
                }
            }
        }

        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach (var (healthComponent, entity) in SystemAPI.Query<RefRW<HealthComponent>>().WithAll<PlayerComponent>().WithEntityAccess())
        {
            if (healthComponent.ValueRO.health <= 0)
            {
                ecb.AddComponent<DestroyComponent>(entity);
                foreach (var stateGameComponent in SystemAPI.Query<RefRW<StateGameComponent>>())
                {
                    //game over
                    stateGameComponent.ValueRW.state = 3;
                    return;
                }
            }
        }
        
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}