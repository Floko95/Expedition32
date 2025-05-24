using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.Cinemachine;
using UnityEngine;

public enum CameraState {
    Idle,
    AbilitySelection,
    TargetSelection,
    ItemSelection,
}

public class EnumDrivenCamera : CinemachineCameraManagerBase
{
    [System.Serializable]
    public struct Instruction {
        public CameraState state;
        public CinemachineVirtualCameraBase virtualCamera;
    }
    
    [SerializeField] public Instruction[] Instructions;

    [SerializeField, PropertySpace(10, 10)] public CameraState CurrentState;
    
    private Dictionary<CameraState, CinemachineVirtualCameraBase> _cameraDict;
    
    protected override CinemachineVirtualCameraBase ChooseCurrentCamera(Vector3 worldUp, float deltaTime) {
        
        if (!PreviousStateIsValid)
            ValidateInstructions();

        return _cameraDict[CurrentState];
    }

    internal void ValidateInstructions() {
        _cameraDict = new Dictionary<CameraState, CinemachineVirtualCameraBase>();
        
        foreach (var instruction in Instructions) {
            if (instruction.virtualCamera != null)
                _cameraDict[instruction.state] = instruction.virtualCamera;
        }
    }
}
