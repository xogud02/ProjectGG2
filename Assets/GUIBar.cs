using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class GUIBar : MonoBehaviour {
    public SpriteRenderer frame;
    public SpriteRenderer bar;
    public Animator anim;
    public int Length { get; private set; }
    public static readonly int BLINK = Animator.StringToHash("Blink");

    protected void Awake() {
        frame = GetComponent<SpriteRenderer>();
        var gui0 = "GUI0";
        var greenBarIndex = 41;
        Addressables.LoadAssetAsync<Sprite>($"{gui0}[{gui0}_{greenBarIndex}]").Completed += _ => bar.sprite = _.Result;
    }

    public void Init(int unit = 1) {
        unit = Mathf.Clamp(unit, 1, int.MaxValue);
        Length = unit;
        ScaleWidth(frame, unit);
        ScaleWidth(bar, unit);

        var frameBounds = frame.bounds;
        UpdateLongBarPosition();
    }

    public void SetLength(int unit) {
        unit = Mathf.Clamp(unit, 1, Length);
        ScaleWidth(bar, unit);
        UpdateLongBarPosition();
    }

    private void UpdateLongBarPosition() {
        var frameBounds = frame.bounds;
        var frameX = frameBounds.min.x;
        var barX = bar.bounds.min.x;
        var deltaBarX = frameX - barX;
        bar.transform.position += deltaBarX * Vector3.right;
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
