using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridField {
    private GridField() { }
    private static GridField _instance;
    public static GridField Instance {
        get {
            if(_instance == null) {
                _instance = new GridField();
            }
            return _instance;
        }
    }
    private HashSet<GridObject> objects = new HashSet<GridObject>();
    public static Action<Vector2Int> OnClick;
    public static bool IsMovable(Vector2Int _) => false;
    public static int Width => 100;
    public static int Height => 100;
    public static TileType GetTileType(Vector2Int _) => TileType.Block;
    public static void AddObject(GridObject gridObject) {
        Instance.objects.Add(gridObject);
    }
}