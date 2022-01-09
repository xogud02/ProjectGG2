using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class GUIBar : MonoBehaviour {
    public SpriteRenderer frame;
    public SpriteRenderer bar;
    public Animator anim;
    public float WidthRatio { get; set; } = 1;
    public static readonly int BLINK = Animator.StringToHash("Blink");

    public bool ShowFrame => frame.enabled;

    private int _unit;
    public int Unit {
        get => _unit;
        set {
            _unit = value;
            SetLength(bar, _unit);
        }
    }

    private int _maxUnit;
    public int MaxUnit {
        get => _maxUnit;
        private set {
            _maxUnit = Mathf.Clamp(value, 1, int.MaxValue);
            _unit = Mathf.Clamp(_unit, 1, _maxUnit);
        }
    }

    private float remainBlinkTime = 0f;
    private Coroutine blinkRoutine = null;

    protected void Awake() {
        var gui0 = "GUI0";
        var greenBarIndex = 41;
        Addressables.LoadAssetAsync<Sprite>($"{gui0}[{gui0}_{greenBarIndex}]").Completed += _ => bar.sprite = _.Result;
    }

    public void Init() => Init(1);
    public void Init(int unit) => Init(unit, false);
    public void Init(int unit, bool showFrame) => Init(unit, showFrame, 1);
    public void Init(int unit, bool showFrame, float length) {
        _unit = unit;
        MaxUnit = unit;
        unit = Mathf.Clamp(unit, 1, int.MaxValue);
        frame.enabled = showFrame;
        SetLengthAndScale(frame, unit, length);
        SetLengthAndScale(bar, unit, length);
        UpdateBarPosition();
    }

    private void SetLengthAndScale(SpriteRenderer sr, int unit, float scale) {
        SetLength(sr, unit);
        var newScale = sr.transform.localScale;
        newScale.x = newScale.y * scale / unit;
        sr.transform.localScale = newScale;
    }

    private void SetLength(SpriteRenderer sr, int unit) {
        var newSize = sr.size;
        newSize.x = newSize.y * unit;
        sr.size = newSize;
    }

    private void UpdateBarPosition() {
        var frameBounds = frame.bounds;
        var frameX = frameBounds.min.x;
        var barX = bar.bounds.min.x;
        var deltaBarX = frameX - barX;
        bar.transform.position += deltaBarX * Vector3.right;
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
}
