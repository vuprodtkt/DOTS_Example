using Unity.Entities;
using UnityEngine.Rendering;

namespace Assets.script.ComponentsAndTags
{
    public partial struct MaterialMeshInfoChange : IComponentData
    {
        public BatchMeshID meshID;
        public BatchMaterialID materialID;
    }
}