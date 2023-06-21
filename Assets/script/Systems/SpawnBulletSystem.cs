using Assets.script.ComponentsAndTags;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct SpawnBulletSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {

    }

    public void OnUpdate(ref SystemState state)
    {
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
                        new LocalTransform { Position = localToWorldComponent.ValueRO.Position, Scale = (float)1 });
                    BulletSpawnComponent.ValueRW.lastSpawnTime = BulletSpawnComponent.ValueRO.spawnSpeed;
                }
                else
                {
                    BulletSpawnComponent.ValueRW.lastSpawnTime -= SystemAPI.Time.DeltaTime;
                }
            }
        }
    }

    public void OnDestroy(ref SystemState state)
    {

    }
}