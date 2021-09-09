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
        if (_focused == null) {
            return;
        }

        var focusedPos = _focused.transform.position;

        var delta = transform.position - focusedPos;

        var targetScreenMin = _camera.ViewportToWorldPoint(Vector3.zero) + delta;
        var targetScreenMax = _camera.ViewportToWorldPoint(Vector3.one) + delta;

        var targetScreenRect = new Rect(targetScreenMin, targetScreenMax - targetScreenMin);

        var fieldMin = GridField.Convert(Vector2Int.zero);
        var fieldMax = GridField.Convert(new Vector2Int(Game.tmp, Game.tmp));

        var fieldRect = new Rect(fieldMin, fieldMax - fieldMin);

        var newPos = focusedPos;
        newPos.x = GetX();
        newPos.x = GetY();

        float GetX() {
            if (targetScreenRect.width > fieldRect.width ) {
                return 0;
            }

            if(fieldRect.xMin < targetScreenRect.xMin) {

            }

            return newPos.x;
        }

        float GetY() {
            if (targetScreenRect.height > fieldRect.height) {
                return 0;
            }

            return newPos.y;
        }


        newPos.z = z;
        transform.position = newPos;
    }

}
