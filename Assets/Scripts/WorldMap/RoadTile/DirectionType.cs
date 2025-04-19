public enum DirectionType
{
    None = 0,
    Top = 1 << 0,        // 1
    Right = 1 << 1,      // 2
    Bottom = 1 << 2,     // 4
    Left = 1 << 3,       // 8
    TopRight = Top | Right,                // 3
    TopBottom = Top | Bottom,              // 5
    TopLeft = Top | Left,                  // 9
    RightBottom = Right | Bottom,          // 6
    RightLeft = Right | Left,              // 10
    BottomLeft = Bottom | Left,            // 12
    TopRightBottom = Top | Right | Bottom, // 7
    TopRightLeft = Top | Right | Left,     // 11
    TopBottomLeft = Top | Bottom | Left,   // 13
    RightBottomLeft = Right | Bottom | Left, // 14
    Cross = Top | Right | Bottom | Left
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

    public static DirectionType DirectionMarge(bool top, bool bottom, bool right, bool left)
    {
        if (top && bottom && right && left) return DirectionType.Cross;
        if (top && bottom && right) return DirectionType.TopRightBottom;
        if (top && bottom && left) return DirectionType.TopBottomLeft;
        if (top && right && left) return DirectionType.TopRightLeft;
        if (bottom && right && left) return DirectionType.RightBottomLeft;
        if (top && bottom) return DirectionType.TopBottom;
        if (top && right) return DirectionType.TopRight;
        if (top && left) return DirectionType.TopLeft;
        if (bottom && right) return DirectionType.RightBottom;
        if (bottom && left) return DirectionType.BottomLeft;
        if (right && left) return DirectionType.RightLeft;

        if (top) return DirectionType.Top;
        if (bottom) return DirectionType.Bottom;
        if (right) return DirectionType.Right;
        if (left) return DirectionType.Left;

        return DirectionType.None; // すべてfalseの場合
    }

    public static bool hasDirection(this DirectionType self, DirectionType target)
    {
        return (self & target) == target;
    }
}