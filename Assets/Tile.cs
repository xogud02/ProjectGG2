using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Tile : MonoBehaviour {
    private float size = 0f;
    void Start() {
        Foo(Floor.FloorShapeType.TopLeft, -1, 1);
        Foo(Floor.FloorShapeType.Top, 0, 1);
        Foo(Floor.FloorShapeType.TopRight, 1, 1);
        Foo(Floor.FloorShapeType.Left, -1, 0);
        Foo(Floor.FloorShapeType.Center, 0, 0);
        Foo(Floor.FloorShapeType.Right, 1, 0);
        Foo(Floor.FloorShapeType.BottomLeft, -1, -1);
        Foo(Floor.FloorShapeType.Bottom, 0, -1);
        Foo(Floor.FloorShapeType.BottomRight, 1, -1);

        void Foo(Floor.FloorShapeType shapeType, int x, int y) {
            var temp = new GameObject("foo");
            var sr = temp.AddComponent<SpriteRenderer>();
            var path = Floor.GetFloorPath(shapeType, Floor.FloorBrightness.Bright, Floor.FloorMaterialType.Grass);
            Addressables.LoadAssetAsync<Sprite>(path).Completed += handle => {
                if (handle.Status == AsyncOperationStatus.Succeeded) {
                    sr.sprite = handle.Result;
                    size = sr.size.x;
                    temp.transform.position = new Vector2(x, y) * size;
                }
            };
        }
    }

    private GameObject GrassField(int width, int height, float ratio = 0.3f) {
        var grass = new HashSet<Vector2Int>();
        for(int x = 0; x < width; ++x) {
            for(int y = 0; y < height; ++y) {
                if (Random.Range(0, 1) > ratio) {
                    grass.Add(new Vector2Int(x, y));
                }
            }
        }
        var dx = -width / 2f;
        var dy = -height / 2f;
        for(int x = 0; x < width; ++x) {
            for(int y = 0; y < height; ++y) {

            }
        }

        var ret = new GameObject("grassField");
        return ret;
    }
}
