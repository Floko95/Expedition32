using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[System.Serializable]
public class CinemachineFOVClip : PlayableAsset, ITimelineClipAsset {
    
    public CinemachineFOVBehaviour template = new CinemachineFOVBehaviour();

    public ClipCaps clipCaps => ClipCaps.Blending;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner) {
        return ScriptPlayable<CinemachineFOVBehaviour>.Create(graph, template);
    }
}