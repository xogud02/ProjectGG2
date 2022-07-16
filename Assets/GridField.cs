using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GridField
{

    private static readonly HashSet<GridObject> objects = new HashSet<GridObject>();
    private static readonly Dictionary<Vector2Int, Unit> occupied = new Dictionary<Vector2Int, Unit>();

    public static bool IsInRange(Vector2Int pos) => 0 <= pos.x && pos.x < Width && 0 <= pos.y && pos.y < Height;
    public static bool IsMovable(Vector2Int _, bool allowOccupied = false) => IsInRange(_) && GetTileType(_) != TileType.Block && (allowOccupied || occupied.ContainsKey(_) == false);
    public static bool IsMovable(GridPositionHandle _, bool allowOccupied = false) => IsMovable(_.WorldPosition, allowOccupied);
    public static int Width { get; private set; }
    public static int Height { get; private set; }
    public static TileType GetTileType(Vector2Int _)
    {
        foreach (var obj in objects)
        {
            var tileType = obj.GetTile(_);
            if (tileType != TileType.None)
            {
                return tileType;
            }
        }
        return TileType.None;
    }

    public static Vector2 Convert(GridPositionHandle handle) => Convert(handle.WorldPosition);
    public static Vector2 Convert(Vector2Int gridPosition) => new Vector2(gridPosition.x, gridPosition.y) * SingleTileSize;

    public static float SingleTileSize => Floor.Size * GridObject.ScaleFactor;

    public static void AddObject(GridObject gridObject)
    {
        objects.Add(gridObject);
    }

    public static bool Occupy(this Unit unit, Vector2Int position)
    {
        if (occupied.ContainsKey(position))
        {
            return false;
        }

        occupied.Add(position, unit);
        return true;
    }

    public static Unit GetOccupied(GridPositionHandle position) => occupied.TryGetValue(position.WorldPosition, out var ret) ? ret : null;

    public static Unit UnOccupy(Vector2Int position)
    {
        if (occupied.ContainsKey(position) == false)
        {
            return null;
        }

        var ret = occupied[position];
        occupied.Remove(position);
        return ret;
    }

    public static IEnumerable<Unit> GetOccupied(Vector2Int position, float range = 1f) =>
        occupied.Where(kvp => (kvp.Key - position).magnitude <= range).Select(kvp => kvp.Value).Distinct();


    public static void Init(int width, int height)
    {
        Width = width;
        Height = height;
    }
}
