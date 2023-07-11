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
            StateGameComponent stateGameComponent;
            var isStateGameComponent = entityManager.CreateEntityQuery(typeof(StateGameComponent)).TryGetSingleton<StateGameComponent>(out stateGameComponent);
            if (!isStateGameComponent)
            {
                return;
            }
            if (stateGameComponent.state != 1 && stateGameComponent.state != 3 && stateGameComponent.state != 4)
            {
                return;
            }

            HealthComponent healthPlayerComponent; 
            var isHealthComponent = entityManager.CreateEntityQuery(typeof(HealthComponent)
                                                                        , typeof(PlayerComponent))
                                                    .TryGetSingleton<HealthComponent>(out healthPlayerComponent);
            if (!isHealthComponent)
            {
                displayHealthBar(0);
                return;
            }
            displayHealthBar((int)healthPlayerComponent.health);
        }

        private void displayHealthBar(int health)
        {
            m_healthBar_slider.value = health;
        }
    }
}