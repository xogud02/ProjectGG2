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
    private MoveDirection _lastMoveDirection;

    public UnitAnimationController(Animator animator) => _animator = animator;

    public void RefreshDirection(Vector2 direction)
    {
        var preferDirection = GetPreferDirection(Vector2.SignedAngle(Vector2.right, direction));
        if (preferDirection != _lastDirectionHash)
        {
            _animator.SetTrigger(preferDirection);
            _lastDirectionHash = preferDirection;
        }
    }

    private int GetPreferDirection(float angle)
    {
        var absAngle = Mathf.Abs(angle);
        if (absAngle < 45f)
        {
            return Right;
        }
        else if (absAngle > 135f)
        {
            return Left;
        }
        else if (angle > 0)
        {
            return Up;
        }
        else
        {
            return Down;
        }
    }

    private MoveDirection ToMoveDirection(Vector2 direction)
    {
        var signedAngle = Vector2.SignedAngle(Vector2.right, direction);
        var absAngle = Mathf.Abs(signedAngle);
        return MoveDirection.None;
    }

    private int GetDirectionHash(MoveDirection direction)
    {
        return Up;
    }
}