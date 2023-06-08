using Assets.script.ComponentsAndTags;
using Unity.Burst;
using Unity.Entities;
using Unity.Entities.UniversalDelegates;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Assets.script.Systems
{

    public partial struct SpawnSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {

        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            ComponentLookup<LocalToWorld> localToWorldLookup = SystemAPI.GetComponentLookup<LocalToWorld>();
            if (Input.GetKeyDown(KeyCode.R))
            {
                foreach (var (localToWorldComponent, BulletSpawnComponent) in SystemAPI.Query<RefRW<LocalToWorld>, RefRW<BulletSpawn>>())
                {
                    var entity = state.EntityManager.Instantiate(BulletSpawnComponent.ValueRO.BulletPrefab);
                    state.EntityManager.SetComponentData(entity, new LocalTransform { Position = localToWorldComponent.ValueRO.Position, Scale = (float)1 });
                }
            }

        }

        public void OnDestroy(ref SystemState state)
        {

        }
    }

}