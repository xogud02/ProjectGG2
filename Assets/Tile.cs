using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Tile : MonoBehaviour {
    void Start() {
        var temp = new GameObject("foo");
        var sr = temp.AddComponent<SpriteRenderer>();
        var path = Floor.GetFloorPath(Floor.FloorShapeType.Center, Floor.FloorBrightness.Bright, Floor.FloorMaterialType.Grass);
        Addressables.LoadAssetAsync<Sprite>(path).Completed += handle => {
            if(handle.Status == AsyncOperationStatus.Succeeded) {
                sr.sprite = handle.Result;
            }
        };
    }

    void Update() {

    }
}
