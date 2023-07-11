using Assets.script.ComponentsAndTags;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

[BurstCompile]
[UpdateInGroup(typeof(LateSimulationSystemGroup))]
public partial struct StateGameSystem : ISystem
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

    public void OnUpdate(ref SystemState state)
    {
        //StateGameComponent stateGameSingleton;
        //var isStateGame = SystemAPI.TryGetSingleton(out stateGameSingleton);
        //if (!isStateGame)
        //{
        //    return;
        //}

        //switch (stateGameSingleton.state)
        //{
        //    case (int)StateGame.GameLoop:
        //        break;
        //    case (int)StateGame.GameOver:
        //        break;
        //}

    }
}