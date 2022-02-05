using Cysharp.Threading.Tasks;
using System.Collections;
using System.Linq;
using UnityEngine;

public class Game : MonoBehaviour {
    public Unit unit;
    private Unit _target;

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
        field.OnClick = v2i => {
            var target = GridField.GetOccupied(v2i);
            if (target) {
                _target = target;
                return;
            }
            unit.SetPath(v2i);
        };
        unit.MoveImmidiately(new Vector2Int(tmp / 2, tmp / 2));
        FindTarget().Forget();
        AttackTarget().Forget();
    }

    private async UniTask FindTarget() {
        while (true) {
            await UniTask.Yield();
            if (_target != null) {
                continue;
            }

            _target = GridField.GetOccupied(unit.CurrentPosition, 2).FirstOrDefault(_ => _ != unit);
        }
    }

    private async UniTask AttackTarget() {
        while (true) {
            await UniTask.Yield();
            if (_target == null) {
                continue;
            }

            if (_target.IsInRange(unit)) {
                await _target.Hit(unit);
            }
        }
    }
}
