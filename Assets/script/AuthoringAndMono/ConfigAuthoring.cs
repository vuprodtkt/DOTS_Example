using System.Collections.Generic;
using Unity.Entities;
using Unity.Collections;
using UnityEngine;
using Assets.script.ComponentsAndTags;

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

            var lst_entity = new FixedList4096Bytes<Entity>();
            var lst = new FixedList128Bytes<SpawnEnemy>();
            Entity enemy1Entity = GetEntity(authoring.enemy1Prefab, TransformUsageFlags.Dynamic);
            Entity enemy2Entity = GetEntity(authoring.enemy2Prefab, TransformUsageFlags.Dynamic);

            //var go0 = authoring.listPrefab[0];
            //var e = GetEntity(go0, TransformUsageFlags.Dynamic);
            //Debug.Log(e);
            //lst_entity.Add(e);

            for (int i = 0; i < authoring.listNumEnemy.Count; i++)
            {
                lst.Add(new SpawnEnemy
                {
                    //enemyPrefab = GetEntity(authoring.listPrefab[i], TransformUsageFlags.Dynamic),
                    enemyPrefab = i % 2 == 0 ? enemy1Entity : enemy2Entity,
                    rowSpawnEnemy = 4,
                    totalEnemy = authoring.listNumEnemy[i]
                });
                //lst_entity.Add(GetEntity(authoring.listPrefab[i], TransformUsageFlags.Dynamic));
            }

            AddComponent(Entity, new ConfigComponent
            {
                lstSpawnEnemy = lst,
                lstEntityEnemy = lst_entity,
                enemy1Prefab = enemy1Entity,
                enemy2Prefab = enemy2Entity,
                maxLevel = authoring.listNumEnemy.Count
            });
        }
    }
}