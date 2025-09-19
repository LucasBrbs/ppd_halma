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
            // Inicializa as peças do jogador em formato de escada triangular
            if (Color == PlayerColor.Black)
            {
                // Jogador preto começa no canto INFERIOR DIREITO 
                // Escada aponta para o objetivo (superior esquerdo)
                // Formato: 1+2+3+4 = 10 peças (linha 12=1, linha 13=2, linha 14=3, linha 15=4)
                for (int linha = 0; linha < 4; linha++)
                {
                    int pecasNaLinha = linha + 1; // linha 0 = 1 peça, linha 1 = 2 peças, etc.
                    for (int coluna = 0; coluna < pecasNaLinha; coluna++)
                    {
                        int x = 15 - coluna;  // Da direita para esquerda
                        int y = 12 + linha;   // Linha 12, 13, 14, 15
                        Pieces.Add(new GamePiece(Color, new Position(x, y)));
                    }
                }
            }
            else if (Color == PlayerColor.White)
            {
                // Jogador branco começa no canto SUPERIOR ESQUERDO
                // Escada aponta para o objetivo (inferior direito) 
                // Formato: 4+3+2+1 = 10 peças (começando com 4, terminando com 1)
                for (int linha = 0; linha < 4; linha++)
                {
                    int pecasNaLinha = 4 - linha; // linha 0 = 4 peças, linha 1 = 3 peças, etc.
                    for (int coluna = 0; coluna < pecasNaLinha; coluna++)
                    {
                        int x = coluna;  // Da esquerda para direita
                        int y = linha;   // De cima para baixo
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
