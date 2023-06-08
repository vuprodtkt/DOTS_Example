using Assets.script.ComponentsAndTags;
using System.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.script.AuthoringAndMono
{
    public class CubeMoveAuthoring : MonoBehaviour
    {
        public float start = -8f;
        public float end = 8f;
        public float move_speed = 20f;
    }

    public class CubeMoveBaker : Baker<CubeMoveAuthoring>
    {
        public override void Bake(CubeMoveAuthoring authoring)
        {
            Entity Cube = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(Cube, new ComponentsAndTags.CubeMove
            {
                start = authoring.start,
                end = authoring.end,
                speed = authoring.move_speed,
            }) ;

        }
    }
}