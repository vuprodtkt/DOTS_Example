using Assets.script.ComponentsAndTags;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct EnemyMoveSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {

    }

    public void OnUpdate(ref SystemState state)
    {
        //move enemy
        new EnemyMoveJob { deltaTime = SystemAPI.Time.DeltaTime }.ScheduleParallel();

    }

    public void OnDestroy(ref SystemState state)
    {

    }
}

public partial struct EnemyMoveJob : IJobEntity
{
    public float deltaTime;

    void Execute(RefRW<LocalTransform> tfComponent, RefRW<EnemyMove> moveComponent, RefRO<EnemyRange> moveRangeComponent)
    {
        float3 direction = moveComponent.ValueRO.direction;
        float rangeMovement = moveComponent.ValueRO.speed * deltaTime;
        //move horizontal
        tfComponent.ValueRW.Position += rangeMovement * direction;
        if (tfComponent.ValueRO.Position.z + rangeMovement > moveRangeComponent.ValueRO.maxHorizontal)
        {
            tfComponent.ValueRW.Position.z = moveRangeComponent.ValueRO.minHorizontal;
        }

        //move vertical
        if (moveComponent.ValueRO.timeMoveVertical < moveComponent.ValueRO.maxTimeMoveVertical)
        {
            moveComponent.ValueRW.timeMoveVertical += deltaTime;
        }
        else
        {
            tfComponent.ValueRW.Position.y--;
            moveComponent.ValueRW.timeMoveVertical = 0f;
        }
        if (tfComponent.ValueRO.Position.y <= moveRangeComponent.ValueRO.minVertical)
        {
            tfComponent.ValueRW.Position.y = moveRangeComponent.ValueRO.maxVertical;
        }
    }
}