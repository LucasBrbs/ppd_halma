namespace PPD_Sockets.Models
{
    public enum PlayerColor
    {
        Black,
        White
    }

    public enum MoveType
    {
        Adjacent,   // Movimento para casa adjacente
        Jump       // Salto sobre uma ou mais peças
    }

    public enum GameState
    {
        WaitingForPlayers,
        InProgress,
        Finished
    }

    public enum MessageType
    {
        // Mensagens de conexão
        PlayerJoin,
        PlayerLeave,
        GameStart,
        
        // Mensagens de jogo
        Move,
        MoveResponse,
        GameState,
        TurnChange,
        GameEnd,
        
        // Mensagens de chat
        ChatMessage,
        
        // Mensagens de erro
        Error,
        InvalidMove
    }
}
