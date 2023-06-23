using System.Collections.Generic;
using Unity.Entities;
using Unity.Collections;
using UnityEngine;
using Assets.script.ComponentsAndTags;
using System.Linq;

namespace Assets.script.AuthoringAndMono
{
    public class ConfigAuthoring : MonoBehaviour
    {
        public List<GameObject> listPrefab;
        public GameObject enemy1Prefab;
        public GameObject enemy2Prefab;
        public List<int> listNumEnemy;
    }

    public class ConfigBake : Baker<ConfigAuthoring>
    {
        public override void Bake(ConfigAuthoring authoring)
        {
            Entity Entity = GetEntity(TransformUsageFlags.None);

            FixedList128Bytes<SpawnEnemy> lst = new FixedList128Bytes<SpawnEnemy>();
            Entity enemy1Entity = GetEntity(authoring.enemy1Prefab, TransformUsageFlags.Dynamic);
            Entity enemy2Entity = GetEntity(authoring.enemy2Prefab, TransformUsageFlags.Dynamic);
            for (int i = 0; i < authoring.listNumEnemy.Count; i++)
            {
                lst.Add(new SpawnEnemy
                {
                    //EnemyPrefab = GetEntity(authoring.listPrefab.ElementAt(i), TransformUsageFlags.Dynamic),
                    enemyPrefab = i % 2 == 0 ? enemy1Entity : enemy2Entity,
                    rowSpawnEnemy = 4,
                    totalEnemy = authoring.listNumEnemy[i]
                });
            }

            AddComponent(Entity, new ConfigComponent
            {
                lstSpawnEnemyWithLevel = lst,
                enemy1Prefab = enemy1Entity,
                enemy2Prefab = enemy2Entity,
            });
        }
    }
}