using UnityEngine;
using UnityEngine.Events;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] private UnityEvent onInteract;
    
    public bool IsInteractable { get; protected set; }
    
    public abstract void Interact();
}
