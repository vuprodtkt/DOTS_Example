using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

namespace Assets.script.AuthoringAndMono
{
    public class EnemyAuthoring : MonoBehaviour
    {
        public float speed = 10f;
        public float maxTimeMoveVertical = 10f;
        public float minHorizontal = -15;
        public float maxHorizontal = 15;
        public float minVertical = -9;
        public float maxVertical = 9;

        public Mesh finalMeshEnemy;
        public Material finalMaterialEnemy;
    }

    public class EnemyAuthoringBake : Baker<EnemyAuthoring>
    {
        public Dictionary<Mesh, BatchMeshID> m_MeshMapping = new Dictionary<Mesh, BatchMeshID>();
        public Dictionary<Material, BatchMaterialID> m_MaterialMapping = new Dictionary<Material, BatchMaterialID>();

        public override void Bake(EnemyAuthoring authoring)
        {
            Entity Entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(Entity, new ComponentsAndTags.EnemyMove
            {
                speed = authoring.speed,
                direction = new float3(0,0,1f),
                maxTimeMoveVertical = authoring.maxTimeMoveVertical,
                timeMoveVertical = 0f,
            });
            AddComponent(Entity, new ComponentsAndTags.EnemyRange
            {
                minHorizontal = authoring.minHorizontal,
                maxHorizontal = authoring.maxHorizontal,
                minVertical = authoring.minVertical,
                maxVertical = authoring.maxVertical,
            });
            AddComponent(Entity, new ComponentsAndTags.EnemyComponent
            {
                state = 0,
                final_meshID = registerMesh(authoring.finalMeshEnemy),
                final_materialID = registerMaterial(authoring.finalMaterialEnemy),
            });
        }

        public BatchMeshID registerMesh(Mesh mesh)
        {
            var entitiesGraphicsSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<EntitiesGraphicsSystem>();
            if (!m_MeshMapping.ContainsKey(mesh))
            {
                m_MeshMapping.Add(mesh, entitiesGraphicsSystem.RegisterMesh(mesh));
            }
            return m_MeshMapping[mesh];
        }

        public BatchMaterialID registerMaterial(Material material)
        {
            var entitiesGraphicsSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<EntitiesGraphicsSystem>();
            if (!m_MaterialMapping.ContainsKey(material))
            {
                m_MaterialMapping.Add(material, entitiesGraphicsSystem.RegisterMaterial(material));
            }
            return m_MaterialMapping[material];
        }
    }
}