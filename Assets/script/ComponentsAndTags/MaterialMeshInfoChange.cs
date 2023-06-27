using Unity.Entities;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace Assets.script.ComponentsAndTags
{
    public partial struct MaterialMeshInfoChange : IComponentData
    {
        public BatchMeshID mesh0_id;
        public BatchMeshID mesh1_id;
        public BatchMaterialID material0_id;
        public BatchMaterialID material1_id;
    }
}