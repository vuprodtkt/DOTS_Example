using Unity.Entities;
using UnityEditor;
using UnityEngine;

namespace Assets.script.ComponentsAndTags
{
    public partial struct HealthComponent : IComponentData
    {
        public float health;
    }
}