using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine.Rendering;

namespace Assets.script.ComponentsAndTags
{
    public partial struct ConfigComponent : IComponentData
    {
        public FixedList4096Bytes<Entity> lstEntityEnemy;
        public FixedList128Bytes<SpawnEnemy> lstSpawnEnemy;
        public int maxLevel;
        public Entity enemy1Prefab;
        public Entity enemy2Prefab;
    }

}