using Assets.script.ComponentsAndTags;
using CortexDeveloper.ECSMessages.Service;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.script.AuthoringAndMono
{
    public class State
    {
        public int id;
        protected StateGameManager gameManager;
        protected World m_world;

        public enum StateGame
        {
            MenuGame = 0,
            GameLoop = 1,
            PauseGame = 2,
            GameOver = 3,
            WinGame = 4,
            RestartGame = 5
        }

        public State(StateGameManager gameManager, int id, World world)
        {
            this.gameManager = gameManager;
            this.id = id;   
            this.m_world = world;
        }

        public virtual void Enter() {
            MessageBroadcaster.PrepareMessage().AliveForOneFrame().PostImmediate(m_world.EntityManager, new StateGameMessage { state = id });
        }
        public virtual void Exit() { }
        public virtual void FixedUpdate() { }
        public virtual void Update() { }
    }

    public class MenuState : State
    {
        protected GameObject m_Button_start;
        protected Canvas m_canvas_menu;
        protected Canvas m_canvas_GameUI;

        public MenuState(StateGameManager manager, int id, World world
                        , GameObject buttonStart,Canvas canvas_menu
                        , Canvas canvas_GameUI) : base(manager, id, world)
        {
            m_Button_start = buttonStart;
            m_canvas_menu = canvas_menu;
            m_canvas_GameUI = canvas_GameUI;
        }

        public override void Enter()
        {
            base.Enter();
            m_canvas_menu.enabled = true;
            m_canvas_GameUI.enabled = false;
            m_Button_start.SetActive(true);

        }

        public override void Exit()
        {
            base.Exit();
            m_canvas_menu.enabled=false;
        }

        public override void Update()
        {
            base.Update();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }
    }

    public class GameLoop : State
    {
        protected GameObject m_Button_pause;
        protected GameObject m_Bg_stateGame;
        protected GameObject m_text_endGame;
        protected GameObject m_button_restart;
        protected GameObject m_button_home;
        protected Canvas m_canvas_GameUI;

        public GameLoop(StateGameManager manager, int id, World world
                        , GameObject buttonPause, GameObject bgStateGame
                        , GameObject textEndGame, GameObject buttonRestart
                        , GameObject buttonHome
                        , Canvas canvas_GameUI) : base(manager, id, world)
        {
            m_Button_pause = buttonPause;
            m_Bg_stateGame = bgStateGame;
            m_text_endGame = textEndGame;
            m_button_restart = buttonRestart;
            m_button_home = buttonHome;
            m_canvas_GameUI = canvas_GameUI;
            
        }

        public override void Enter()
        {
            base.Enter();
            m_canvas_GameUI.enabled = true;
            m_Button_pause.GetComponentInChildren<Text>().text = "Pause";
            m_Button_pause.SetActive(true);
            m_Bg_stateGame.SetActive(false);
            m_text_endGame.SetActive(false);
            m_button_restart.SetActive(false);
            m_button_home.SetActive(false);
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Update()
        {
            base.Update();
            ChangeStateGameComponent changeStateComponent;
            var isChangeStateComponent = m_world.EntityManager.CreateEntityQuery(typeof(ChangeStateGameComponent))
                .TryGetSingleton<ChangeStateGameComponent>(out changeStateComponent);
            if (isChangeStateComponent)
            {
                this.gameManager.setCurrentState(gameManager.getState(changeStateComponent.newState));
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }
    }

    public class PauseGame : State
    {
        protected GameObject m_Button_pause;
        protected GameObject m_Bg_pause;

        public PauseGame(StateGameManager manager, int id, World world
                        , GameObject buttonPause, GameObject bgPause) : base(manager, id, world)
        {
            m_Button_pause = buttonPause;
            m_Bg_pause = bgPause;
        }

        public override void Enter()
        {
            base.Enter();
            m_Button_pause.GetComponentInChildren<Text>().text = "Start";
            m_Button_pause.SetActive(true);
            m_Bg_pause.SetActive(true);
        }

        public override void Exit()
        {
            base.Exit();
            m_Bg_pause.SetActive(false);
        }

        public override void Update()
        {
            base.Update();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }
    }

    public class GameOver : State
    {
        protected GameObject m_Button_pause;
        protected GameObject m_Bg_stateGame;
        protected GameObject m_text_endGame;
        protected GameObject m_button_restart;
        protected GameObject m_button_home;

        public GameOver(StateGameManager manager, int id, World world
                        , GameObject buttonPause, GameObject bgState
                        , GameObject textEndGame, GameObject buttonRestart
                        , GameObject buttonHome) : base(manager, id, world)
        {
            m_Button_pause = buttonPause;
            m_Bg_stateGame = bgState;
            m_text_endGame = textEndGame;
            m_button_restart = buttonRestart;
            m_button_home = buttonHome;
        }

        public override void Enter()
        {
            base.Enter();
            m_text_endGame.GetComponent<Text>().text = "GameOver!";
            m_text_endGame.SetActive(true);
            m_Bg_stateGame.SetActive(true);
            m_button_restart.SetActive(true);
            m_button_home.SetActive(true);
            m_Button_pause.SetActive(false);
        }

        public override void Exit()
        {
            base.Exit();
            m_text_endGame.GetComponent<Text>().text = "";
            m_text_endGame.SetActive(false);
            m_Bg_stateGame.SetActive(false);
            m_button_restart.SetActive(false);
            m_button_home.SetActive(false);
        }

        public override void Update()
        {
            base.Update();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }
    }

    public class WinGame : State
    {
        protected GameObject m_Button_pause;
        protected GameObject m_Bg_stateGame;
        protected GameObject m_text_endGame;
        protected GameObject m_button_restart;
        protected GameObject m_button_home;

        public WinGame(StateGameManager manager, int id, World world
                        , GameObject buttonPause, GameObject bgState
                        , GameObject textEndGame, GameObject buttonRestart
                        , GameObject buttonHome) : base(manager, id, world)
        {
            m_Button_pause = buttonPause;
            m_Bg_stateGame = bgState;
            m_text_endGame = textEndGame;
            m_button_restart = buttonRestart;
            m_button_home = buttonHome;
        }

        public override void Enter()
        {
            base.Enter();
            m_text_endGame.GetComponent<Text>().text = "You win!";
            m_text_endGame.SetActive(true);
            m_Bg_stateGame.SetActive(true);
            m_button_restart.SetActive(true);
            m_button_home.SetActive(true);
            m_Button_pause.SetActive(false);
        }

        public override void Exit()
        {
            base.Exit();
            m_text_endGame.GetComponent<Text>().text = "";
            m_text_endGame.SetActive(false);
            m_Bg_stateGame.SetActive(false);
            m_button_restart.SetActive(false);
            m_button_home.SetActive(false);
        }

        public override void Update()
        {
            base.Update();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }
    }

    public class RestartGame : State
    {
        protected GameObject m_Button_pause;
        protected GameObject m_Bg_StateGame;
        protected GameObject m_text_endGame;
        protected GameObject m_button_restart;
        protected GameObject m_button_home;

        public RestartGame(StateGameManager manager, int id, World world
                        , GameObject buttonPause, GameObject textEndGame
                        , GameObject buttonRestart, GameObject buttonHome) : base(manager, id, world)
        {
            m_Button_pause = buttonPause;
            m_text_endGame = textEndGame;
            m_button_restart = buttonRestart;
            m_button_home = buttonHome;
        }

        public override void Enter()
        {
            base.Enter();
            m_text_endGame.GetComponent<Text>().text = "";
            m_text_endGame.SetActive(false);
            m_button_restart.SetActive(false);
            m_button_home.SetActive(false);
            m_Button_pause.SetActive(true);
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Update()
        {
            base.Update();
            gameManager.setCurrentState(gameManager.getState((int)StateGame.GameLoop));
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }
    }
}