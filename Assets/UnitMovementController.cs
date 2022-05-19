using DG.Tweening;
using System;
using UnityEngine;

public class UnitMovementController
{
    private readonly Transform _transform;
    public UnitMovementController(Transform unitTransform) => _transform = unitTransform;

    public Vector2Int CurrentPosition { get; set; }
    public Vector2Int CurrentTargetPosition { get; set; }
    private Tween moving;

    public bool IsMoving => (moving?.active ?? false);

    public void StartMoveSingle(Vector2 direction, float speed, Vector2 dest, Action onComplete)
    {
        var dist = direction.magnitude;
        var time = dist / speed;
        moving = DOTween.To(() => (Vector2)_transform.position, vec => _transform.position = vec, dest, time).SetEase(Ease.Linear);

        moving.onComplete = () =>
        {
            moving = null;

            CurrentPosition = CurrentTargetPosition;
            onComplete?.Invoke();
        };
    }
}
