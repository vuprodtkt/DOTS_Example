using Assets.script.ComponentsAndTags;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

[BurstCompile]
[UpdateInGroup(typeof(LateSimulationSystemGroup))]
public partial struct DestroySystem : ISystem
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

        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.TempJob);

        new DestroyJob { ECB = ecb }.Schedule();
        new BulletRangeJob { ECB = ecb }.Schedule();

        state.Dependency.Complete();
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}

[BurstCompile]
public partial struct BulletRangeJob : IJobEntity
{
    public EntityCommandBuffer ECB;
    void Execute(RefRO<BulletRange> rangeComponent, Entity entity)
    {
        if (rangeComponent.ValueRO.range <= 0)
        {
            ECB.DestroyEntity(entity);
        }
    }
}

[BurstCompile]
public partial struct DestroyJob : IJobEntity
{
    public EntityCommandBuffer ECB;
    void Execute(RefRO<DestroyComponent> destroyComponent, Entity entity)
    {
        ECB.DestroyEntity(entity);
    }
}