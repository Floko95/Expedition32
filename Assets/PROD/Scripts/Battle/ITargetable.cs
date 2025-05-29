using UnityEngine;

public interface ITargetable : IHaveBehaviour {
    void OnTargeted();
    void OnUntargeted();
}
