using UnityEngine;

public class Game : MonoBehaviour {
    void Start() {
        if (GridObject.Initialized) {
            GridObject.GrassField(10, 10);
        } else {
            GridObject.OnInit = () => GridObject.GrassField(10, 10);
        }
    }
}
