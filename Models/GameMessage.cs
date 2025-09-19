namespace PPD_Sockets.Models
{
    public class GameMessage
    {
        public string Type { get; set; } = "";
        public string Data { get; set; } = "";
        public string PlayerName { get; set; } = "";
        
        public GameMessage() { }
        
        public GameMessage(string type, string data, string playerName = "")
        {
            Type = type;
            Data = data;
            PlayerName = playerName;
        }
        
        // Converte a mensagem para string para enviar via socket
        public override string ToString()
        {
            return $"{Type}|{PlayerName}|{Data}";
        }
        
        // Cria uma mensagem a partir de string recebida via socket
        public static GameMessage? FromString(string messageStr)
        {
            try
            {
                string[] parts = messageStr.Split('|', 3);
                if (parts.Length >= 2)
                {
                    return new GameMessage
                    {
                        Type = parts[0],
                        PlayerName = parts.Length > 1 ? parts[1] : "",
                        Data = parts.Length > 2 ? parts[2] : ""
                    };
                }
            }
            catch
            {
                // Se der erro na conversão, retorna null
            }
            
            return null;
        }
    }
    
    // Tipos de mensagem que vamos usar
    public static class MessageTypes
    {
        public const string PLAYER_JOIN = "PLAYER_JOIN";
        public const string GAME_START = "GAME_START";
        public const string MOVE = "MOVE";
        public const string BOARD_UPDATE = "BOARD_UPDATE";
        public const string TURN_CHANGE = "TURN_CHANGE";
        public const string GAME_END = "GAME_END";
        public const string CHAT = "CHAT";
        public const string ERROR = "ERROR";
    }
}
