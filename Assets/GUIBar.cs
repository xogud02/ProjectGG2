using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class GUIBar : MonoBehaviour {
    public SpriteRenderer frame;
    public SpriteRenderer longBar;
    public Animator shortBar;
    public RuntimeAnimatorController[] controllers;

    protected void Awake() {
        frame = GetComponent<SpriteRenderer>();
        var gui0 = "GUI0";
        var greenBarIndex = 41;
        Addressables.LoadAssetAsync<Sprite>($"{gui0}[{gui0}_{greenBarIndex}]").Completed += _ => longBar.sprite = _.Result;

        controllers = new RuntimeAnimatorController[4];
        for (int i = 0; i < 4; ++i) {
            var capture = i;
            Addressables.LoadAssetAsync<RuntimeAnimatorController>($"GUI_{greenBarIndex + i}").Completed += _ => controllers[capture] = _.Result;
        }
    }

    public void Init(int unit = 1) {
        unit = Mathf.Clamp(unit, 1, int.MaxValue);
        ScaleWidth(frame, unit);
        ScaleWidth(longBar, unit - 1);

        var frameBounds = frame.bounds;
        var frameX = frameBounds.min.x;
        var barX = longBar.bounds.min.x;
        var deltaBarX = frameX - barX;
        longBar.transform.localPosition += deltaBarX * Vector3.right;

        var shortBarX = frameBounds.extents.x - frame.size.y / 2;
        var localZ = shortBar.transform.localPosition.z;
        shortBar.transform.localPosition = new Vector3(shortBarX, 0, localZ);
    }

    private void ScaleWidth(SpriteRenderer renderer, int unit) {
        if (renderer == null || renderer.drawMode != SpriteDrawMode.Sliced) {
            return;
        }
        var size = renderer.size;
        size.x = size.y * unit;
        renderer.size = size;
    }
}
