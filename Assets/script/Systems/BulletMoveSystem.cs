using Assets.script.ComponentsAndTags;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct BulletMoveSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {

    }

    public void OnUpdate(ref SystemState state)
    {
        //move bullet
        new BulletMoveJob { deltaTime = SystemAPI.Time.DeltaTime }.ScheduleParallel();
    }

    public void OnDestroy(ref SystemState state)
    {

    }
}

public partial struct BulletMoveJob : IJobEntity
{
    public float deltaTime;

    void Execute(RefRW<LocalTransform> tfComponent, RefRO<BulletMove> moveComponent, RefRW<BulletRange> rangeComponent)
    {
        //float3 moveDirection = new float3(0f, 1f, 0f);
        float3 moveDirection = tfComponent.ValueRO.Up();
        tfComponent.ValueRW.Position += moveDirection * moveComponent.ValueRO.speed * deltaTime;
        rangeComponent.ValueRW.range -= moveComponent.ValueRO.speed * deltaTime;
    }
}