using UnityEngine;

public class Game : MonoBehaviour {
    public Unit unit;

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
        GridField.Instance.Init(tmp, tmp);
        field.OnClick = unit.SetPath;
        unit.MoveImmidiately(new Vector2Int(tmp / 2, tmp / 2));
    }
}
