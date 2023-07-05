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
    public void OnUpdate(ref SystemState state)
    {
        var stateGameComponent = SystemAPI.GetSingleton<StateGameComponent>();
        if(stateGameComponent.state != 1)
        {
            return;
        }

        //move bullet
        new BulletMoveJob { 
            deltaTime = SystemAPI.Time.DeltaTime 
        }.ScheduleParallel();
    }
}

[BurstCompile]
public partial struct BulletMoveJob : IJobEntity
{
    public float deltaTime;

    void Execute(RefRW<LocalTransform> tfComponent, RefRO<BulletMove> moveComponent
                ,RefRO<BulletDirection> bulletDirectionComponent, RefRW<BulletRange> rangeComponent)
    {
        float3 moveDirection = bulletDirectionComponent.ValueRO.direction;
        tfComponent.ValueRW.Position += moveDirection * moveComponent.ValueRO.speed * deltaTime;
        rangeComponent.ValueRW.range -= moveComponent.ValueRO.speed * deltaTime;
    }
}