using UnityEngine;

public class WorldspaceUIScaler : MonoBehaviour
{
    public float sizeAtReferenceDistance = 1.0f;
    public float referenceDistance = 10.0f;

    private Camera cam;
    
    private void Awake() {
        cam = Camera.main;
    }

    void LateUpdate()
    {
        float currentDistance = Vector3.Distance(transform.position, cam.transform.position);
        float scale = sizeAtReferenceDistance * (currentDistance / referenceDistance);
        transform.localScale = Vector3.one * scale;
    }
}