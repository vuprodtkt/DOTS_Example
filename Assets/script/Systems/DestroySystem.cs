using Assets.script.ComponentsAndTags;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.UniversalDelegates;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Assets.script.Systems
{

    public partial struct DestroySystem : ISystem
    {

        public void OnCreate(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            foreach (var (localToWorldComponent, BulletMoveComponent, entity) in SystemAPI.Query<RefRW<LocalToWorld>, RefRW<BulletMove>>().WithEntityAccess())
            {
                if (BulletMoveComponent.ValueRO.range <= 0)
                {
                    ecb.DestroyEntity(entity);
                }
            }
            ecb.Playback(state.EntityManager);
        }

        public void OnDestroy(ref SystemState state)
        {

        }
    }

}