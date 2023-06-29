using Assets.script.ComponentsAndTags;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Rendering;
using Unity.Transforms;

[BurstCompile]
public partial struct ChangeMaterialInfoSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        foreach (var stateGanmecomponent in SystemAPI.Query<RefRO<StateGameComponent>>())
        {
            if (stateGanmecomponent.ValueRO.state != 1)
            {
                return;
            }
        }

        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.TempJob);
        //spawn enemy
        new ChangeMaterialMeshJob { ECB = ecb }.Schedule();
        state.Dependency.Complete();
        ecb.Playback(state.EntityManager);
        ecb.Dispose();

    }
}

[BurstCompile]
public partial struct ChangeMaterialMeshJob : IJobEntity
{
    public EntityCommandBuffer ECB;

    void Execute(RefRW<MaterialMeshInfo> materialMeshInfo, RefRO<MaterialMeshInfoChange> materialMeshInfoChange
                , RefRW<LocalTransform> tfComponent, RefRW<PhysicsCollider> physicsColliderComponent
                , RefRO<EnemyComponent> enemyComponent, Entity e)
    {
        materialMeshInfo.ValueRW.MaterialID = materialMeshInfoChange.ValueRO.materialID;
        materialMeshInfo.ValueRW.MeshID = materialMeshInfoChange.ValueRO.meshID;

        tfComponent.ValueRW.Rotation = quaternion.LookRotation(new float3(0, 1, 0), new float3(-1, 0, 0));
        tfComponent.ValueRW.Position.x += 1;

        ECB.RemoveComponent<MaterialMeshInfoChange>(e);
    }
}