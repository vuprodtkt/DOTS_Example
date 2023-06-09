﻿using Assets.script.ComponentsAndTags;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
public partial struct EnemyMoveSystem : ISystem
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
        //move enemy
        new EnemyMoveJob { ECB = ecb, deltaTime = SystemAPI.Time.DeltaTime }.Schedule();
        state.Dependency.Complete();

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}

[BurstCompile]
public partial struct EnemyMoveJob : IJobEntity
{
    public EntityCommandBuffer ECB;
    public float deltaTime;

    void Execute(RefRW<LocalTransform> tfComponent, RefRW<EnemyMove> moveComponent
                , RefRO<EnemyRange> moveRangeComponent, RefRW<EnemyComponent> enemyComponent, Entity e)
    {
        float3 direction = moveComponent.ValueRO.direction;
        float rangeMovement = moveComponent.ValueRO.speed * (float)0.01;
        //move horizontal
        tfComponent.ValueRW.Position += rangeMovement * direction;
        if (tfComponent.ValueRO.Position.z + rangeMovement > moveRangeComponent.ValueRO.maxHorizontal)
        {
            tfComponent.ValueRW.Position.z = moveRangeComponent.ValueRO.minHorizontal;
            if(enemyComponent.ValueRO.state != 1)
            {
                enemyComponent.ValueRW.state = 1;
                ECB.AddComponent(e, new MaterialMeshInfoChange
                {
                    materialID = enemyComponent.ValueRO.final_materialID, 
                    meshID = enemyComponent.ValueRO.final_meshID
                });
            }
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
    }
}