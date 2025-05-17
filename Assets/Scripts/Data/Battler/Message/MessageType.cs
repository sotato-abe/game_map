public enum MessageType
{
        Encount,  //遭遇
        Attack,   //攻撃
        Recovery,   //回復        
        Damage,   //ダメージ 
        Escape,   //逃亡
        Win,     //勝利
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
            case MessageType.Recovery:
                return "これで大丈夫";
            case MessageType.Damage:
                return "いてぇ";
            case MessageType.Escape:
                return "にげろ !";
            case MessageType.Win:
                return "よし";
            case MessageType.Lose:
                return "くそぅ";
            default:
                return "";
        }
    }
}