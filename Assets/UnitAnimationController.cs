using System;
using UnityEngine;

public class UnitAnimationController
{
    private enum MoveDirection
    {
        None,
        Up,
        UpRight,
        Right,
        DownRight,
        Down,
        DownLeft,
        Left,
        UpLeft,
    }

    private readonly Animator _animator;
    private readonly int Right = Animator.StringToHash("Right");
    private readonly int Left = Animator.StringToHash("Left");
    private readonly int Up = Animator.StringToHash("Up");
    private readonly int Down = Animator.StringToHash("Down");
    private int _lastDirectionHash;

    public UnitAnimationController(Animator animator) => _animator = animator;

    public void RefreshDirection(Vector2 direction)
    {
        var preferDirection = GetDirectionHash(ToMoveDirection(direction));
        if (preferDirection != _lastDirectionHash)
        {
            _animator.SetTrigger(preferDirection);
            _lastDirectionHash = preferDirection;
        }
    }

    private MoveDirection ToMoveDirection(Vector2 direction)
    {
        var signedAngle = Vector2.SignedAngle(Vector2.right, direction);
        var absAngle = Mathf.Abs(signedAngle);
        const float half = 22.5f;
        const float leftAngle = 180;
        const float verticalAngle = 90;
        if (absAngle < half)
        {
            return MoveDirection.Right;
        }

        if (absAngle > leftAngle - half)
        {
            return MoveDirection.Left;
        }

        var isUpSide = signedAngle > 0;

        if (absAngle > verticalAngle + half)
        {
            return isUpSide ? MoveDirection.UpLeft : MoveDirection.DownLeft;
        }

        if (absAngle > verticalAngle - half)
        {
            return isUpSide ? MoveDirection.Up : MoveDirection.Down;
        }

        return isUpSide ? MoveDirection.UpRight : MoveDirection.DownRight;
    }

    private int GetDirectionHash(MoveDirection direction)
    {
        switch (direction)
        {
            case MoveDirection.Up:
                return Up;
            case MoveDirection.Down:
                return Down;
            case MoveDirection.Left:
                return Left;
            case MoveDirection.Right:
                return Right;
            case MoveDirection.UpRight:
                return GetHashByPriority(Up, Right);
            case MoveDirection.UpLeft:
                return GetHashByPriority(Up, Left);
            case MoveDirection.DownLeft:
                return GetHashByPriority(Left, Down);
            case MoveDirection.DownRight:
                return GetHashByPriority(Right, Down);
            default:
                return Down;
        }

        int GetHashByPriority(int first, int second) => _lastDirectionHash == first ? first : second;
    }
}
