using UnityEngine;

public interface IHaveBehaviour  {
    
    public Transform transform { get; }

    public GameObject gameObject { get; }

    public bool enabled { get; }

    public bool isActiveAndEnabled { get; }
}
