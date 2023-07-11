using CortexDeveloper.ECSMessages.Components;
using Unity.Entities;

namespace Assets.script.ComponentsAndTags
{
    public partial struct StateGameComponent : IComponentData
    {
        enum StateGame
        {
            MenuGame = 0,
            GameLoop = 1,
            PauseGame = 2,
            GameOver = 3,
            WinGame = 4,
            RestartGame = 5
        }
        public int state;
    }

    public partial struct ChangeStateGameComponent : IComponentData
    {
        public int newState;
    }
}