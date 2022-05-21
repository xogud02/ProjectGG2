using UnityEngine;

public class UnitAnimationController
{
    private readonly Animator _animator;
    private readonly int Right = Animator.StringToHash("Right");
    private readonly int Left = Animator.StringToHash("Left");
    private readonly int Up = Animator.StringToHash("Up");
    private readonly int Down = Animator.StringToHash("Down");
    private int currentDirection;

    public UnitAnimationController(Animator animator) => _animator = animator;

    public void RefreshDirection(Vector2 direction)
    {
        var preferDirection = GetPreferDirection(Vector2.SignedAngle(Vector2.right, direction));
        if (preferDirection != currentDirection)
        {
            _animator.SetTrigger(preferDirection);
            currentDirection = preferDirection;
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
}
