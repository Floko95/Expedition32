using UnityEngine;

public class GrapplingHook : AnInteractable {

    [SerializeField] private Transform destinationPoint;
    
    private Character _character;

    protected override void OnBecameInteractable() { }
    protected override void OnBecameUnInteractable() { }
    public override void Interact() { }
}
