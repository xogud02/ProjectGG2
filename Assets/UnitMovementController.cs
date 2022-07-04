using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using UnityEngine;

public class UnitMovementController
{
    private readonly Transform _transform;
    private readonly GridPositionHandle _handle;

    public UnitMovementController(Transform unitTransform, GridPositionHandle handle)
    {
        _transform = unitTransform;
        _handle = handle;
    }

    public Vector2Int CurrentPosition
    {
        get => currentPosition; set
        {
            currentPosition = value;
            _transform.position = GridField.Convert(value);
        }
    }


    public GridPositionHandle CurrentTargetPosition { get; set; } = new GridPositionHandle();
    public float Speed { get; set; } = 5;
    private Tween moving;
    private Vector2Int currentPosition;

    public bool IsMoving => (moving?.active ?? false);

    public async UniTask StartMoveSingle(float dist, Vector2Int v)
    {
        var dest = GridField.Convert(v);
        var time = dist / Speed;
        moving = DOTween.To(() => (Vector2)_transform.position, vec => _transform.position = vec, dest, time).SetEase(Ease.Linear);

        await moving.AsyncWaitForCompletion();
        
        moving = null;
        CurrentPosition = CurrentTargetPosition.WorldPosition;
    }

    public Vector2 GetDirection(Vector2Int v)
    {
        var dest = GridField.Convert(v);
        return dest - _transform.position.Convert();
    }
}
