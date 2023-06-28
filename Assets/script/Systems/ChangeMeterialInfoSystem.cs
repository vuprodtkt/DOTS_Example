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
        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
        //spawn enemy
        foreach (var(materialMeshInfo, materialMeshInfoChange, tfComponent, physicsColliderComponent, entity) 
            in SystemAPI.Query<RefRW<MaterialMeshInfo> ,RefRO<MaterialMeshInfoChange>, RefRW<LocalTransform>, RefRW<PhysicsCollider>>()
                .WithAll<EnemyComponent>()
                .WithEntityAccess())
        {
            materialMeshInfo.ValueRW.MaterialID = materialMeshInfoChange.ValueRO.materialID;
            materialMeshInfo.ValueRW.MeshID = materialMeshInfoChange.ValueRO.meshID;

            tfComponent.ValueRW.Rotation = quaternion.LookRotation(new float3(0, 1, 0), new float3(-1, 0, 0));
            tfComponent.ValueRW.Position.x += 1;

            ecb.RemoveComponent<MaterialMeshInfoChange>(entity);
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();

    }
}