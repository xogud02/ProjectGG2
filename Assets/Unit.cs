using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections.Generic;
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
    public Unit Target
    {
        get => target;
        set => target = value;
    }

    public int MaxHp => Level * 10;
    private int hp = 10;
    public int Hp
    {
        get => hp;
        private set
        {
            var before = hp;
            hp = Mathf.Clamp(value, 0, MaxHp);
            hpBar.Unit = hp;
            if (hp < before)
            {
                hpBar.Blink();
            }
        }
    }

    public int RewardExp => 15;
    private int _exp;
    public int Exp {
        get => _exp;
        set
        {
            _exp += value;
            Debug.Log($"exp increased {value} => current : {_exp}/{MaxExp}");
            while(_exp >= MaxExp)
            {
                _exp -= MaxExp;
                ++Level;
            }
        }
    }
    public int MaxExp => Level * 10;
    private int _level = 1;
    public int Level
    {
        get => _level;
        set
        {
            var before = _level;
            _level = value;
            if(_level > before)
            {
                Hp = MaxHp;
                hpBar.Init(Hp, hpBar.ShowFrame);
                Debug.Log("level up to " + _level);
            }
        }
    }

    protected void KillLogic(Unit other)
    {
        if (other == this)
        {
            return;
        }
        Debug.Log($"{other} killed by {this} , reward : {other.RewardExp}");
        Exp += other.RewardExp;
    }

    private int attack;
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
    private Queue<Vector2Int> currentPath = new Queue<Vector2Int>();
    public Vector2Int CurrentPosition { get; protected set; }

    public void Start()
    {
        _status = new UnitStatus(this);
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
            hpBar.Init(Hp);
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
        if (currentPath.Count == 0)
        {
            return;
        }

        var z = transform.position.z;
        var current = (Vector3)GridField.Convert(CurrentPosition);
        Gizmos.color = Color.white;
        current.z = z;
        foreach (var next in currentPath)
        {
            var nextV3 = (Vector3)GridField.Convert(next);
            nextV3.z = z;
            Gizmos.DrawLine(current, nextV3);
            current = nextV3;
        }
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
        --Hp;
        if (Hp <= 0)
        {
            by.KillLogic(this);
            Die();
            return true;
        }
        return false;
    }

    public virtual void Die()
    {
        GridField.UnOccupy(CurrentPosition);
        Destroy(gameObject);
    }

    public void SetPath(Vector2Int v)
    {
        EmptyPath();
        var newPath = AStar.Find(currentPath.Count > 0 ? currentPath.Peek() : CurrentPosition, v);
        var wasMoving = IsMoving;
        while (newPath.Count > 0)
        {
            currentPath.Enqueue(newPath.Pop());
        }

        if (wasMoving == false && currentPath.Count > 0)
        {
            Move(currentPath.Peek());
        }
    }

    private void EmptyPath(bool leaveFirst = true)
    {
        if (leaveFirst == false)
        {
            currentPath.Clear();
            return;
        }

        var lastPath = new Queue<Vector2Int>();
        if (IsMoving && currentPath.Count > 0)
        {
            lastPath.Enqueue(currentPath.Dequeue());
        }
        currentPath = lastPath;
    }

    public bool IsMoving => currentPath.Count > 0 || (moving?.active ?? false);

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

            if (currentPath.Count == 0)
            {
                return;
            }

            CurrentPosition = currentPath.Dequeue();

            if (currentPath.Count != 0)
            {
                Move(currentPath.Peek());
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
