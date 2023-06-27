using System.Collections.Generic;
using Unity.Entities;
using Unity.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

namespace Assets.script.AuthoringAndMono
{
    public class MaterialMeshAuthoring : MonoBehaviour
    {
        public Mesh mesh0;
        public Mesh mesh1;
        public Material material0;
        public Material material1;
    }

    public class MaterialMeshChangeBake : Baker<MaterialMeshAuthoring>
    {
        public Dictionary<Mesh, BatchMeshID> m_MeshMapping = new Dictionary<Mesh, BatchMeshID>();
        public Dictionary<Material, BatchMaterialID> m_MaterialMapping = new Dictionary<Material, BatchMaterialID>();

        public override void Bake(MaterialMeshAuthoring authoring)
        {
            Entity Entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(Entity, new ComponentsAndTags.MaterialMeshInfoChange
            {
                mesh0_id = registerMesh(authoring.mesh0),
                mesh1_id = registerMesh(authoring.mesh1),
                material0_id = registerMaterial(authoring.material0),
                material1_id = registerMaterial(authoring.material1),
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