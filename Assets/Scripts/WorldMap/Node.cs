using UnityEngine;
public class Node
{
    public Vector2Int Position;
    public bool Walkable;
    public int G;
    public int H;
    public int F => G + H;
    public Node Parent;

    public Node(Vector2Int pos, bool walkable)
    {
        Position = pos;
        Walkable = walkable;
    }
}
