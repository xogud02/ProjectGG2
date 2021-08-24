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

    private void Init() {
        var tmp = 30;
        var field = GridObject.GrassField(tmp, tmp);
        GridField.Instance.Init(tmp, tmp);
        field.OnClick = unit.SetPath;
    }
}
