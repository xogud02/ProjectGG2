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
        var arr = new Floor.FloorShapeType[] {      //rldu
            Floor.FloorShapeType.Single,            //0000
            Floor.FloorShapeType.VirticalBottom,    //0001
            Floor.FloorShapeType.VirticalTop,       //0010
            Floor.FloorShapeType.VirticalCenter,    //0011
            Floor.FloorShapeType.HorizontalRight,   //0100
            Floor.FloorShapeType.BottomRight,       //0101
            Floor.FloorShapeType.TopRight,          //0110
            Floor.FloorShapeType.Right,             //0111
            Floor.FloorShapeType.HorizontalLeft,    //1000
            Floor.FloorShapeType.BottomLeft,        //1001
            Floor.FloorShapeType.TopLeft,           //1010
            Floor.FloorShapeType.Left,              //1011
            Floor.FloorShapeType.HorizontalCenter,  //1100
            Floor.FloorShapeType.Bottom,            //1101
            Floor.FloorShapeType.Top,               //1110
            Floor.FloorShapeType.Center,            //1111

        };
        for (int x = 0; x < width; ++x) {
            for (int y = 0; y < height; ++y) {
                var newPos = new Vector2(x + dx, y + dy);
                var current = new Vector2Int(x, y);
                var tile = new GameObject("");
                tile.transform.parent = ret.transform;
                var sr = tile.AddComponent<SpriteRenderer>();
                AdjType adj = AdjType.None;
                foreach (var kvp in dic) {
                    if (grass.Contains(kvp.Key + current)) {
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
                var path2 = Floor.GetFloorPath(arr[(int)adj], Floor.FloorBrightness.Bright, Floor.FloorMaterialType.Grass);
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
