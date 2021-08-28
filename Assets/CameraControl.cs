using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {
    [SerializeField] private Camera _camera;
    [SerializeField] private Unit _focused;
    private void Awake() {
        if(_camera != null) {
            _camera = Camera.main;
        }
    }
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
}
