using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Tile : MonoBehaviour {
    private float size = 0f;
    void Start() {
        Foo(Floor.FloorShapeType.TopLeft, -1, -1);
        Foo(Floor.FloorShapeType.Top, -1, 0);
        Foo(Floor.FloorShapeType.TopRight, -1, 1);
        Foo(Floor.FloorShapeType.Left, 0, -1);
        Foo(Floor.FloorShapeType.Center, 0, 0);
        Foo(Floor.FloorShapeType.Right, 0, 1);
        Foo(Floor.FloorShapeType.BottomLeft, 1, -1);
        Foo(Floor.FloorShapeType.Bottom, 1, 0);
        Foo(Floor.FloorShapeType.BottomRight, 1, 1);

        void Foo(Floor.FloorShapeType shapeType, int row, int col) {
            var temp = new GameObject("foo");
            var sr = temp.AddComponent<SpriteRenderer>();
            var path = Floor.GetFloorPath(shapeType, Floor.FloorBrightness.Bright, Floor.FloorMaterialType.Grass);
            Addressables.LoadAssetAsync<Sprite>(path).Completed += handle => {
                if (handle.Status == AsyncOperationStatus.Succeeded) {
                    sr.sprite = handle.Result;
                    size = sr.size.x;
                    temp.transform.position = new Vector2(col, -row) * size;
                }
            };
        }
    }

    void Update() {

    }
}
