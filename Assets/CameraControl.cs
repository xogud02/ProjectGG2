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
    }
    

    private void Attach() {
        transform.parent = _focused.transform;
        transform.localPosition = Vector3.back * 10;
    }
}
