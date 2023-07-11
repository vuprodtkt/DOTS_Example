using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.script.AuthoringAndMono
{
    public class StateGameManager
    {
        protected Dictionary<int, State> states;
        protected State mCurrentState;

        public StateGameManager()
        {
            this.states = new Dictionary<int, State>();
            this.mCurrentState = null;
        }

        public StateGameManager(Dictionary<int, State> states, State mCurrentState)
        {
            this.states = states;
            this.mCurrentState = mCurrentState;
        }
        
        public void addState(State state)
        {
            states.Add(state.id, state);
        }

        public void addState(int id, State state)
        {
            states.Add(id, state);
        }

        public State getState(int id)
        {
            return states[id];
        }

        public void setCurrentState(State state)
        {
            if(mCurrentState != null)
            {
                mCurrentState.Exit();
            }
            mCurrentState = state;
            if(mCurrentState != null)
            {
                mCurrentState.Enter();
            }
        }

        public State getCurrentState()
        {
            return mCurrentState;
        }

        public void Update()
        {
            if (mCurrentState != null)
            {
                mCurrentState.Update();
            }
        }

        public void FixedUpdate()
        {
            if (mCurrentState != null)
            {
                mCurrentState.FixedUpdate();
            }
        }
    }
}