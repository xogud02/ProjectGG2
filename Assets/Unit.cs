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

    private UnitMovementController _movement;
    private UnitAnimationController unitAnimationController;

    public Vector2Int CurrentPosition { get => _movement.CurrentPosition; protected set => _movement.CurrentPosition = value; }
    public Vector2Int CurrentTargetPosition { get => _movement.CurrentTargetPosition; protected set => _movement.CurrentTargetPosition = value; }

    protected void Awake()
    {
        _pathFinder = new PathFinder();
        _movement = new UnitMovementController(transform);
        gameObject.AddComponent<BoxCollider2D>();
    }
    private void OnMouseDown()
    {
        Debug.Log($"{this} ckicked");
        Game.LastClicked = this;
        ClickManager.Instance.Click(new GridPositionHandle(CurrentPosition));
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
        unitAnimationController = new UnitAnimationController(GetComponent<Animator>());
        GridObject.OnInit += () => transform.localScale *= GridObject.ScaleFactor;

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

    protected void OnDrawGizmos() => _pathFinder?.OnDrawGizmos(GridField.Convert(CurrentPosition), transform.position.z);

    public void Stop()//TODO impl?
    {
        if (IsMoving == false)
        {
            return;
        }
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
        var wasMoving = IsMoving;

        _pathFinder.ResetPath(_pathFinder.IsRemainPath ? CurrentTargetPosition : CurrentPosition, v);

        if (wasMoving == false)
        {
            TryMoveSingle();
        }
    }


    public bool IsMoving => _pathFinder.IsRemainPath || _movement.IsMoving;

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
        Stop();
        CurrentPosition = v;
    }

    public void Move(Vector2Int v)
    {
        CurrentTargetPosition = v;
        var result = GridField.UnOccupy(CurrentPosition);
        if (result != null && result != this)//todo cleanup Logic
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
            Stop();
            this.Occupy(CurrentPosition);
            return;
        }

        var direction = _movement.GetDirection(v);
        OnMoveSingle(direction);
        _movement.StartMoveSingle(direction, v, TryMoveSingle);
    }

    private void TryMoveSingle()
    {
        if (_pathFinder.TryGetNextTile(out var next))
        {
            Move(next);
        }
    }

    protected virtual void OnMoveSingle(Vector2 direction)
    {//TODO weapon Direction
        unitAnimationController.RefreshDirection(direction);
    }
}
