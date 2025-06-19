using UnityEngine;

public class BillboardUI : MonoBehaviour {
    private Camera _mainCamera;
    public bool ignoreYAxis = false; // Booléen pour contrôler la rotation sur l'axe Y
    public bool invertZAxis = false;
    
    private void Start() {
        _mainCamera = Camera.main;
    }

    private void LateUpdate() {
        if (_mainCamera == null) return;

        // Faire en sorte que l'UI regarde toujours la caméra
        Vector3 targetPosition = _mainCamera.transform.position;

        if (ignoreYAxis) {
            // Ignorer la rotation sur l'axe Y
            targetPosition.x = transform.position.x;
        }

        transform.LookAt(targetPosition);

        // Inverser la direction pour que l'UI ne soit pas à l'envers
        if(invertZAxis)
            transform.Rotate(0, 180, 0);
    }
}