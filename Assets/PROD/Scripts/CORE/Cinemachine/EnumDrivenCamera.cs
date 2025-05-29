using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.Cinemachine;
using UnityEngine;

public class EnumDrivenCamera : CinemachineCameraManagerBase
{
    [System.Serializable]
    public struct Instruction {
        public TurnStateEnum state;
        public CinemachineVirtualCameraBase virtualCamera;
    }
    
    [SerializeField] public Instruction[] Instructions;

    [SerializeField, PropertySpace(10, 10)] public TurnStateEnum CurrentState;
    
    private Dictionary<TurnStateEnum, CinemachineVirtualCameraBase> _cameraDict;
    
    protected override CinemachineVirtualCameraBase ChooseCurrentCamera(Vector3 worldUp, float deltaTime) {
        
        if (!PreviousStateIsValid)
            ValidateInstructions();

        return _cameraDict[CurrentState];
    }

    internal void ValidateInstructions() {
        _cameraDict = new Dictionary<TurnStateEnum, CinemachineVirtualCameraBase>();
        
        foreach (var instruction in Instructions) {
            if (instruction.virtualCamera != null)
                _cameraDict[instruction.state] = instruction.virtualCamera;
        }
    }
}
