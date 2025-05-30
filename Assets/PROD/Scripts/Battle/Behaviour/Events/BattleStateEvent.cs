using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/BattleStateEvent")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "BattleStateEvent", message: "Battle state has changed to [newstate]", category: "Events", id: "a8287ae8433747e771dee400ba9b5955")]
public partial class BattleStateEvent : EventChannelBase
{
    public delegate void BattleStateEventEventHandler(BattleState newstate);
    public event BattleStateEventEventHandler Event; 

    public void SendEventMessage(BattleState newstate)
    {
        Event?.Invoke(newstate);
    }

    public override void SendEventMessage(BlackboardVariable[] messageData)
    {
        BlackboardVariable<BattleState> newstateBlackboardVariable = messageData[0] as BlackboardVariable<BattleState>;
        var newstate = newstateBlackboardVariable != null ? newstateBlackboardVariable.Value : default(BattleState);

        Event?.Invoke(newstate);
    }

    public override Delegate CreateEventHandler(BlackboardVariable[] vars, System.Action callback)
    {
        BattleStateEventEventHandler del = (newstate) =>
        {
            BlackboardVariable<BattleState> var0 = vars[0] as BlackboardVariable<BattleState>;
            if(var0 != null)
                var0.Value = newstate;

            callback();
        };
        return del;
    }

    public override void RegisterListener(Delegate del)
    {
        Event += del as BattleStateEventEventHandler;
    }

    public override void UnregisterListener(Delegate del)
    {
        Event -= del as BattleStateEventEventHandler;
    }
}

