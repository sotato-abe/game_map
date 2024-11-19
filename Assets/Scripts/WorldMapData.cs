using System;
using System.Collections.Generic;

[Serializable]
public class WorldMapData
{
    public int height = 100;
    public int width = 40;
    // 各レイヤーのデータ

    [Serializable]
    public class LayerData
    {
        public LayerType layerType;  // レイヤータイプ
        public int[,] tiles;         // タイルマップ（例: タイルIDなど）
    }
}
