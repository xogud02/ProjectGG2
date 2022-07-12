using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class UnitMovementController
{
    private readonly Transform _transform;
    private readonly GridPositionHandle _handle;
    public Vector2 CurrentMovingDirection { get; private set; }
    public UnitMovementController(Transform unitTransform, GridPositionHandle handle)
    {
        _transform = unitTransform;
        _handle = handle;
    }

    public Vector2Int CurrentPosition
    {
        get => _handle.WorldPosition;
        set
        {
            _handle.LocalPosition = value;
            _transform.position = GridField.Convert(value);
        }
    }


    public GridPositionHandle CurrentTargetPosition { get; set; } = new GridPositionHandle();
    public float Speed { get; set; } = 5;
    private Tween moving;

    public bool IsMoving => (moving?.active ?? false);

    public async UniTask StartMoveSingle(Vector2Int v)
    {
        var dest = GridField.Convert(v);
        CurrentMovingDirection = dest - _transform.position.Convert();
        var dist = CurrentMovingDirection.magnitude;
        var time = dist / Speed;
        moving = DOTween.To(() => (Vector2)_transform.position, vec => _transform.position = vec, dest, time).SetEase(Ease.Linear);

        await moving.AsyncWaitForCompletion();

        moving = null;
        CurrentPosition = CurrentTargetPosition.WorldPosition;
    }
}
