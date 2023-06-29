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
        public Text m_text_score;
        public Text m_text_level;
        public Text m_text_pauseGame;
        public GameObject m_game_state;
        public GameObject m_background_pauseGame;

        private int init_score = 0;
        private int init_level = 0;
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
            var CalculatorScoreSystem = _world.GetExistingSystemManaged<CalculatorScoreSystemBase>();
            CalculatorScoreSystem.onCalculatorScore += onScore;

            var LevelSystem = _world.GetExistingSystemManaged<LevelSystem>();
            LevelSystem.onLevel += onLevel;

            var endGameSystem = _world.GetExistingSystemManaged<EndGameSystem>();
            endGameSystem.onEndGame += onEndGame;
        }

        void SetupUIGamePlay()
        {
            m_UI_canvas.enabled = false;
            m_UI_canvas.renderMode = RenderMode.ScreenSpaceCamera;
            m_UI_canvas.pixelPerfect = true;
            m_UI_canvas.scaleFactor = 1.0f;

            m_text_score.text = "Score: " + init_score;
            m_text_level.text = "Level: " + init_level;
        }

        private void onScore(int score)
        {
            m_text_score.text = "Score: " + score;
        }

        private void onLevel(int level)
        {
            m_text_level.text = "Level: " + level;
        }

        private void onEndGame(bool isWin)
        {
            if (isWin)
            {
                m_game_state.GetComponent<Text>().text = "You win!";
                m_game_state.SetActive(true);
            }
            else
            {
                m_game_state.GetComponent<Text>().text = "GameOver!";
                m_game_state.SetActive(true);
            }
            
        }

        public void OnClickStartGame()
        {
            stateGame = 1;
            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            MessageBroadcaster.PrepareMessage().AliveForOneFrame().PostImmediate(entityManager, new StateGameMessage { state = stateGame });
        }

        public void OnClickPauseGame()
        {
            if(stateGame == 1)
            {
                stateGame = 2;
                m_text_pauseGame.text = "Start";
                m_background_pauseGame.SetActive(true);
            }
            else if(stateGame == 2)
            {
                stateGame = 1;
                m_text_pauseGame.text = "Pause";
                m_background_pauseGame.SetActive(false);
            }
            
            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            MessageBroadcaster.PrepareMessage().AliveForOneFrame().PostImmediate(entityManager, new StateGameMessage { state = stateGame });
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