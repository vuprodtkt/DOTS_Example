using Assets.script.ComponentsAndTags;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.script.AuthoringAndMono
{
    public class HealthBarController : MonoBehaviour
    {
        public Slider m_healthBar_slider;

        private EntityManager entityManager;

        // Use this for initialization
        void Start()
        {
            entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        }

        // Update is called once per frame
        void Update()
        {
            var stateGameComponent = entityManager.CreateEntityQuery(typeof(StateGameComponent))
                                                        .GetSingleton<StateGameComponent>();
            if(stateGameComponent.state == 0)
            {
                return;
            }

            var healthPlayerComponent = entityManager.CreateEntityQuery(typeof(HealthComponent)
                                                                        , typeof(PlayerComponent))
                                                    .GetSingleton<HealthComponent>();
            displayHealthBar((int)healthPlayerComponent.health);
        }

        private void displayHealthBar(int health)
        {
            m_healthBar_slider.value = health;
        }
    }
}