using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridField {
    private GridField() { }
    private static GridField _instance;
    public static GridField Instance {
        get {
            if (_instance == null) {
                _instance = new GridField();
            }
            return _instance;
        }
    }
    private HashSet<GridObject> objects = new HashSet<GridObject>();
    public static Action<Vector2Int> OnClick;
    public static bool IsInRange(Vector2Int pos) => 0 <= pos.x && pos.x < Width && 0 <= pos.y && pos.y < Height;
    public static bool IsMovable(Vector2Int _) => IsInRange(_) && GetTileType(_) != TileType.Block;
    public static int Width => Instance.width;
    public static int Height => Instance.height;
    public static TileType GetTileType(Vector2Int _) {
        foreach(var obj in Instance.objects) {
            if(_ == obj.GridPosition) {
            }
        }
        return TileType.Block;
    }
    public static void AddObject(GridObject gridObject) {
        Instance.objects.Add(gridObject);
    }

    private int width, height;
    public void Init(int width, int height) {
        this.width = width;
        this.height = height;
    }
}
