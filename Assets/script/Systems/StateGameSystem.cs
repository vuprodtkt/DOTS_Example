using Assets.script.ComponentsAndTags;
using Unity.Burst;
using Unity.Entities;

[BurstCompile]
public partial struct StateGameSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        foreach (var levelComponent in SystemAPI.Query<RefRW<LevelComponent>>().WithAll<SpawnEnemyTag>())
        {
            if (levelComponent.ValueRW.currentLevel == levelComponent.ValueRO.maxLevel)
            {
                foreach (var stateGameComponent in SystemAPI.Query<RefRW<StateGameComponent>>())
                {
                    stateGameComponent.ValueRW.state = 4;
                    return;
                }
            }
        }

        foreach (var (healthComponent, entity) in SystemAPI.Query<RefRW<HealthComponent>>().WithAll<PlayerComponent>().WithEntityAccess())
        {
            if (healthComponent.ValueRO.health <= 0)
            {
                state.EntityManager.AddComponent<DestroyComponent>(entity);
                foreach (var stateGameComponent in SystemAPI.Query<RefRW<StateGameComponent>>())
                {
                    stateGameComponent.ValueRW.state = 3;
                    return;
                }
            }
        }
    }
}