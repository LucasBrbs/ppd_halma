using PPD_Sockets.Models;

namespace PPD_Sockets.Game
{
    public class GameBoard
    {
        private const int BOARD_SIZE = 16;
        private GamePiece?[,] board;
        
        public GameBoard()
        {
            board = new GamePiece?[BOARD_SIZE, BOARD_SIZE];
        }

        public void InitializeBoard(Player player1, Player player2)
        {
            // Limpa o tabuleiro
            ClearBoard();
            
            // Coloca as peças dos jogadores no tabuleiro
            foreach (var piece in player1.Pieces)
            {
                PlacePiece(piece);
            }
            
            foreach (var piece in player2.Pieces)
            {
                PlacePiece(piece);
            }
        }

        private void ClearBoard()
        {
            for (int x = 0; x < BOARD_SIZE; x++)
            {
                for (int y = 0; y < BOARD_SIZE; y++)
                {
                    board[x, y] = null;
                }
            }
        }

        private void PlacePiece(GamePiece piece)
        {
            if (piece.Position.IsValid())
            {
                board[piece.Position.X, piece.Position.Y] = piece;
            }
        }

        public bool IsPositionEmpty(Position position)
        {
            if (!position.IsValid()) return false;
            return board[position.X, position.Y] == null;
        }

        public GamePiece? GetPieceAt(Position position)
        {
            if (!position.IsValid()) return null;
            return board[position.X, position.Y];
        }

        public bool MovePiece(Position from, Position to)
        {
            if (!from.IsValid() || !to.IsValid()) return false;
            if (!IsPositionEmpty(to)) return false;
            
            var piece = GetPieceAt(from);
            if (piece == null) return false;

            // Remove da posição atual
            board[from.X, from.Y] = null;
            
            // Move para nova posição
            piece.MoveTo(to);
            board[to.X, to.Y] = piece;
            
            return true;
        }

        public List<Position> GetAdjacentPositions(Position position)
        {
            var adjacent = new List<Position>();
            
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0) continue;
                    
                    var newPos = new Position(position.X + dx, position.Y + dy);
                    if (newPos.IsValid())
                    {
                        adjacent.Add(newPos);
                    }
                }
            }
            
            return adjacent;
        }

        public List<Position> GetPossibleJumps(Position startPosition, HashSet<Position>? visited = null)
        {
            visited ??= new HashSet<Position>();
            visited.Add(startPosition);
            
            var jumps = new List<Position>();
            
            // Verifica todas as direções possíveis
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0) continue;
                    
                    var jumpOverPos = new Position(startPosition.X + dx, startPosition.Y + dy);
                    var landingPos = new Position(startPosition.X + (2 * dx), startPosition.Y + (2 * dy));
                    
                    // Verifica se pode saltar
                    if (landingPos.IsValid() && 
                        !IsPositionEmpty(jumpOverPos) && 
                        IsPositionEmpty(landingPos) &&
                        !visited.Contains(landingPos))
                    {
                        jumps.Add(landingPos);
                        
                        // Recursivamente procura mais saltos a partir desta posição
                        var furtherJumps = GetPossibleJumps(landingPos, new HashSet<Position>(visited));
                        jumps.AddRange(furtherJumps);
                    }
                }
            }
            
            return jumps.Distinct().ToList();
        }

        public void DisplayBoard()
        {
            Console.WriteLine("=== JOGO HALMA - SERVIDOR ===");
            Console.WriteLine();
            Console.WriteLine("📋 COMO LER AS COORDENADAS:");
            Console.WriteLine("   Formato: COLUNA+LINHA (ex: A0, B1, A10, P15)");
            Console.WriteLine("   Colunas: A B C D E F G H I J K L M N O P");
            Console.WriteLine("   Linhas:  0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15");
            Console.WriteLine();
            
            // Cabeçalho com colunas mais visível
            Console.WriteLine("    A B C D E F G H I J K L M N O P");
            Console.WriteLine("    │ │ │ │ │ │ │ │ │ │ │ │ │ │ │ │");
            
            for (int y = 0; y < BOARD_SIZE; y++)
            {
                Console.Write($"{y,2}──");
                for (int x = 0; x < BOARD_SIZE; x++)
                {
                    var piece = board[x, y];
                    if (piece == null)
                    {
                        Console.Write("·│");
                    }
                    else
                    {
                        Console.Write(piece.Color == PlayerColor.Black ? "○│" : "●│");
                    }
                }
                Console.WriteLine($" ←linha {y}");
            }
            
            Console.WriteLine();
            Console.WriteLine("💡 EXEMPLO: Para mover peça de coluna A linha 0 para coluna B linha 1:");
            Console.WriteLine("   Digite: A0,B1");
            Console.WriteLine("💡 EXEMPLO: Para mover peça de coluna A linha 10 para coluna B linha 11:");
            Console.WriteLine("   Digite: A10,B11");
            Console.WriteLine();
            Console.WriteLine("🎯 OBJETIVOS:");
            Console.WriteLine("   ● (BRANCO): Formar escada triangular no canto INFERIOR DIREITO");
            Console.WriteLine("   ○ (PRETO): Formar escada triangular no canto SUPERIOR ESQUERDO");
            Console.WriteLine("   Escada = 4+3+2+1 peças (10 total por jogador)");
            Console.WriteLine();
        }

        public bool VerificarVitoria(Player jogador)
        {
            // Verifica se todas as peças do jogador chegaram ao lado oposto na orientação correta
            if (jogador.Color == PlayerColor.Black)
            {
                // Preto precisa formar escada no canto superior esquerdo (espelhada)
                // Formato: 1+2+3+4 crescendo da esquerda para direita
                int pecasNaDestino = 0;
                for (int linha = 0; linha < 4; linha++)
                {
                    for (int coluna = 0; coluna <= linha; coluna++)
                    {
                        int x = coluna;  // Da esquerda para direita
                        int y = linha;   // De cima para baixo
                        if (board[x, y]?.Color == PlayerColor.Black)
                        {
                            pecasNaDestino++;
                        }
                    }
                }
                return pecasNaDestino == 10; // 10 peças na área de destino
            }
            else
            {
                // Branco precisa formar escada no canto inferior direito (espelhada)
                // Formato: 1+2+3+4 crescendo da direita para esquerda
                int pecasNaDestino = 0;
                for (int linha = 0; linha < 4; linha++)
                {
                    for (int coluna = 0; coluna <= linha; coluna++)
                    {
                        int x = 15 - coluna;  // Da direita para esquerda
                        int y = 15 - linha;   // De baixo para cima
                        if (board[x, y]?.Color == PlayerColor.White)
                        {
                            pecasNaDestino++;
                        }
                    }
                }
                return pecasNaDestino == 10; // 10 peças na área de destino
            }
        }

        public GameBoard Clone()
        {
            var cloned = new GameBoard();
            for (int x = 0; x < BOARD_SIZE; x++)
            {
                for (int y = 0; y < BOARD_SIZE; y++)
                {
                    cloned.board[x, y] = board[x, y];
                }
            }
            return cloned;
        }
    }
}
