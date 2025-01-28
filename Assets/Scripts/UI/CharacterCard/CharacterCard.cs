using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(CanvasRenderer))]
public class CharacterCard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;  // 表示用のTextMeshProUGUIフィールド
    [SerializeField] private Image cardImage;  // 表示用のTextMeshProUGUIフィールド
    [SerializeField] private Color cardColor = Color.white; // カードの色
    [SerializeField] private float radius = 5f; // 角の半径
    [SerializeField] private Vector2 size = new Vector2(200f, 300f); // カードのサイズ

    private Texture2D cardTexture; // カードの背景テクスチャ
    private Material material;

    private void Awake()
    {
        // マテリアルを作成
        material = new Material(Shader.Find("UI/Default"));
        GetComponent<CanvasRenderer>().SetMaterial(material, cardTexture);
    }

    private void Update()
    {
        // メッシュを更新
        // UpdateMesh();
    }

    private void UpdateMesh()
    {
        var canvasRenderer = GetComponent<CanvasRenderer>();
        var vh = new VertexHelper();
        CreateRoundedRectMesh(vh);
        var mesh = new Mesh();
        vh.FillMesh(mesh);
        canvasRenderer.SetMesh(mesh);
    }

    private void CreateRoundedRectMesh(VertexHelper vh)
    {
        vh.Clear();

        // カードの描画領域を計算
        Vector4 rect = new Vector4(-size.x / 2, -size.y / 2, size.x / 2, size.y / 2);
        float clampedRadius = Mathf.Min(radius, size.x / 2, size.y / 2);

        // UV座標（基本は0～1範囲）
        Vector2 uvMin = Vector2.zero;
        Vector2 uvMax = Vector2.one;

        // カードの頂点と三角形を生成
        AddRoundedRect(vh, rect, clampedRadius, cardColor, uvMin, uvMax);
    }

    private void AddRoundedRect(VertexHelper vh, Vector4 rect, float radius, Color color, Vector2 uvMin, Vector2 uvMax)
    {
        vh.Clear();

        // カードの中心
        Vector2 center = new Vector2((rect.x + rect.z) / 2, (rect.y + rect.w) / 2);
        float width = rect.z - rect.x;
        float height = rect.w - rect.y;

        // 四隅の円弧の中心座標
        Vector2[] cornerCenters = new Vector2[4];
        cornerCenters[0] = new Vector2(rect.x + radius, rect.w - radius); // 左上
        cornerCenters[1] = new Vector2(rect.x + radius, rect.y + radius); // 左下
        cornerCenters[2] = new Vector2(rect.z - radius, rect.y + radius); // 右下
        cornerCenters[3] = new Vector2(rect.z - radius, rect.w - radius); // 右上

        // 四隅の円弧を分割するセグメント数
        int segments = 10;


        // 頂点を追加
        for (int i = 0; i < 4; i++)
        {
            float startAngle = i * 90f + 90; // 開始角度
            float endAngle = startAngle + 90f; // 終了角度

            for (int j = 0; j <= segments; j++)
            {
                float t = (float)j / segments;
                float angle = Mathf.Lerp(startAngle, endAngle, t) * Mathf.Deg2Rad;

                // 円弧上の座標を計算
                Vector3 point = new Vector3(
                    cornerCenters[i].x + radius * Mathf.Cos(angle),
                    cornerCenters[i].y + radius * Mathf.Sin(angle),
                    0f
                );

                // UV座標を計算
                Vector2 uv = new Vector2(
                    Mathf.Lerp(uvMin.x, uvMax.x, (point.x - rect.x) / width),
                    Mathf.Lerp(uvMin.y, uvMax.y, (point.y - rect.y) / height)
                );

                vh.AddVert(point, color, uv);

                // 三角形を作成
                if (i > 0 || j > 0)
                {
                    vh.AddTriangle(0, vh.currentVertCount - 1, vh.currentVertCount - 2);
                }
            }
        }
    }

    public void SetCharacter(Battler battler)
    {
        cardImage.sprite = battler.Base.Sprite;
        nameText.SetText(battler.Base.Name);
    }
}
