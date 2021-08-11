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
        var field = GridObject.GrassField(10, 10);
        GridField.Instance.Init(10, 10);
        field.OnClick = unit.SetPath;
    }
}