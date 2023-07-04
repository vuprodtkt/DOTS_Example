using Assets.script.ComponentsAndTags;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.script.AuthoringAndMono
{
    public class ScoreController : MonoBehaviour
    {
        public Text m_text_score;

        private EntityManager entityManager;

        // Use this for initialization
        void Start()
        {
            entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        }

        // Update is called once per frame
        void Update()
        {
            var scoreComponent = entityManager.CreateEntityQuery(typeof(ScoreComponent)).GetSingleton<ScoreComponent>();
            writeScore(scoreComponent.score);
        }

        private void writeScore(int score)
        {
            m_text_score.text = "Score: " + score;
        }
    }
}