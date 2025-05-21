public enum BuildingType
{
    Building,
    Kiosk,
    ArmsShop,
    Cafeteria,
    Laboratory,
    Hotel
}

public static class BuildingTypeExtensions
{
    public static TileType ConverTileType(this BuildingType buildingType)
    {
        switch (buildingType)
        {
            case BuildingType.Building:
                return TileType.Building;
            case BuildingType.Kiosk:
                return TileType.Kiosk;
            case BuildingType.ArmsShop:
                return TileType.ArmsShop;
            case BuildingType.Cafeteria:
                return TileType.Cafeteria;
            case BuildingType.Laboratory:
                return TileType.Laboratory;
            case BuildingType.Hotel:
                return TileType.Hotel;
            default:
                return TileType.Building;
        }
    }
}
