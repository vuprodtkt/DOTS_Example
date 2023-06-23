using Unity.Entities;
using UnityEditor;
using UnityEngine;

namespace Assets.script.ComponentsAndTags
{
    public partial struct TargetDamagedComponent : IComponentData
    {
        public float damaged;
    }
}