using Assets.script.ComponentsAndTags;
using CortexDeveloper.ECSMessages.Service;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.script.AuthoringAndMono
{
    public class UIController : MonoBehaviour
    {
        public Canvas m_UI_canvas;
        public GameObject m_pauseGame_button;
        public GameObject m_game_state;
        public GameObject m_background_pauseGame;
        public GameObject m_restart_button;
        public GameObject m_home_button;

        // 0: menu game
        // 1: game loop
        // 2: pause game
        // 3: game over
        // 4: win game
        private int stateGame;

        private static World _world;
        private SimulationSystemGroup _simulationSystemGroup;
        private LateSimulationSystemGroup _lateSimulationSystemGroup;

        private void Awake()
        {
            InitializeMessageBroadcaster();
            CreateExampleSystems();
            stateGame = 0;
        }

        private void CreateExampleSystems()
        {
            _simulationSystemGroup.AddSystemToUpdateList(_world.CreateSystem<GetMessageSystem>());
        }

        private void InitializeMessageBroadcaster()
        {
            _world = World.DefaultGameObjectInjectionWorld;
            _simulationSystemGroup = _world.GetOrCreateSystemManaged<SimulationSystemGroup>();
            _lateSimulationSystemGroup = _world.GetOrCreateSystemManaged<LateSimulationSystemGroup>();

            MessageBroadcaster.InitializeInWorld(_world, _lateSimulationSystemGroup);
        }

        // Use this for initialization
        void Start()
        {
            SetupUIGamePlay();
        }

        // Update is called once per frame
        void Update()
        {
            if(stateGame == 0)
            {
                return;
            }
            var stateGameComponent = _world.EntityManager.CreateEntityQuery(typeof(StateGameComponent))
                                                        .GetSingleton<StateGameComponent>();
            onStateGame(stateGameComponent.state);
        }

        void SetupUIGamePlay()
        {
            m_UI_canvas.enabled = false;
            m_UI_canvas.renderMode = RenderMode.ScreenSpaceCamera;
            m_UI_canvas.pixelPerfect = true;
            m_UI_canvas.scaleFactor = 1.0f;

        }

        private void onStateGame(int state)
        {
            stateGame = state != 0 ? state : stateGame;
            switch (state)
            {
                case 0:
                    break;
                case 1:
                    m_pauseGame_button.GetComponentInChildren<Text>().text = "Pause";

                    m_background_pauseGame.SetActive(false);
                    m_pauseGame_button.SetActive(true);

                    m_game_state.SetActive(false);
                    m_restart_button.SetActive(false);
                    m_home_button.SetActive(false);
                    break;
                case 2:
                    m_pauseGame_button.GetComponentInChildren<Text>().text = "Start";

                    m_background_pauseGame.SetActive(true);
                    m_pauseGame_button.SetActive(true);

                    m_game_state.SetActive(false);
                    m_restart_button.SetActive(false);
                    m_home_button.SetActive(false);
                    break;
                case 3:
                    m_game_state.GetComponent<Text>().text = "GameOver!";

                    m_game_state.SetActive(true);
                    m_restart_button.SetActive(true);
                    m_home_button.SetActive(true);

                    m_pauseGame_button.SetActive(false);
                    break;
                case 4:
                    m_game_state.GetComponent<Text>().text = "You win!";

                    m_game_state.SetActive(true);
                    m_restart_button.SetActive(true);
                    m_home_button.SetActive(true);

                    m_pauseGame_button.SetActive(false);
                    break;
            }
        }

        public void OnClickStartGame()
        {
            stateGame = 1;
            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            MessageBroadcaster.PrepareMessage().AliveForOneFrame().PostImmediate(entityManager, new StateGameMessage { state = stateGame });
        }

        public void onClickRestartGame()
        {
            stateGame = 5;
            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            MessageBroadcaster.PrepareMessage().AliveForOneFrame().PostImmediate(entityManager, new StateGameMessage { state = stateGame });
        }

        public void onClickHome()
        {
            stateGame = 0;
            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            MessageBroadcaster.PrepareMessage().AliveForOneFrame().PostImmediate(entityManager, new StateGameMessage { state = stateGame });
        }

        public void OnClickPauseGame()
        {
            var stateMessage = 2;
            if(stateGame == 1)
            {
                stateMessage = 2;
            }else if(stateGame == 2)
            {
                stateMessage = 1;
            }
            else
            {
                return;
            }

            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            MessageBroadcaster.PrepareMessage().AliveForOneFrame().PostImmediate(entityManager, new StateGameMessage { state = stateMessage });
        }

        private void OnDestroy()
        {
            if (!_world.IsCreated)
            {
                return;
            }

            DisposeMessageBroadcaster();
            RemoveExampleSystem();
        }

        private void RemoveExampleSystem()
        {
            _simulationSystemGroup.RemoveSystemFromUpdateList(_world.CreateSystem<GetMessageSystem>());
        }

        private void DisposeMessageBroadcaster()
        {
            MessageBroadcaster.DisposeFromWorld(_world);
        }

    }
}