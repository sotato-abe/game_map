public enum RoadTileType
{
        None,           // なし
        Up,             // 上 （マップ上のタイルはなし）
        Right,          // 右 （マップ上のタイルはなし）
        Bottom,         // 下 （マップ上のタイルはなし）
        Left,           // 左 （マップ上のタイルはなし）
        UpBottom,       // 上下 554
        RightLeft,      // 左右 553
        UpRight,        // 上右 557
        UpLeft,         // 上左 558
        BottomRigth,    // 下右 555
        BottomLeft,     // 下左 556
        RightBottomLeft,// 右下左 559
        UpBottomLeft,   // 上下左 561
        UpRightLeft,    // 上右左 560
        UpRightBottom,  // 上右下 562
        All,            // 十字 563
}