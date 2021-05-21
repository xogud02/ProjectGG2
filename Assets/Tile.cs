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
    public Action<Tile> OnClicked;
    public Vector2Int GridPosition { get; private set; }

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
                tile.AddComponent<BoxCollider2D>();
                ret.GridPosition = position;
                tile.transform.position = new Vector3(position.x, position.y) * Floor.Size * GridObject.ScaleFactor;
                ret.tileType = tileType;
            }
        };
        return ret;
    }

    private void OnMouseDown() {
        OnClicked?.Invoke(this);
    }
}
