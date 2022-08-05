using Cysharp.Threading.Tasks;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Tilemaps;

public class Game : MonoBehaviour {
    public bool DrawTileGizmo;
    public PlayableCharacter unit;
    public static Unit LastClicked;
    public Monster sample;
    public Grid grid;
    public RuleTile testTile;

    private void Awake()
    {
        if(gameObject.TryGetComponent(out grid) == false)
        {
            grid = gameObject.AddComponent<Grid>();
        }
    }

    void Start() {
        if (GridObject.Initialized) {
            Init();
        } else {
            GridObject.OnInit += Init;
        }
    }

    public static int tmp = 30;

    private void Init() {
        var field = GridObject.GrassField(tmp, tmp);//todo
        field.transform.parent = transform;
        field.gameObject.AddComponent<Tilemap>();
        field.gameObject.AddComponent<TilemapRenderer>();
        GridField.Init(tmp, tmp);
        ClickManager.Instance.OnClick -= OnClick;
        ClickManager.Instance.OnClick += OnClick;
        unit.MoveImmidiately(new GridPositionHandle(new Vector2Int(tmp / 2, tmp / 2)));
        unit.AttackTarget().Forget();
    }

    private void OnClick(GridPositionHandle position)
    {
        if (LastClicked == null)
        {
            LastClicked = GridField.GetOccupied(position);
        }

        if (LastClicked)
        {
            unit.Target = LastClicked;
        }
        unit.SetPath(position.WorldPosition);
    }

    private void Update() {
        Tile.DrawTileGizmos = DrawTileGizmo;

        if(sample == null)
        {
            sample = Addressables.InstantiateAsync("Slime_3").WaitForCompletion().GetComponent<Monster>();
        }
    }
}
