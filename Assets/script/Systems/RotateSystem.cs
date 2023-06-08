using Assets.script.ComponentsAndTags;
using Unity.Burst;
using Unity.Entities;
using Unity.Entities.UniversalDelegates;
using Unity.Mathematics;
using Unity.Transforms;

namespace Assets.script.Systems
{

    public partial struct RotateSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {

        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            
            foreach (var (transform, cubeRotate) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<CubeRotate>>())
            {
                transform.ValueRW = transform.ValueRO.RotateY(cubeRotate.ValueRO.speed * SystemAPI.Time.DeltaTime);
            }
            
        }

        public void OnDestroy(ref SystemState state)
        {

        }
    }

}