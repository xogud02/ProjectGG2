using UnityEngine;

public class CameraControl : MonoBehaviour {
    [SerializeField] private Camera _camera;
    [SerializeField] private Unit _focused;

    private float z;
    private void Awake() {
        _camera = Camera.main;
        z = _camera.transform.position.z;
    }

    private void LateUpdate() {
        if(_focused == null) {
            return;
        }

        var newPos = _focused.transform.position;
        newPos.z = z;
        transform.position = newPos;
    }
    
}
