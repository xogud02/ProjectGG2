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

        var focusedPos = _focused.transform.position;

        var screenMin = _camera.ViewportToWorldPoint(Vector3.zero);
        var screenMax = _camera.ViewportToWorldPoint(Vector3.one);

        var screenRect = new Rect(screenMin, screenMax - screenMin);

        var fieldMin = GridField.Convert(Vector2Int.zero);
        var fieldMax = GridField.Convert(new Vector2Int(Game.tmp, Game.tmp));

        var fieldRect = new Rect(fieldMin, fieldMax - fieldMin);

        var newPos = focusedPos;
        newPos.z = z;
        transform.position = newPos;
    }
    
}
