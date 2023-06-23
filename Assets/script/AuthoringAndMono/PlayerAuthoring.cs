using Unity.Entities;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace Assets.script.AuthoringAndMono
{
    public class PlayerAuthoring : MonoBehaviour
    {
        public float speed = 10f;
        public float minHorizontal = -15;
        public float maxHorizontal = 15;
        public float minVertical = -9;
        public float maxVertical = 9;

    }

    public class PlayerAuthoringBake : Baker<PlayerAuthoring>
    {
        public override void Bake(PlayerAuthoring authoring)
        {
            Entity Entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(Entity, new ComponentsAndTags.PlayerMove
            {
                speed = authoring.speed,
            });
            AddComponent(Entity, new ComponentsAndTags.PlayerMoveRange
            {
                minHorizontal = authoring.minHorizontal,
                maxHorizontal = authoring.maxHorizontal,
                minVertical = authoring.minVertical,
                maxVertical = authoring.maxVertical,
            });
            AddComponent(Entity, new ComponentsAndTags.PlayerComponent
            {

            });
        }
    }

}