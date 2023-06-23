using Assets.script.ComponentsAndTags;
using Assets.script.Systems;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

[BurstCompile]
[UpdateAfter(typeof(BulletCollideSystem))]
public partial struct BulletDamageSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<BulletDamageComponent>();
    }
    
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach (var (bulletDamageComponent, entity) in SystemAPI.Query<RefRW<BulletDamageComponent>>().WithEntityAccess())
        {
            var bulletEntity = bulletDamageComponent.ValueRW.bullet;
            var targetEntity = bulletDamageComponent.ValueRW.target;

            BulletDamage damgeComponet = state.EntityManager.GetComponentData<BulletDamage>(bulletEntity);
            ecb.AddComponent(targetEntity, new TargetDamagedComponent { damaged = damgeComponet.damage });

            ecb.DestroyEntity(bulletEntity);
            ecb.DestroyEntity(entity);
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }

    public void OnDestroy(ref SystemState state)
    {

    }
}