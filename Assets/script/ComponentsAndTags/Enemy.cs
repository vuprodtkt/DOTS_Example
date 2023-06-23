using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace Assets.script.ComponentsAndTags
{
    public partial struct SpawnEnemy : IComponentData
    {
        public Entity enemyPrefab;
        public int totalEnemy;
        public int rowSpawnEnemy;
    }

    public partial struct SpawnEnemyTag : IComponentData
    {
    }

    public partial struct EnemyComponent: IComponentData, IEnableableComponent
    {

    }

    public partial struct EnemyMove : IComponentData
    {
        public float speed;
        public float3 direction;
        public float maxTimeMoveVertical;
        public float timeMoveVertical;
    }

    public partial struct EnemyRange : IComponentData
    {
        public float minHorizontal;
        public float maxHorizontal;
        public float minVertical;
        public float maxVertical;
    }
}