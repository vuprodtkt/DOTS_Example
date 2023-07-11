using Assets.script.ComponentsAndTags;
using CortexDeveloper.ECSMessages.Service;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.script.AuthoringAndMono
{
    public class UIController : MonoBehaviour
    {
        public GameObject m_button_start;
        public GameObject m_button_pause;
        public GameObject m_text_endGame;
        public GameObject m_bg_stateGame;
        public GameObject m_button_restart;
        public GameObject m_button_home;
        public Canvas m_canvas_menu;
        public Canvas m_canvas_gameUI;

        enum StateGame
        {
            MenuGame = 0,
            GameLoop = 1,
            PauseGame = 2,
            GameOver = 3,
            WinGame = 4,
            RestartGame = 5
        }
        private StateGameManager stateGameManager;

        private static World _world;
        private SimulationSystemGroup _simulationSystemGroup;
        private LateSimulationSystemGroup _lateSimulationSystemGroup;

        private void Awake()
        {
            InitializeMessageBroadcaster();
            CreateExampleSystems();
            this.stateGameManager = new StateGameManager();
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
            //SetupUIGamePlay();
            stateGameManager.addState((int)StateGame.MenuGame, new MenuState(stateGameManager, (int)StateGame.MenuGame, _world
                                                                            , m_button_start, m_canvas_menu, m_canvas_gameUI));

            stateGameManager.addState((int)StateGame.GameLoop, new GameLoop(stateGameManager, (int)StateGame.GameLoop, _world
                                                                            , m_button_pause, m_bg_stateGame, m_text_endGame
                                                                            , m_button_restart, m_button_home, m_canvas_gameUI));

            stateGameManager.addState((int)StateGame.PauseGame, new PauseGame(stateGameManager, (int)StateGame.PauseGame, _world
                                                                            , m_button_pause, m_bg_stateGame));

            stateGameManager.addState((int)StateGame.GameOver, new GameOver(stateGameManager, (int)StateGame.GameOver, _world
                                                                            , m_button_pause, m_bg_stateGame, m_text_endGame
                                                                            , m_button_restart, m_button_home));

            stateGameManager.addState((int)StateGame.WinGame, new WinGame(stateGameManager, (int)StateGame.PauseGame, _world
                                                                            , m_button_pause, m_bg_stateGame, m_text_endGame
                                                                            , m_button_restart, m_button_home));

            stateGameManager.addState((int)StateGame.RestartGame, new RestartGame(stateGameManager, (int)StateGame.RestartGame, _world
                                                                            , m_button_pause, m_text_endGame
                                                                            , m_button_restart, m_button_home));

            stateGameManager.setCurrentState(stateGameManager.getState((int)StateGame.MenuGame));
        }

        // Update is called once per frame
        void Update()
        {
            stateGameManager.getCurrentState().Update();
        }

        //void SetupUIGamePlay()
        //{
        //    m_UI_canvas.enabled = false;
        //    m_UI_canvas.renderMode = RenderMode.ScreenSpaceCamera;
        //    m_UI_canvas.pixelPerfect = true;
        //    m_UI_canvas.scaleFactor = 1.0f;

        //}

        public void OnClickStartGame()
        {
            stateGameManager.setCurrentState(stateGameManager.getState((int)StateGame.GameLoop));
            //EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            //MessageBroadcaster.PrepareMessage().AliveForOneFrame().PostImmediate(entityManager, new StateGameMessage { state = (int)StateGame.GameLoop });
        }

        public void onClickRestartGame()
        {
            stateGameManager.setCurrentState(stateGameManager.getState((int)StateGame.RestartGame));
            //EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            //MessageBroadcaster.PrepareMessage().AliveForOneFrame().PostImmediate(entityManager, new StateGameMessage { state = (int)StateGame.RestartGame });
        }

        public void onClickHome()
        {
            stateGameManager.setCurrentState(stateGameManager.getState((int)StateGame.MenuGame));
            //EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            //MessageBroadcaster.PrepareMessage().AliveForOneFrame().PostImmediate(entityManager, new StateGameMessage { state = (int)StateGame.MenuGame });
        }

        public void OnClickPauseGame()
        {
            if(stateGameManager.getCurrentState().id == (int)StateGame.GameLoop)
            {
                stateGameManager.setCurrentState(stateGameManager.getState((int)StateGame.PauseGame));
            }else if(stateGameManager.getCurrentState().id == (int)StateGame.PauseGame)
            {
                stateGameManager.setCurrentState(stateGameManager.getState((int)StateGame.GameLoop));
            }
            else
            {
                return;
            }

            //EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            //MessageBroadcaster.PrepareMessage().AliveForOneFrame().PostImmediate(entityManager, new StateGameMessage { state = stateGameManager.getCurrentState().id });
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