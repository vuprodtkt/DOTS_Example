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
            var levelComponent = entityManager.CreateEntityQuery(typeof(LevelComponent)).GetSingleton<LevelComponent>();
            writeLevel(levelComponent.currentLevel);
        }

        private void writeLevel(int level)
        {
            m_text_level.text = "Level: " + level;
        }
    }
}