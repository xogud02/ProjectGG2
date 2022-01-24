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
    }
}
