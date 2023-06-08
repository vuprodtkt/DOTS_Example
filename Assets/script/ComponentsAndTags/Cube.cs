using Unity.Entities;
using Unity.Mathematics;

namespace Assets.script.ComponentsAndTags
{
    public struct CubeMove : IComponentData
    {
        public float start;
        public float end;
        public float speed;
    }

    public struct CubeRotate : IComponentData
    {
        public float speed;
    }
}