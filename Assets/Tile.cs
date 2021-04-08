using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Tile : MonoBehaviour {
    public enum AdjType {
        None = 0,
        Up = 1 << 0,
        Down = 1 << 1,
        Left = 1 << 2,
        Right = 1 << 3
    }
    private float size = 0f;
    void Start() {
        GrassField(10, 10);
    }

    private GameObject GrassField(int width, int height, float ratio = 0.3f) {
        var grass = new HashSet<Vector2Int>();
        for (int x = 0; x < width; ++x) {
            for (int y = 0; y < height; ++y) {
                if (Random.Range(0, 1f) > ratio) {
                    grass.Add(new Vector2Int(x, y));
                }
            }
        }

        var ret = new GameObject("grassField");
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
                var newPos = new Vector2(x + dx, y + dy);
                var tile = new GameObject("");
                tile.transform.parent = ret.transform;
                var sr = tile.AddComponent<SpriteRenderer>();
                AdjType adj = AdjType.None;
                foreach(var kvp in dic) {
                    if (grass.Contains(kvp.Key)) {
                        adj |= kvp.Value;
                    }
                }

                if (grass.Contains(new Vector2Int(x, y)) == false) {
                    var path = Floor.GetFloorPath(Floor.FloorShapeType.Center, Floor.FloorBrightness.Bright, Floor.FloorMaterialType.Dirt);
                    Addressables.LoadAssetAsync<Sprite>(path).Completed += handle => {
                        if (handle.Status == AsyncOperationStatus.Succeeded) {
                            sr.sprite = handle.Result;
                            size = sr.size.x;
                            tile.transform.position = newPos * size;
                        }
                    };
                    continue;
                }
                //TODO grass shape
                var path2 = Floor.GetFloorPath(Floor.FloorShapeType.Single, Floor.FloorBrightness.Bright, Floor.FloorMaterialType.Grass);
                Addressables.LoadAssetAsync<Sprite>(path2).Completed += handle => {
                    if (handle.Status == AsyncOperationStatus.Succeeded) {
                        sr.sprite = handle.Result;
                        size = sr.size.x;
                        tile.transform.position = newPos * size;
                    }
                };
            }
        }

        return ret;
    }
}
