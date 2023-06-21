using Unity.Entities;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace Assets.script.ComponentsAndTags
{
    public struct SpawnPlayer : IComponentData
    {
        public Entity PlayerPrefab;
        public float3 position;
        public bool isSpawn;
    }

    public struct PlayerComponent: IComponentData
    {

    }

    public struct PlayerMove : IComponentData
    {
        public float speed;
    }

    public struct PlayerMoveRange : IComponentData
    {
        public float minHorizontal;
        public float maxHorizontal;
        public float minVertical;
        public float maxVertical;
    }

    public struct PlayerCannon : IComponentData
    {

    }
}