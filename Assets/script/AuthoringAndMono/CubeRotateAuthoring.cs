using Assets.script.ComponentsAndTags;
using System.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.script.AuthoringAndMono
{
    public class CubeRotateAuthoring : MonoBehaviour
    {
        public float move_rotate = 10f;
    }

    public class CubeRotateBaker : Baker<CubeRotateAuthoring>
    {
        public override void Bake(CubeRotateAuthoring authoring)
        {
            Entity Cube = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(Cube, new ComponentsAndTags.CubeRotate
            {
                speed = authoring.move_rotate,
            }) ;

        }
    }
}