using UnityEngine;

public interface IInitializable<in T> {
    void Init(T data);
    
    public bool Initialized { get; protected set; }
}