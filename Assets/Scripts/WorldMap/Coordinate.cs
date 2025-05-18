public class Coordinate
{
    public int row; // 行数
    public int col; // 列数

    // 通常のコンストラクタ
    public Coordinate(int row, int col)
    {
        this.row = row;
        this.col = col;
    }

    // コピーコンストラクタ
    public Coordinate(Coordinate other)
    {
        this.row = other.row;
        this.col = other.col;
    }

    public override bool Equals(object obj)
    {
        if (obj is Coordinate other)
        {
            return this.row == other.row && this.col == other.col;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return row * 31 + col;
    }
}