using Assets.script.ComponentsAndTags;
using Unity.Burst;
using Unity.Entities;

[DisableAutoCreation]
[BurstCompile]
public partial struct StartGameSystem : ISystem
{

    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<StartGameMessage>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var startGameMessage in SystemAPI.Query<RefRO<StartGameMessage>>())
        {
            foreach (var stateGameComponent in SystemAPI.Query<RefRW<StateGameComponent>>())
            {
                stateGameComponent.ValueRW.state = 1;
            }
        }
        //new CheckMessageJob().Schedule();
    }

    public void OnDestroy(ref SystemState state)
    {
            
    }
}

//public partial struct CheckMessageJob : IJobEntity
//{
//    public void Execute(RefRO<StartGameMessage> startGameMessage)
//    {
//        new startSpawnJob{startGameMessage = startGameMessage}.Schedule();
//    }
//}

//public partial struct startSpawnJob : IJobEntity
//{
//    public RefRO<StartGameMessage> startGameMessage;

//    public void Execute(RefRW<StateGameComponent> StateGameComponent)
//    {
//        if (startGameMessage.ValueRO.isStart)
//        {
//            StateGameComponent.ValueRW.state = 1;
//        }
//    }
//}
