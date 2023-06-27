using Unity.Entities;
using Unity.Mathematics;

namespace Unity.Rendering
{
    [MaterialProperty("_URPMyColor")]
    public partial struct URPMyColor : IComponentData
    {
        public float4 Value;
    }
}