using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public enum TileType {
    None,
    Floor,
    Grass,
    Block,
    Water
}

public class Tile : MonoBehaviour {

    public TileType tileType;
    public Vector2Int GridPosition { get; private set; }
    public Action<Tile> OnClick;
    private BoxCollider2D boxCollider;

    public static Tile Create(Floor.ShapeType shape, Floor.Brightness bright, Floor.MaterialType material, Vector2Int position, Transform parent, TileType tileType = TileType.None) {
        var tile = new GameObject("tile");
        tile.transform.parent = parent;
        var ret = tile.AddComponent<Tile>();
        var sr = tile.AddComponent<SpriteRenderer>();
        var path = Floor.GetFloorPath(shape, bright, material);
        Addressables.LoadAssetAsync<Sprite>(path).Completed += handle => {
            if (handle.Status == AsyncOperationStatus.Succeeded) {
                sr.sprite = handle.Result;
                sr.sortingOrder = -1;
                ret.boxCollider = tile.AddComponent<BoxCollider2D>();
                ret.GridPosition = position;
                tile.transform.position = GridField.Convert(position);
                ret.tileType = tileType;
            }
        };
        return ret;
    }

    private void OnMouseDown() {
        OnClick?.Invoke(this);
        Debug.Log($"{this} ckicked");
    }

    public static bool DrawTileGizmos = true;

    public void OnDrawGizmos() {
        if(DrawTileGizmos == false) {
            return;
        }

        if (boxCollider) {
            Gizmos.color = GetTileGizmoColor();
            Gizmos.DrawCube(transform.position, boxCollider.bounds.size);
        }
    }


    private Color GetTileGizmoColor() {
        if (GridField.IsMovable(GridPosition) == false) {
            return new Color(1, 0, 0, 0.5f);
        }

        switch (tileType) {
            case TileType.Block:
                return new Color(1, 0, 0, 0.5f);
            case TileType.Water:
                return new Color(0, 0, 1, 0.5f);
            default:
                return new Color(0, 1, 0, 0.5f);
        }
    }
}
