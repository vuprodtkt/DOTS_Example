using Assets.script.ComponentsAndTags;
using TMPro;
using Unity.Burst;
using Unity.Entities;
using Unity.Entities.UniversalDelegates;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Assets.script.Systems
{

    public partial struct MoveSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {

        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            
            foreach (var (transform, cubeMove) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<CubeMove>>())
            {
                float3 currentPosition = transform.ValueRW.Position;
                float3 targetPosition = new float3(cubeMove.ValueRO.end, currentPosition.y, currentPosition.z);
                float speed = cubeMove.ValueRO.speed;

                
                float3 direction = math.normalize(targetPosition - currentPosition);
                float distanceToTarget = math.distance(currentPosition, targetPosition);

                if (distanceToTarget > 0.01f)
                {
                    float3 newPosition = currentPosition + direction * speed * SystemAPI.Time.DeltaTime;
                    transform.ValueRW.Position = newPosition;
                }
                else
                {
                    float start = cubeMove.ValueRO.start;
                    float end = cubeMove.ValueRO.end;
                    cubeMove.ValueRW.start = end;
                    cubeMove.ValueRW.end = start;
                }
            }

            foreach (var (transformComponent, playerMovingComponent) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<PlayerMoving>>())
            {
                float horizontalInput = Input.GetAxis("Horizontal");
                float3 moveDirection = new float3(0f, 0f, horizontalInput);
                float3 movement = moveDirection * playerMovingComponent.ValueRO.speed * SystemAPI.Time.DeltaTime;
                transformComponent.ValueRW.Position = transformComponent.ValueRO.Position + movement;
            }

            foreach (var (transformComponent, bulletMovingComponent) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<BulletMove>>())
            {
                float3 moveDirection = new float3(-1f, 0f, 0f);
                float3 movement = moveDirection * bulletMovingComponent.ValueRO.speed * SystemAPI.Time.DeltaTime;
                bulletMovingComponent.ValueRW.range -= bulletMovingComponent.ValueRO.speed * SystemAPI.Time.DeltaTime;
                transformComponent.ValueRW.Position = transformComponent.ValueRO.Position + movement;
            }

        }

        public void OnDestroy(ref SystemState state)
        {

        }
    }

}