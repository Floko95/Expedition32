using UnityEngine;

public abstract class AnInteractable : MonoBehaviour {
    private bool _isInteractable;

    public bool IsInteractable {
        get => _isInteractable;
        set {
            if (_isInteractable == value) return;

            _isInteractable = value;
            if (_isInteractable)
                OnBecameInteractable();
            else
                OnBecameUnInteractable();
        }
    }

    protected abstract void OnBecameInteractable();
    protected abstract void OnBecameUnInteractable();
    public abstract void Interact();
}