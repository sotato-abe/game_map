using System.Collections.Generic;

public class FieldData
{
    public GroundType groundType; // グラウンドタイプ
    public FloorType floorType; // フロアタイプ
    public DirectionType roadDirection; // 道
    public SpotType spot; // スポット（スポットが該当しないときは０）
    public Coordinate coordinate; // 座標
}
