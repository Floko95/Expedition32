using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/CancelEvent")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "CancelEvent", message: "Cancel was pressed", category: "Events", id: "30aeeaf725481587285af0981b9da3e1")]
public partial class CancelEvent : EventChannelBase
{
    public delegate void CancelEventEventHandler();
    public event CancelEventEventHandler Event; 

    public void SendEventMessage()
    {
        Event?.Invoke();
    }

    public override void SendEventMessage(BlackboardVariable[] messageData)
    {
        Event?.Invoke();
    }

    public override Delegate CreateEventHandler(BlackboardVariable[] vars, System.Action callback)
    {
        CancelEventEventHandler del = () =>
        {
            callback();
        };
        return del;
    }

    public override void RegisterListener(Delegate del)
    {
        Event += del as CancelEventEventHandler;
    }

    public override void UnregisterListener(Delegate del)
    {
        Event -= del as CancelEventEventHandler;
    }
}

