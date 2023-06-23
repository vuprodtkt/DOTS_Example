using Assets.script.ComponentsAndTags;
using System.Linq;
using Unity.Burst;
using Unity.Entities;

[BurstCompile]
public partial struct LevelSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {

    }
    
    public void OnUpdate(ref SystemState state)
    {
        foreach (var enemyComponent in SystemAPI.Query<RefRW<EnemyComponent>>())
        {
            return;
        }
        foreach (var levelComponent in SystemAPI.Query<RefRW<LevelComponent>>().WithAll<SpawnEnemyTag>())
        {
            //int lstEnemyComponent = SystemAPI.Query<EnemyComponent>().Count();
            //if (lstEnemyComponent <= 0)
            //{
            //    levelComponent.ValueRW.currentLevel = levelComponent.ValueRO.nextLevel;
            //}
            levelComponent.ValueRW.currentLevel = levelComponent.ValueRO.nextLevel;
        }
    }

    public void OnDestroy(ref SystemState state)
    {

    }
}