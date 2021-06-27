using System.Threading.Tasks;
using UnityEngine;
//using UnityEngine.AddressableAssets;

public class Floor {
    public static readonly string AddressPrefix = "Floor";

    public static async Task Init() {
        //var req = Addressables.LoadAssetAsync<Sprite>(GetFloorPath(0, 0, 0));
        //var sprite = await req.Task;
        //Size = sprite.rect.size.x / sprite.pixelsPerUnit;
    }

    public static float Size { get; private set; }

    public enum ShapeType {
        TopLeft, Top, TopRight, /*          */VirticalTop,/*                    */ Single,
        Left, Center, Right, /*             */VirticalCenter, HorizontalLeft, HorizontalCenter, HorizontalRight,
        BottomLeft, Bottom, BottomRight, /* */VirticalBottom
    }

    public enum Brightness {
        Brighter,
        Bright,
        Dark,
        Darker
    }

    public enum MaterialType {
        Brick, Grass, Bush,
        Dirt, Wood, Lock,
        Soil
    }

    public static int GetFloorNumber(ShapeType floorShapeType, Brightness floorBrightness, MaterialType floorMaterialType) {
        var start = 32;
        var shapeCount = (int)ShapeType.VirticalBottom + 1;
        var skipShapeCount = Mathf.Max((int)floorMaterialType / 3, 0) * ((int)Brightness.Darker + 1) * shapeCount * 3;
        skipShapeCount += Mathf.Max((int)floorBrightness - 1, 0) * 3 * shapeCount;
        var skipLines = 0;
        var currentLine = (int)ShapeType.Single + 1;
        var smallSkip = (int)floorShapeType;
        if (floorShapeType > ShapeType.HorizontalRight) {
            skipLines = (int)ShapeType.HorizontalRight + 1;
            currentLine = ShapeType.VirticalBottom - ShapeType.HorizontalRight;
            smallSkip = floorShapeType - ShapeType.BottomLeft;
        } else if (floorShapeType > ShapeType.Single) {
            skipLines = (int)ShapeType.Single + 1;
            currentLine = ShapeType.HorizontalRight - ShapeType.Single;
            smallSkip = floorShapeType - ShapeType.Left;
        }

        return start + skipShapeCount + skipLines * 3 + currentLine * (Mathf.Max((int)floorMaterialType % 3, 0)) + smallSkip;
    }

    public static string GetFloorPath(ShapeType floorShapeType, Brightness floorBrightness, MaterialType floorMaterialType) {
        return $"{AddressPrefix}[{AddressPrefix}_{GetFloorNumber(floorShapeType, floorBrightness, floorMaterialType).ToString()}]";
    }

}
