using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class GUIBar : MonoBehaviour {
    public SpriteRenderer frame;
    public SpriteRenderer longBar;
    public SpriteRenderer shortBar;
    public Animator shortBarAnimator;
    public RuntimeAnimatorController[] controllers;
    public int Length { get; private set; }

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
        Length = unit;
        ScaleWidth(frame, unit);
        ScaleWidth(longBar, unit - 1);

        var frameBounds = frame.bounds;
        UpdateLongBarPosition();
        UpdateShortBarPosition();
    }

    public void SetLength(int unit) {//shortBar anim, shortBar Position
        unit = Mathf.Clamp(unit, 1, Length);
        ScaleWidth(longBar, unit);
        UpdateLongBarPosition();
        UpdateShortBarPosition();
    }

    private void UpdateLongBarPosition() {
        var frameBounds = frame.bounds;
        var frameX = frameBounds.min.x;
        var barX = longBar.bounds.min.x;
        var deltaBarX = frameX - barX;
        longBar.transform.position += deltaBarX * Vector3.right;
    }

    private void UpdateShortBarPosition() {
        var longBarBounds = longBar.bounds;
        var shortBarX = longBarBounds.max.x + shortBar.bounds.extents.x;
        var z = shortBarAnimator.transform.position.z;
        shortBarAnimator.transform.position = new Vector3(shortBarX, longBar.transform.position.y, z);
    }

    private void ScaleWidth(SpriteRenderer renderer, int unit) {
        if (renderer == null || renderer.drawMode == SpriteDrawMode.Simple) {
            return;
        }
        var size = renderer.size;
        size.x = size.y * unit;
        renderer.size = size;
    }
}
