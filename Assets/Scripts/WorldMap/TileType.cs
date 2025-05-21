public enum TileType
{
    Base,
    Ground,
    Floor,
    Edge,
    Wall,
    Entry,
    Object,
    Building,
    Kiosk,
    Cafeteria,
    ArmsShop,
    Laboratory,
    Hotel,
}

public static class TileTypeExtensions
{
    public static BuildingType ConverTileType(this TileType tileType)
    {
        switch (tileType)
        {
            case TileType.Building:
                return BuildingType.Building;
            case TileType.Kiosk:
                return BuildingType.Kiosk;
            case TileType.ArmsShop:
                return BuildingType.ArmsShop;
            case TileType.Cafeteria:
                return BuildingType.Cafeteria;
            case TileType.Laboratory:
                return BuildingType.Laboratory;
            case TileType.Hotel:
                return BuildingType.Hotel;
            default:
                return BuildingType.Building;
        }
    }
}