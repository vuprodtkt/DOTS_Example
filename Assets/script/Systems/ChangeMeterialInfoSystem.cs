using Assets.script.ComponentsAndTags;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Rendering;

[BurstCompile]
public partial struct ChangeMaterialInfoSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
        //spawn enemy
        var materialMeshinfoChange = SystemAPI.GetSingleton<MaterialMeshInfoChange>();
        foreach(var (materialMeshInfo, entity) in SystemAPI.Query<RefRW<MaterialMeshInfo>>().WithAll<PlayerComponent>().WithEntityAccess())
        {
            materialMeshInfo.ValueRW.MaterialID = materialMeshinfoChange.material1_id;
            materialMeshInfo.ValueRW.MeshID = materialMeshinfoChange.mesh0_id;
        }

    }
}