﻿using Assets.script.ComponentsAndTags;
using CortexDeveloper.ECSMessages.Service;
using System;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.script.AuthoringAndMono
{
    public class UIController : MonoBehaviour
    {
        public Canvas m_UI_canvas;
        public Text m_text_score;
        private int init_score = 0;
        public Text m_text_level;
        private int init_level = 0;

        private static World _world;
        private SimulationSystemGroup _simulationSystemGroup;
        private LateSimulationSystemGroup _lateSimulationSystemGroup;

        private void Awake()
        {
            InitializeMessageBroadcaster();
            CreateExampleSystems();
        }

        private void CreateExampleSystems()
        {
            _simulationSystemGroup.AddSystemToUpdateList(_world.CreateSystem<StartGameSystem>());
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

        public void OnClickStartGame()
        {
            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            MessageBroadcaster.PrepareMessage().AliveForOneFrame().PostImmediate(entityManager, new StartGameMessage { isStart = true });
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
            _simulationSystemGroup.RemoveSystemFromUpdateList(_world.CreateSystem<StartGameSystem>());
        }

        private void DisposeMessageBroadcaster()
        {
            MessageBroadcaster.DisposeFromWorld(_world);
        }

    }
}