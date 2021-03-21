using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Floor {

    public static readonly string AddressPrefix = "Floor_";

    public enum FloorShapeType {
        TopLeft, Top, TopRight, /*          */VirticalTop,/*                    */ Single,
        Left, Center, Right, /*             */VirticalCenter, HorizontalLeft, HorizontalCenter, HorizontalRight,
        BottomLeft, Bottom, BottomRight, /* */VirticalBottom
    }

    public enum FloorBrightness {
        Brighter,
        Bright,
        Dark,
        Darker
    }

    public enum FloorMaterialType {
        Brick, Grass, Bush,
        Dirt, Wood, Lock,
        Soil
    }

    public static int GetFloorNumber(FloorShapeType floorShapeType, FloorBrightness floorBrightness, FloorMaterialType floorMaterialType) {
        var start = 32;
        var shapeCount = (int)FloorShapeType.VirticalBottom + 1;
        var skipShapeCount = Mathf.Max((int)floorMaterialType / 3 - 1, 0) * ((int)FloorBrightness.Darker + 1) * shapeCount;
        skipShapeCount += Mathf.Max((int)floorBrightness - 1, 0) * 3 * shapeCount;
        var skipLines = 0;
        var currentLine = (int)FloorShapeType.Single + 1;
        var smallSkip = (int)floorShapeType;
        if (floorShapeType > FloorShapeType.HorizontalRight) {
            skipLines = (int)FloorShapeType.HorizontalRight + 1;
            currentLine = FloorShapeType.VirticalBottom - FloorShapeType.HorizontalRight;
            smallSkip = floorShapeType - FloorShapeType.Left;
        } else if (floorShapeType > FloorShapeType.Single) {
            skipLines = (int)FloorShapeType.Single + 1;
            currentLine = FloorShapeType.HorizontalRight - FloorShapeType.Single;
            smallSkip = floorShapeType - FloorShapeType.BottomLeft;
        }


        return start + skipShapeCount + skipLines * 3 + currentLine * (Mathf.Max((int)floorMaterialType % 3 - 1, 0)) + smallSkip;
    }

    public static string GetFloorPath(FloorShapeType floorShapeType, FloorBrightness floorBrightness, FloorMaterialType floorMaterialType) {
        return $"{AddressPrefix}{GetFloorNumber(floorShapeType, floorBrightness, floorMaterialType).ToString()}";
    }

}
