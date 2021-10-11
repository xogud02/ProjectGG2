using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class GUIBar : MonoBehaviour {
    public SpriteRenderer longBar;
    public Animator shortBar;
    public RuntimeAnimatorController[] controllers;

    protected void Awake() {
        var gui0 = "GUI0";
        var greenBarIndex = 41;
        Addressables.LoadAssetAsync<Sprite>($"{gui0}[{gui0}_{greenBarIndex}]").Completed += _=>longBar.sprite = _.Result;

        controllers = new RuntimeAnimatorController[4];
        for (int i = 0; i < 4; ++i) {
            var capture = i;
            Addressables.LoadAssetAsync<RuntimeAnimatorController>($"GUI_{greenBarIndex + i}").Completed += _ => controllers[capture] = _.Result;
        }
    }
}
