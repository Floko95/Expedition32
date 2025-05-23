using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class CinemachineFOVBehaviour : PlayableBehaviour {
    [Range(1f, 179f)] public float fov = 60f;
}