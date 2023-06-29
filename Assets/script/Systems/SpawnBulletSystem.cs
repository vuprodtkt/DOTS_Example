using Assets.script.ComponentsAndTags;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct SpawnBulletSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var stateGanmecomponent in SystemAPI.Query<RefRO<StateGameComponent>>())
        {
            if (stateGanmecomponent.ValueRO.state != 1)
            {
                return;
            }
        }

        //spawn bullet
        var isPress = Input.GetKey(KeyCode.Space);
        foreach (var (localToWorldComponent, BulletSpawnComponent) in SystemAPI.Query<RefRO<LocalToWorld>, RefRW<SpawnBullet>>().WithAll<PlayerCannon>())
        {
            if(!isPress)
            {
                BulletSpawnComponent.ValueRW.lastSpawnTime = 0;
            }
            else
            {
                if (BulletSpawnComponent.ValueRO.lastSpawnTime <= 0)
                {
                    var bulletEntity = state.EntityManager.Instantiate(BulletSpawnComponent.ValueRO.BulletPrefab);
                    state.EntityManager.SetComponentData(bulletEntity, 
                        new LocalTransform { Position = localToWorldComponent.ValueRO.Position, Scale = 1f, Rotation = quaternion.identity });
                    BulletSpawnComponent.ValueRW.lastSpawnTime = BulletSpawnComponent.ValueRO.spawnSpeed;
                }
                else
                {
                    BulletSpawnComponent.ValueRW.lastSpawnTime -= SystemAPI.Time.DeltaTime;
                }
            }
        }
    }
}