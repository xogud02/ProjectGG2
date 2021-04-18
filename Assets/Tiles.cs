using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Tiles : MonoBehaviour
{

    public enum AdjType {
        None = 0,
        Up = 1 << 0,
        Down = 1 << 1,
        Left = 1 << 2,
        Right = 1 << 3
    }

    void Start() {
        var awaiter = Floor.Init().GetAwaiter();
        awaiter.OnCompleted(() => GrassField(10, 10));
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

        var arr = new Floor.ShapeType[] {      //rldu
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

        for (int x = 0; x < width; ++x) {
            for (int y = 0; y < height; ++y) {
                var newPos = new Vector2(x + dx, y + dy);
                var current = new Vector2Int(x, y);
                AdjType adj = AdjType.None;
                foreach (var kvp in dic) {
                    if (grass.Contains(kvp.Key + current)) {
                        adj |= kvp.Value;
                    }
                }

                if (grass.Contains(new Vector2Int(x, y)) == false) {
                    Tile.Create(Floor.ShapeType.Center, Floor.Brightness.Bright, Floor.MaterialType.Dirt, newPos * Floor.Size, ret.transform);
                    continue;
                }

                Tile.Create(arr[(int)adj], Floor.Brightness.Bright, Floor.MaterialType.Grass, newPos * Floor.Size, ret.transform);
            }
        }

        return ret;
    }
}
