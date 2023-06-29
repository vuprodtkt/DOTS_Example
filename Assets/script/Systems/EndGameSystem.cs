using Assets.script.ComponentsAndTags;
using System;
using Unity.Burst;
using Unity.Entities;

[BurstCompile]
public partial class EndGameSystem : SystemBase
{
    public Action<bool> onEndGame;

    [BurstCompile]
    protected override void OnUpdate()
    {
        foreach (var stateGameComponent in SystemAPI.Query<RefRO<StateGameComponent>>())
        {
            if(stateGameComponent.ValueRO.state == 3)
            {
                onEndGame?.Invoke(false);
            }else if (stateGameComponent.ValueRO.state == 4)
            {
                onEndGame?.Invoke(true);
            }
            
        }
    }
}