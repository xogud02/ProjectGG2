using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {
    [SerializeField] private Camera _camera;
    [SerializeField] private Unit _focused;
    private void Awake() {
        if (_focused) {
            Attach();
        }
    }

    private void Update() {
        if(_focused == null) {
            return;
        }

        var min = 0;
        var max = Game.tmp;
        var vMin = GridField.Convert(new Vector2Int(min, min));
        var vMax = GridField.Convert(new Vector2Int(max, max));
        var newPos = transform.localPosition;
    }
    

    private void Attach() {
        transform.parent = _focused.transform;
        transform.localPosition = Vector3.back * 10;
    }
}
