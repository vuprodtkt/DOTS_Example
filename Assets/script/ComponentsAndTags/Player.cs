using Unity.Entities;
using Unity.Mathematics;

namespace Assets.script.ComponentsAndTags
{
    public struct Player : IComponentData
    {
        public int health;
    }

    public struct PlayerMoving : IComponentData
    {
        public float speed;
    }
}