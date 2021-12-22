using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class GUIBar : MonoBehaviour {
    public SpriteRenderer frame;
    public SpriteRenderer bar;
    public Animator anim;
    public int Length { get; private set; }
    public static readonly int BLINK = Animator.StringToHash("Blink");

    public bool ShowFrame => frame.enabled;

    private int remainUnit;

    private float remainBlinkTime = 0f;
    private Coroutine blinkRoutine = null;

    protected void Awake() {
        frame = GetComponent<SpriteRenderer>();
        var gui0 = "GUI0";
        var greenBarIndex = 41;
        Addressables.LoadAssetAsync<Sprite>($"{gui0}[{gui0}_{greenBarIndex}]").Completed += _ => bar.sprite = _.Result;
    }

    public void Init(int unit = 1) {
        unit = Mathf.Clamp(unit, 1, int.MaxValue);
        Length = unit;

        SetMaxAndCurrent(unit);
    }

    public void SetMaxAndCurrent(int unit) {
        ScaleWidth(frame, unit);
        remainUnit = unit;
    }

    public void SetMax(int unit) { }
    public void SetCurrent(int unit) {
        remainUnit = unit;
    }

    public void SetLength(int unit) {
        unit = Mathf.Clamp(unit, 1, Length);
        ScaleWidth(bar, unit);
        UpdateLongBarPosition();
    }

    public void Blink(float time = 0.5f) {
        remainBlinkTime = time;
        if (blinkRoutine == null) {
            blinkRoutine = StartCoroutine(BlinkCoroutine());
        }
    }

    private IEnumerator BlinkCoroutine() {
        anim.SetBool(BLINK, true);
        while (remainBlinkTime > 0) {
            yield return null;
            remainBlinkTime -= Time.deltaTime;
        }
        blinkRoutine = null;
        anim.SetBool(BLINK, false);
    }

    private void UpdateLongBarPosition() {
        var frameBounds = frame.bounds;
        var frameX = frameBounds.min.x;
        var barX = bar.bounds.min.x;
        var deltaBarX = frameX - barX;
        bar.transform.position += deltaBarX * Vector3.right;
    }

    private void ScaleWidth(SpriteRenderer renderer, int unit, bool keepRatio = false) {
        if (renderer == null || renderer.drawMode == SpriteDrawMode.Simple) {
            return;
        }
        var size = renderer.size;
        size.x = size.y * unit;
        renderer.size = size;

        if (keepRatio) {

        }
    }
}
