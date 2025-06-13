using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class CameraTargetController : MonoBehaviour, IInputAxisOwner
{
    [Tooltip("Horizontal Rotation.  Value is in degrees, with 0 being centered.")]
    public InputAxis HorizontalLook = new () { Range = new Vector2(-180, 180), Wrap = true, Recentering = InputAxis.RecenteringSettings.Default };

    [Tooltip("Vertical Rotation.  Value is in degrees, with 0 being centered.")]
    public InputAxis VerticalLook = new () { Range = new Vector2(-70, 70), Recentering = InputAxis.RecenteringSettings.Default };

    private Quaternion _desiredWorldRotation;
    private Character _character;
    
    public void GetInputAxes(List<IInputAxisOwner.AxisDescriptor> axes) {
        axes.Add(new () { DrivenAxis = () => ref HorizontalLook, Name = "Horizontal Look", Hint = IInputAxisOwner.AxisDescriptor.Hints.X });
        axes.Add(new () { DrivenAxis = () => ref VerticalLook, Name = "Vertical Look", Hint = IInputAxisOwner.AxisDescriptor.Hints.Y });
    }
    
    private void Awake() {
        _character = GetComponentInParent<Character>();
        if (_character == null) {
            Debug.LogWarning("Script not parented to a Character");
        }

        _character.AfterSimulationUpdated += PostUpdate;
    }

    private void OnDestroy() {
        _character.AfterSimulationUpdated -= PostUpdate;
    }

    private void Update() {
        transform.localRotation = Quaternion.Euler(VerticalLook.Value, HorizontalLook.Value, 0);
        _desiredWorldRotation = transform.rotation;
        
        VerticalLook.UpdateRecentering(Time.deltaTime, VerticalLook.TrackValueChange());
        HorizontalLook.UpdateRecentering(Time.deltaTime, HorizontalLook.TrackValueChange());
    }
    
    private void PostUpdate(float deltaTime) {
        transform.rotation = _desiredWorldRotation;
        var delta = (Quaternion.Inverse(_character.transform.rotation) * _desiredWorldRotation).eulerAngles;
        
        VerticalLook.Value = NormalizeAngle(delta.x);
        HorizontalLook.Value = NormalizeAngle(delta.y);
    }

    float NormalizeAngle(float angle)
    {
        while (angle > 180)
            angle -= 360;
        while (angle < -180)
            angle += 360;
        return angle;
    }
}
