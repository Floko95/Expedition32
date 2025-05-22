using UnityEngine.Playables;
using Unity.Cinemachine;
using UnityEngine;

public class CinemachineFOVMixerBehaviour : PlayableBehaviour {

    private float originalFOV;
    private bool firstFrameHappened;
    private CinemachineCamera vcam;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData) {
        vcam = playerData as CinemachineCamera;
        if (vcam == null)
            return;

        if (!firstFrameHappened) {
            originalFOV = vcam.Lens.FieldOfView;
            firstFrameHappened = true;
        }

        int inputCount = playable.GetInputCount();
        float blendedFOV = 0f;
        float totalWeight = 0f;

        for (int i = 0; i < inputCount; i++) {
            float inputWeight = playable.GetInputWeight(i);
            ScriptPlayable<CinemachineFOVBehaviour> inputPlayable =
                (ScriptPlayable<CinemachineFOVBehaviour>) playable.GetInput(i);
            CinemachineFOVBehaviour input = inputPlayable.GetBehaviour();

            blendedFOV += input.fov * inputWeight;
            totalWeight += inputWeight;
        }

        if (totalWeight > 0f)
            vcam.Lens.FieldOfView = blendedFOV;
    }

    public override void OnBehaviourPause(Playable playable, FrameData info) {
        if (vcam != null)
            vcam.Lens.FieldOfView = originalFOV;

        firstFrameHappened = false;
    }

    public override void OnGraphStop(Playable playable) {
        if (vcam != null) {
#if UNITY_EDITOR
            // Reset FOV to default when scrubbing stops
            if (!Application.isPlaying)
                vcam.Lens.FieldOfView = originalFOV;
#endif
        }

        firstFrameHappened = false;
    }

    public override void OnPlayableDestroy(Playable playable) {
        firstFrameHappened = false;
    }
}