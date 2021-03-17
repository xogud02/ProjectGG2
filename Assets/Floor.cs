using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Floor {

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

    public static int GetFlooNumber(FloorShapeType floorShapeType, FloorBrightness floorBrightness, FloorMaterialType floorMaterialType) {
        var start = 32;
        var shapeCount = (int)FloorShapeType.VirticalBottom + 1;
        var skipShapeCount = Mathf.Clamp((int)floorMaterialType / 3 - 1, 0, 2) * ((int)FloorBrightness.Darker + 1) * shapeCount;
        skipShapeCount += Mathf.Clamp((int)floorBrightness - 1, 0, 3) * 3 * shapeCount;
        var skipLine = 0;
        if (floorShapeType > FloorShapeType.HorizontalRight) {
            skipLine = (int)FloorShapeType.HorizontalRight;
        } else if (floorShapeType > FloorShapeType.Single) {
            skipLine = (int)FloorShapeType.Single;
        }

        return start + skipShapeCount;
    }

}
