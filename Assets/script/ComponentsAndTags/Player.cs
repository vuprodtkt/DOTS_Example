using Unity.Entities;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace Assets.script.ComponentsAndTags
{
    public partial struct SpawnPlayer : IComponentData
    {
        public Entity PlayerPrefab;
        public float3 position;
        public bool isSpawn;
    }

    public partial struct PlayerComponent: IComponentData
    {

    }

    public partial struct PlayerMove : IComponentData
    {
        public float speed;
    }

    public partial struct PlayerMoveRange : IComponentData
    {
        public float minHorizontal;
        public float maxHorizontal;
        public float minVertical;
        public float maxVertical;
    }

    public partial struct PlayerCannon : IComponentData
    {

    }
}