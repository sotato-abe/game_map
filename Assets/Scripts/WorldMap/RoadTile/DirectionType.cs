public enum DirectionType
{
        None,           //　０：なし
        Top,            //　１：上
        Right,          //　２：右
        Bottom,         //　３：下
        Left,           //　４：左
        TopRight,       //　５：上右 557
        TopBottom,      //　６：上下 558
        TopLeft,        //　７：上左 554
        RightBottom,    //　８：右下 555
        RightLeft,      //　９：右左 553
        BottomLeft,     //　10：下左 556
        TopRightBottom, //　11：上右下 562
        TopRightLeft,   //　12：上右左 560
        TopBottomLeft,  //　13：上下左 561
        RightBottomLeft,//　14：右下左 559
        Cross,          //　15：十字 563
}

// 拡張メソッドの定義
public static class DirectionTypeExtensions
{
    public static DirectionType GetOppositeDirection(this DirectionType targetDirection)
    {
        return targetDirection switch
        {
            DirectionType.Top => DirectionType.Bottom,
            DirectionType.Right => DirectionType.Left,
            DirectionType.Bottom => DirectionType.Top,
            DirectionType.Left => DirectionType.Right,
            DirectionType.TopRight => DirectionType.BottomLeft,
            DirectionType.TopBottom => DirectionType.None, // 対応なし
            DirectionType.TopLeft => DirectionType.RightBottom,
            DirectionType.RightBottom => DirectionType.TopLeft,
            DirectionType.RightLeft => DirectionType.None, // 対応なし
            DirectionType.BottomLeft => DirectionType.TopRight,
            DirectionType.TopRightBottom => DirectionType.RightBottomLeft,
            DirectionType.TopRightLeft => DirectionType.TopBottomLeft,
            DirectionType.TopBottomLeft => DirectionType.TopRightLeft,
            DirectionType.RightBottomLeft => DirectionType.TopRightBottom,
            DirectionType.Cross => DirectionType.Cross,
            _ => DirectionType.None, // デフォルト
        };
    }
}