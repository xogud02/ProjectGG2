using System.Collections;
using System.Collections.Generic;
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

}
