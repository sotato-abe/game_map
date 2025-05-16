public enum MessageType
{
        Encount,  //遭遇
        Attack,   //攻撃
        Damage,   //ダメージ 
        Escape,   //逃亡
        Lose,     //敗北
}

public static class MessageTypeExtensions
{
    public static string GetDefaultMessage(this MessageType messageType)
    {
        switch (messageType)
        {
            case MessageType.Encount:
                return "へっへっへ";
            case MessageType.Attack:
                return "くらえ";
            case MessageType.Damage:
                return "いてぇ";
            case MessageType.Escape:
                return "にげろ !";
            case MessageType.Lose:
                return "くそぅ";
            default:
                return "";
        }
    }
}