using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class GUIBar : MonoBehaviour {
    public SpriteRenderer longBar;
    public Animator shortBar;
    public RuntimeAnimatorController[] controllers;

    protected void Awake() {
        controllers = new RuntimeAnimatorController[5];
        for (int i = 0; i < 4; ++i) {
            var capture = i;
            Addressables.LoadAssetAsync<RuntimeAnimatorController>($"GUI_{41 + i}").Completed += _ => controllers[capture] = _.Result;
        }
    }
}
