using Assets.script.ComponentsAndTags;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct ControlMoveSystem : ISystem
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

        // Move Player
        float horizontalInput = Input.GetAxis("Horizontal");
        float VerticalInput = Input.GetAxis("Vertical");
        new ControlMoveJob { 
            deltaTime = SystemAPI.Time.DeltaTime, 
            horizontalInput = horizontalInput, 
            verticalInput = VerticalInput 
        }.ScheduleParallel();
    }
}

public partial struct ControlMoveJob : IJobEntity
{
    public float deltaTime;
    public float horizontalInput;
    public float verticalInput;

    void Execute(RefRW<LocalTransform> tfComponent, RefRO<PlayerMove> moveComponent, RefRO<PlayerMoveRange> moveRangeComponent)
    {
        float3 moveDirection = new float3(0f, verticalInput, horizontalInput);
        float3 movement = moveDirection * moveComponent.ValueRO.speed * deltaTime;
        tfComponent.ValueRW.Position = tfComponent.ValueRO.Position + movement;

        var minHorizontal = moveRangeComponent.ValueRO.minHorizontal;
        var maxHorizontal = moveRangeComponent.ValueRO.maxHorizontal;
        var minVertical = moveRangeComponent.ValueRO.minVertical;
        var maxVertical = moveRangeComponent.ValueRO.maxVertical;
        if (tfComponent.ValueRO.Position.y < minVertical)
        {
            tfComponent.ValueRW.Position.y = minVertical;
        }
        else if (tfComponent.ValueRO.Position.y > maxVertical)
        {
            tfComponent.ValueRW.Position.y = maxVertical;
        }
        if (tfComponent.ValueRO.Position.z < minHorizontal)
        {
            tfComponent.ValueRW.Position.z = minHorizontal;
        }
        else if (tfComponent.ValueRO.Position.z > maxHorizontal)
        {
            tfComponent.ValueRW.Position.z = maxHorizontal;
        }
    }
}