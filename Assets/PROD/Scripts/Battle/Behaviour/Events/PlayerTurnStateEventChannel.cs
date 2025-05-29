using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/PlayerTurnStateEventChannel")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "PlayerTurnStateEventChannel", message: "PlayerTurn state has changed to [newstate]", category: "Events", id: "f57680621a6d6c1facc4c18ada8390f4")]
public partial class PlayerTurnStateEventChannel : EventChannelBase
{
    public delegate void PlayerTurnStateEventChannelEventHandler(TurnStateEnum newstate);
    public event PlayerTurnStateEventChannelEventHandler Event; 

    public void SendEventMessage(TurnStateEnum newstate)
    {
        Event?.Invoke(newstate);
    }

    public override void SendEventMessage(BlackboardVariable[] messageData)
    {
        BlackboardVariable<TurnStateEnum> newstateBlackboardVariable = messageData[0] as BlackboardVariable<TurnStateEnum>;
        var newstate = newstateBlackboardVariable != null ? newstateBlackboardVariable.Value : default(TurnStateEnum);

        Event?.Invoke(newstate);
    }

    public override Delegate CreateEventHandler(BlackboardVariable[] vars, System.Action callback)
    {
        PlayerTurnStateEventChannelEventHandler del = (newstate) =>
        {
            BlackboardVariable<TurnStateEnum> var0 = vars[0] as BlackboardVariable<TurnStateEnum>;
            if(var0 != null)
                var0.Value = newstate;

            callback();
        };
        return del;
    }

    public override void RegisterListener(Delegate del)
    {
        Event += del as PlayerTurnStateEventChannelEventHandler;
    }

    public override void UnregisterListener(Delegate del)
    {
        Event -= del as PlayerTurnStateEventChannelEventHandler;
    }
}

