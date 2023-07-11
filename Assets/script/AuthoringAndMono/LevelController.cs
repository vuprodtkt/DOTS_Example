using Assets.script.ComponentsAndTags;
using System.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.script.AuthoringAndMono
{
    public class LevelController : MonoBehaviour
    {
        public Text m_text_level;

        private EntityManager entityManager;

        // Use this for initialization
        void Start()
        {
            entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        }

        // Update is called once per frame
        void Update()
        {
            LevelComponent levelComponent;
            var isLevelComponent = entityManager.CreateEntityQuery(typeof(LevelComponent)).TryGetSingleton<LevelComponent>(out levelComponent);
            if(isLevelComponent)
            {
                writeLevel(levelComponent.currentLevel);
            }
        }

        private void writeLevel(int level)
        {
            m_text_level.text = "Level: " + level;
        }
    }
}