using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(0.5f, 0.3f, 1f)]
[TrackBindingType(typeof(CinemachineCamera))]
[TrackClipType(typeof(CinemachineFOVClip))]
public class CinemachineFOVTrack : TrackAsset {

    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount) {
        return ScriptPlayable<CinemachineFOVMixerBehaviour>.Create(graph, inputCount);
    }
}