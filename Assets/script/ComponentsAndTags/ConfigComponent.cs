using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;

namespace Assets.script.ComponentsAndTags
{
    public partial struct ConfigComponent : IComponentData
    {
        public FixedList128Bytes<SpawnEnemy> lstSpawnEnemyWithLevel;
        public Entity enemy1Prefab;
        public Entity enemy2Prefab;
    }
}