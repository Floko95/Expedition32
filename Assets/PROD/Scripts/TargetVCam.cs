using Unity.Cinemachine;
using UnityEngine;

public class TargetVCam : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCameraBase vCam;
    
    private TargetManager _targetManager;
    
    private async void Start() {
        await Toolbox.WaitUntilReadyAsync();
        _targetManager = Toolbox.Get<TargetManager>();
        
        _targetManager.onTargetChanged += OnTargetChanged;
    }

    private void OnDestroy() {
        _targetManager.onTargetChanged -= OnTargetChanged;
    }

    private void OnTargetChanged(ITargetable obj) {
        vCam.Follow = obj.transform;
    }
}
