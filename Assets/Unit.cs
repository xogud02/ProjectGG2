using DG.Tweening;
using UnityEngine;
using UnityEngine.AddressableAssets;

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

public static class MoveExtension
{
    public static Vector3 STW(this Vector3 position) => Camera.main.ScreenToWorldPoint(position);
    public static Vector3 STW(this Vector2 position) => Camera.main.ScreenToWorldPoint(position);
    public static Vector2 Convert(this Vector3 v) => v;
    public static Vector3 Convert(this Vector2 v) => v;
}

public class Unit : MonoBehaviour
{
    [SerializeField] protected Weapon _weapon;
    [SerializeField] protected Unit target;
    protected UnitStatus _status;
    protected PathFinder _pathFinder;
    public Unit Target
    {
        get => target;
        set => target = value;
    }

    protected int AttackRange { get; set; }

    private GUIBar hpBar;

    private float speed = 5;
    private readonly int Right = Animator.StringToHash("Right");
    private readonly int Left = Animator.StringToHash("Left");
    private readonly int Up = Animator.StringToHash("Up");
    private readonly int Down = Animator.StringToHash("Down");
    private int currentDirection;
    private Tween moving;

    private Animator animator;
    public Vector2Int CurrentPosition { get; protected set; }
    public Vector2Int CurrentTargetPosition { get; protected set; }

    protected void Awake()
    {
        _pathFinder = new PathFinder();
    }

    public void Start()
    {
        _status = new UnitStatus(this);
        _status.AddListener(new UnitStatus.StatKeyType(UnitStatus.ValueType.Hp), _ =>
         {
             hpBar.Unit = _.After;
             if (_.After < _.Before)
             {
                 hpBar.Blink();
             }
         });

        _status.AddListener(new UnitStatus.StatKeyType(UnitStatus.ValueType.Level), _ =>
         {
             var level = _.After;
             if (level > _.Before)
             {
                 _status.Hp = _status.MaxHp;
                 hpBar.Init(_status.Hp, hpBar.ShowFrame);
                 Debug.Log("level up to " + level);
             }
         });
        animator = GetComponent<Animator>();
        currentDirection = Down;
        if (GridObject.Initialized)
        {
            transform.localScale *= GridObject.ScaleFactor;
        }
        else
        {
            GridObject.OnInit += () => transform.localScale *= GridObject.ScaleFactor;
        }

        var sr = GetComponent<SpriteRenderer>();
        sr.sprite.texture.filterMode = FilterMode.Point;//TODO inspector settings not working!!!!
        Addressables.LoadAssetAsync<GameObject>("GUIBar").Completed += task =>
        {
            var barObject = Instantiate(task.Result);
            barObject.transform.parent = transform;
            barObject.transform.localPosition = new Vector3(0, sr.bounds.extents.y, -10);
            barObject.transform.localScale = Vector3.one;
            hpBar = barObject.GetComponent<GUIBar>();
            hpBar.Init(_status.Hp);
        };

        if (_weapon)
        {
            _weapon = Instantiate(_weapon, transform);
            _weapon.transform.localPosition = Vector3.zero;
            _weapon.AttackSpeed = 2.5f;
        }
    }

    protected void OnDrawGizmos()
    {
        _pathFinder?.OnDrawGizmos(GridField.Convert(CurrentPosition), transform.position.z);
    }

    public void Stop()
    {
        if (IsMoving == false)
        {
            return;
        }

        EmptyPath();
    }

    public bool Hit(Unit by)
    {
        --_status.Hp;
        if (_status.Hp <= 0)
        {
            by.KillLogic(this);
            Die();
            return true;
        }
        return false;
    }

    public void KillLogic(Unit other)
    {
        if (other == this)
        {
            return;
        }
        Debug.Log($"{other} killed by {this} , reward : {other._status.RewardExp}");
        _status.Exp += other._status.RewardExp;
    }

    public virtual void Die()
    {
        GridField.UnOccupy(CurrentPosition);
        Destroy(gameObject);
    }

    public void SetPath(Vector2Int v)
    {
        EmptyPath();

        var wasMoving = IsMoving;

        _pathFinder.SetPath(_pathFinder.IsRemainPath ? CurrentTargetPosition : CurrentPosition, v);

        if (wasMoving == false && _pathFinder.TryGetNextTile(out var next))
        {
            Move(next);
        }
    }

    private void EmptyPath(bool leaveFirst = true)
    {
        _pathFinder.EmptyPath(leaveFirst && IsMoving);
    }

    public bool IsMoving => _pathFinder.IsRemainPath || (moving?.active ?? false);

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

    public virtual bool IsInRange(Unit unit)
    {
        if (unit == null)
        {
            return false;
        }

        if (unit == this)
        {
            return true;
        }

        return (unit.CurrentPosition - CurrentPosition).magnitude <= AttackRange;
    }

    public void MoveImmidiately(Vector2Int v)
    {
        EmptyPath(false);
        transform.position = GridField.Convert(v);
        CurrentPosition = v;
    }

    public void Move(Vector2Int v)
    {
        CurrentTargetPosition = v;
        var result = GridField.UnOccupy(CurrentPosition);
        if (result != null && result != this)
        {
            result.Occupy(CurrentPosition);
            return;
        }

        if (GridField.IsMovable(v))
        {
            this.Occupy(v);
        }
        else
        {
            EmptyPath(false);
            this.Occupy(CurrentPosition);
            return;
        }

        var dest = GridField.Convert(v);
        var direction = dest - transform.position.Convert();
        OnMoveSingle(direction);

        var dist = direction.magnitude;
        var time = dist / speed;
        moving = DOTween.To(() => (Vector2)transform.position, vec => transform.position = vec, dest, time).SetEase(Ease.Linear);

        moving.onComplete = () =>
        {
            moving = null;

            CurrentPosition = CurrentTargetPosition;
            if(_pathFinder.TryGetNextTile(out var next))
            {
                Move(next);
            }
        };
    }

    protected virtual void OnMoveSingle(Vector2 direction)
    {//TODO weapon Direction
        var preferDirection = GetPreferDirection(Vector2.SignedAngle(Vector2.right, direction));
        if (preferDirection != currentDirection)
        {
            animator.SetTrigger(preferDirection);
            currentDirection = preferDirection;
        }
    }
}
