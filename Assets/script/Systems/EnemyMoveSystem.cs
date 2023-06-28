using Assets.script.ComponentsAndTags;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
public partial struct EnemyMoveSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {

    }

    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.TempJob);
        //move enemy
        new EnemyMoveJob { ECB = ecb, deltaTime = SystemAPI.Time.DeltaTime }.Schedule();
        state.Dependency.Complete();

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }

    public void OnDestroy(ref SystemState state)
    {

    }
}

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
        if (tfComponent.ValueRO.Position.y <= moveRangeComponent.ValueRO.minVertical)
        {
            //game over
            tfComponent.ValueRW.Position.y = moveRangeComponent.ValueRO.maxVertical;

        }
    }
}