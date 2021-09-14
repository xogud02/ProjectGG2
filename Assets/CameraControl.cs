using UnityEngine;

public class CameraControl : MonoBehaviour {
    [SerializeField] private Camera _camera;
    [SerializeField] private Unit _focused;
    private Rect targetScreenRect;
    private Rect fieldRect;

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

        var camToFocus = focusedPos - transform.position;

        var targetScreenMin = _camera.ViewportToWorldPoint(Vector3.zero) + camToFocus;
        var targetScreenMax = _camera.ViewportToWorldPoint(Vector3.one) + camToFocus;

        targetScreenRect = new Rect(targetScreenMin, targetScreenMax - targetScreenMin);

        var fieldMin = GridField.Convert(Vector2Int.zero);
        var fieldMax = GridField.Convert(new Vector2Int(Game.tmp, Game.tmp));

        fieldRect = new Rect(fieldMin, fieldMax - fieldMin);

        var newPos = new Vector3(GetX(), GetY(), z);

        float GetX() {
            if (targetScreenRect.width > fieldRect.width ) {
                return fieldRect.center.x;
            }

            var screenMinToFieldMin = fieldRect.xMin - targetScreenRect.xMin;
            if(screenMinToFieldMin > 0) {
                return focusedPos.x + screenMinToFieldMin;
            }

            var screenMaxToFieldMax = fieldRect.xMax - targetScreenRect.xMax;
            if(screenMaxToFieldMax < 0) {
                return focusedPos.x + screenMaxToFieldMax;
            }

            return focusedPos.x;
        }

        float GetY() {
            if (targetScreenRect.height> fieldRect.height) {
                return fieldRect.center.y;
            }

            var screenMinToFieldMin = fieldRect.yMin- targetScreenRect.yMin;
            if (screenMinToFieldMin > 0) {
                return focusedPos.y + screenMinToFieldMin;
            }

            var screenMaxToFieldMax = fieldRect.yMax - targetScreenRect.yMax;
            if (screenMaxToFieldMax < 0) {
                return focusedPos.y + screenMaxToFieldMax;
            }

            return focusedPos.y;
        }


        newPos.z = z;
        transform.position = newPos;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(targetScreenRect.center, targetScreenRect.size);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(fieldRect.center, fieldRect.size);
    }

}
