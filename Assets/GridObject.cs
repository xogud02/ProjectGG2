using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class GridObject : MonoBehaviour
{
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

    private Tilemap tileMap;

    private Dictionary<Vector2Int, Tile> tiles = new Dictionary<Vector2Int, Tile>();

    static GridObject()
    {
        Floor.Init().GetAwaiter().OnCompleted(Init);
    }

    private static void Init()
    {
        ScaleFactor = 3;
        _onInit.Invoke();
        Initialized = true;
    }

    public Vector2Int GridPosition;

    public TileType GetTile(Vector2Int pos)
    {
        var relative = pos - GridPosition;
        if (tiles.ContainsKey(relative))
        {
            return tiles[relative].tileType;
        }
        return TileType.None;
    }

    public void AddTile(Tile tile, Vector2Int pos)
    {
        tiles[pos] = tile;
        tileMap.SetTile(new Vector3Int(pos.x, pos.y, 0), tile.ruleTile);
    }

    public static GridObject GrassField(int width, int height, RuleTile tile, float ratio = 0.3f)
    {
        var grass = new HashSet<Vector2Int>();
        for (int x = 0; x < width; ++x)
        {
            for (int y = 0; y < height; ++y)
            {
                if (Random.Range(0, 1f) > ratio)
                {
                    grass.Add(new Vector2Int(x, y));
                }
            }
        }

        var gameObject = new GameObject("grassField");
        var ret = gameObject.AddComponent<GridObject>();
        ret.tileMap = gameObject.AddComponent<Tilemap>();
        ret.tileMap.tileAnchor = Vector3.zero;
        var col =  gameObject.AddComponent<TilemapCollider2D>();
        col.isTrigger = true;

        for (int x = 0; x < width; ++x)
        {
            for (int y = 0; y < height; ++y)
            {

                var current = new Vector2Int(x, y);

                if (grass.Contains(current) == false)
                {
                    var dirt = Tile.Create(Floor.Brightness.Bright, Floor.MaterialType.Dirt, new GridPositionHandle(current, ret.GridPosition), gameObject.transform);
                    dirt.ruleTile = tile;//TODO
                    //ret.AddTile(dirt, current);
                    continue;
                }

                var grassTile = Tile.Create(Floor.Brightness.Bright, Floor.MaterialType.Grass, new GridPositionHandle(current, ret.GridPosition), gameObject.transform);
                grassTile.ruleTile = tile;//TODO
                ret.AddTile(grassTile, current);

            }

        }
        gameObject.transform.localScale *= ScaleFactor;
        return ret;
    }

    private void OnMouseDown()
    {
        var pos = Input.mousePosition.STW().Convert();
        ClickManager.Instance.Click(new GridPositionHandle(GridField.Convert(pos)));
    }
}
