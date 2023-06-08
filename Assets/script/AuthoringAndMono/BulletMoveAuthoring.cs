using Assets.script.ComponentsAndTags;
using System.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.script.AuthoringAndMono
{
    public class BulletMoveẠuthoring : MonoBehaviour
    {
        public float speed;
        public float range;
    }

    public class BulletMoveBaker : Baker<BulletMoveẠuthoring>
    {
        public override void Bake(BulletMoveẠuthoring authoring)
        {
            Entity BulletSpawnEntity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(BulletSpawnEntity, new ComponentsAndTags.BulletMove
            {
                speed = authoring.speed,
                range = authoring.range,
            });
        }
    }
}