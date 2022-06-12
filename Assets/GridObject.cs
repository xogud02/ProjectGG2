using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridObject : MonoBehaviour {
    public static float ScaleFactor { get; private set; }
    public static bool Initialized { get; private set; }
    public delegate void InitializeCallback();
    private static InitializeCallback _onInit;
    public static event InitializeCallback OnInit
    {
        add
        {
            if (Initialized)
            {
                value.Invoke();
                return;
            }

            _onInit += value;
        }

        remove
        {
            _onInit -= value;
        }
    }

    private Dictionary<Vector2Int, Tile> tiles = new Dictionary<Vector2Int, Tile>();

    static GridObject() {
        Floor.Init().GetAwaiter().OnCompleted(Init);
    }

    private static void Init() {
        ScaleFactor = 3;
        _onInit.Invoke();
        Initialized = true;
    }

    public enum AdjType {
        None = 0,
        Up = 1 << 0,
        Down = 1 << 1,
        Left = 1 << 2,
        Right = 1 << 3
    }

    public Vector2Int GridPosition;

    public static Floor.ShapeType[] FloorShapeTypes = {              //rldu
        Floor.ShapeType.Single,            //0000
        Floor.ShapeType.VirticalBottom,    //0001
        Floor.ShapeType.VirticalTop,       //0010
        Floor.ShapeType.VirticalCenter,    //0011
        Floor.ShapeType.HorizontalRight,   //0100
        Floor.ShapeType.BottomRight,       //0101
        Floor.ShapeType.TopRight,          //0110
        Floor.ShapeType.Right,             //0111
        Floor.ShapeType.HorizontalLeft,    //1000
        Floor.ShapeType.BottomLeft,        //1001
        Floor.ShapeType.TopLeft,           //1010
        Floor.ShapeType.Left,              //1011
        Floor.ShapeType.HorizontalCenter,  //1100
        Floor.ShapeType.Bottom,            //1101
        Floor.ShapeType.Top,               //1110
        Floor.ShapeType.Center,            //1111
    };

    public TileType GetTile(Vector2Int pos) {
        var relative = pos - GridPosition;
        if (tiles.ContainsKey(relative)) {
            return tiles[relative].tileType;
        }
        return TileType.None;
    }

    public void AddTile(Tile tile, Vector2Int pos) {
        tiles[pos] = tile;
    }

    public static GridObject GrassField(int width, int height, float ratio = 0.3f) {
        var grass = new HashSet<Vector2Int>();
        for (int x = 0; x < width; ++x) {
            for (int y = 0; y < height; ++y) {
                if (Random.Range(0, 1f) > ratio) {
                    grass.Add(new Vector2Int(x, y));
                }
            }
        }

        var gameObject = new GameObject("grassField");
        var ret = gameObject.AddComponent<GridObject>();
        var dx = -width / 2f;
        var dy = -height / 2f;

        var dic = new Dictionary<Vector2Int, AdjType> {
            {Vector2Int.up, AdjType.Up },
            {Vector2Int.down, AdjType.Down},
            {Vector2Int.left, AdjType.Left},
            {Vector2Int.right, AdjType.Right}
        };



        for (int x = 0; x < width; ++x) {
            for (int y = 0; y < height; ++y) {
                var newPos = new Vector2(x + dx, y + dy) * ScaleFactor;
                var current = new Vector2Int(x, y);
                AdjType adj = AdjType.None;
                foreach (var kvp in dic) {
                    if (grass.Contains(kvp.Key + current)) {
                        adj |= kvp.Value;
                    }
                }

                if (grass.Contains(new Vector2Int(x, y)) == false) {
                    var dirt = Tile.Create(Floor.ShapeType.Center, Floor.Brightness.Bright, Floor.MaterialType.Dirt, current, gameObject.transform);
                    dirt.OnClick = ret.OnTileClicked;
                    ret.AddTile(dirt, current);
                    continue;
                }

                var grassTile = Tile.Create(FloorShapeTypes[(int)adj], Floor.Brightness.Bright, Floor.MaterialType.Grass, current, gameObject.transform);
                ret.AddTile(grassTile, current);

                grassTile.OnClick = ret.OnTileClicked;
            }
        }
        gameObject.transform.localScale *= ScaleFactor;
        return ret;
    }

    public void OnTileClicked(Tile tile) => ClickManager.Instance.Click(GridPosition + tile.LocalPosition);
}
