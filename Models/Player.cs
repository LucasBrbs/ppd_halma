namespace PPD_Sockets.Models
{
    public class Player
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public PlayerColor Color { get; set; }
        public List<GamePiece> Pieces { get; set; }
        public bool IsConnected { get; set; }

        public Player(string id, string name, PlayerColor color)
        {
            Id = id;
            Name = name;
            Color = color;
            Pieces = new List<GamePiece>();
            IsConnected = true;
            InitializePieces();
        }

        private void InitializePieces()
        {
            // Inicializa as 19 peças do jogador
            // Jogador preto começa no canto superior esquerdo
            if (Color == PlayerColor.Black)
            {
                // Triângulo no canto superior esquerdo (0,0 até 3,3)
                for (int x = 0; x <= 3; x++)
                {
                    for (int y = 0; y <= 3 - x; y++)
                    {
                        Pieces.Add(new GamePiece(Color, new Position(x, y)));
                    }
                }
            }
            // Jogador branco começa no canto inferior direito
            else if (Color == PlayerColor.White)
            {
                // Triângulo no canto inferior direito (12,12 até 15,15)
                for (int x = 12; x <= 15; x++)
                {
                    for (int y = 12 + (x - 12); y <= 15; y++)
                    {
                        Pieces.Add(new GamePiece(Color, new Position(x, y)));
                    }
                }
            }
        }

        public bool HasWon()
        {
            // Verifica se todas as peças estão na zona alvo
            return Pieces.All(piece => piece.IsInTargetZone);
        }

        public GamePiece? GetPieceAt(Position position)
        {
            return Pieces.FirstOrDefault(piece => piece.Position.Equals(position));
        }

        public int PiecesInTargetZone()
        {
            return Pieces.Count(piece => piece.IsInTargetZone);
        }

        public override string ToString()
        {
            return $"Player {Name} ({Color}) - {PiecesInTargetZone()}/19 pieces in target";
        }
    }
}
