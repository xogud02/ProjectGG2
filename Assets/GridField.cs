using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridField {
    private GridField() { }
    public static bool IsMovable(Vector2Int _) => false;
    public static int Width => 100;
    public static int Height => 100;

}
