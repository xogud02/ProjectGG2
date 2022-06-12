using Cysharp.Threading.Tasks;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Game : MonoBehaviour {
    public bool DrawTileGizmo;
    public PlayableCharacter unit;
    public static Unit LastClicked;
    public Monster sample;

    void Start() {
        if (GridObject.Initialized) {
            Init();
        } else {
            GridObject.OnInit += Init;
        }
    }

    public static int tmp = 30;
    private Coroutine findTarget;
    private Coroutine attackTarget;

    private void Init() {
        var field = GridObject.GrassField(tmp, tmp);
        GridField.Init(tmp, tmp);
        ClickManager.Instance.OnClick -= OnClick;
        ClickManager.Instance.OnClick += OnClick;
        unit.MoveImmidiately(new Vector2Int(tmp / 2, tmp / 2));
        unit.AttackTarget().Forget();
    }

    private void OnClick(Vector2Int v2i)
    {
        if (LastClicked == null)
        {
            LastClicked = GridField.GetOccupied(v2i);
        }

        if (LastClicked)
        {
            unit.Target = LastClicked;
        }
        unit.SetPath(v2i);
    }

    private void Update() {
        Tile.DrawTileGizmos = DrawTileGizmo;

        if(sample == null)
        {
            sample = Addressables.InstantiateAsync("Slime_3").WaitForCompletion().GetComponent<Monster>();
        }
    }
}
