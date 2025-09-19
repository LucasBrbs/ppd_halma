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
            Console.WriteLine("   0123456789ABCDEF");
            for (int y = 0; y < BOARD_SIZE; y++)
            {
                Console.Write($"{y:X2} ");
                for (int x = 0; x < BOARD_SIZE; x++)
                {
                    var piece = board[x, y];
                    if (piece == null)
                    {
                        Console.Write(".");
                    }
                    else
                    {
                        Console.Write(piece.Color == PlayerColor.Black ? "●" : "○");
                    }
                }
                Console.WriteLine();
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
