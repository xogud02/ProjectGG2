using DG.Tweening;
using System;
using UnityEngine;

public class UnitMovementController
{
    private readonly Transform _transform;
    public UnitMovementController(Transform unitTransform) => _transform = unitTransform;

    public Vector2Int CurrentPosition
    {
        get => currentPosition; set
        {
            currentPosition = value;
            _transform.position = GridField.Convert(value);
        }
    }

    public GridPositionHandle _handle = new GridPositionHandle();

    public Vector2Int CurrentTargetPosition { get; set; }
    public float Speed { get; set; } = 5;
    private Tween moving;
    private Vector2Int currentPosition;

    public bool IsMoving => (moving?.active ?? false);

    public void StartMoveSingle(Vector2 direction, Vector2Int v, Action onComplete)
    {
        var dest = GridField.Convert(v);
        var dist = direction.magnitude;
        var time = dist / Speed;
        moving = DOTween.To(() => (Vector2)_transform.position, vec => _transform.position = vec, dest, time).SetEase(Ease.Linear);

        moving.onComplete = () =>
        {
            moving = null;

            CurrentPosition = CurrentTargetPosition;
            onComplete?.Invoke();
        };
    }

    public Vector2 GetDirection(Vector2Int v)
    {
        var dest = GridField.Convert(v);
        return dest - _transform.position.Convert();
    }
}
