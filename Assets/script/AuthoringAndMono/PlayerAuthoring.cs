using Assets.script.ComponentsAndTags;
using System.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.script.AuthoringAndMono
{
    public class PlayerAuthoring : MonoBehaviour
    {
        public int health;
        public float speed;
    }

    public class PlayerInitBaker : Baker<PlayerAuthoring>
    {
        public override void Bake(PlayerAuthoring authoring)
        {
            Entity Player = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(Player, new ComponentsAndTags.Player
            {
                health = authoring.health,
            });

        }
    }

    public class PlayerMovingBaker : Baker<PlayerAuthoring>
    {
        public override void Bake(PlayerAuthoring authoring)
        {
            Entity PlayerMoving = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(PlayerMoving, new ComponentsAndTags.PlayerMoving
            {
                speed = authoring.speed,
            });

        }
    }
}