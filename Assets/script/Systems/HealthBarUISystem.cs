using Assets.script.ComponentsAndTags;
using System;
using Unity.Burst;
using Unity.Entities;

[BurstCompile]
public partial class HealthBarUISystem : SystemBase
{
    public Action<int> onDisplayHealthBar;
    protected override void OnUpdate()
    {
        foreach (var stateGanmecomponent in SystemAPI.Query<RefRO<StateGameComponent>>())
        {
            if (stateGanmecomponent.ValueRO.state != 1)
            {
                return;
            }
        }

        foreach (var healthComponent in SystemAPI.Query<RefRO<HealthComponent>>().WithAll<PlayerComponent>())
        {
            onDisplayHealthBar?.Invoke((int)healthComponent.ValueRO.health);
        }
    }
}