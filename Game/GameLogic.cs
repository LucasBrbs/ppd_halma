using PPD_Sockets.Models;

namespace PPD_Sockets.Game
{
    public class Move
    {
        public Position From { get; set; }
        public Position To { get; set; }
        public MoveType Type { get; set; }
        public PlayerColor PlayerColor { get; set; }

        public Move(Position from, Position to, PlayerColor playerColor)
        {
            From = from;
            To = to;
            PlayerColor = playerColor;
            Type = CalculateMoveType(from, to);
        }

        private MoveType CalculateMoveType(Position from, Position to)
        {
            int deltaX = Math.Abs(to.X - from.X);
            int deltaY = Math.Abs(to.Y - from.Y);
            
            // Movimento adjacente (uma casa)
            if (deltaX <= 1 && deltaY <= 1)
            {
                return MoveType.Adjacent;
            }
            
            // Qualquer movimento maior é considerado salto
            return MoveType.Jump;
        }

        public override string ToString()
        {
            return $"{PlayerColor} moves from {From} to {To} ({Type})";
        }
    }

    public class GameLogic
    {
        private GameBoard board;
        private Player? currentPlayer;
        private Player? waitingPlayer;
        private GameState gameState;
        private List<Move> moveHistory;

        public GameBoard Board => board;
        public Player? CurrentPlayer => currentPlayer;
        public Player? WaitingPlayer => waitingPlayer;
        public GameState GameState => gameState;
        public List<Move> MoveHistory => moveHistory;

        public event Action<Player>? PlayerTurnChanged;
        public event Action<Player>? GameEnded;
        public event Action<Move>? MoveExecuted;

        public GameLogic()
        {
            board = new GameBoard();
            gameState = GameState.WaitingForPlayers;
            moveHistory = new List<Move>();
        }

        public bool StartGame(Player player1, Player player2)
        {
            if (gameState != GameState.WaitingForPlayers)
                return false;

            // Player1 sempre começa (geralmente o preto)
            currentPlayer = player1.Color == PlayerColor.Black ? player1 : player2;
            waitingPlayer = player1.Color == PlayerColor.Black ? player2 : player1;
            
            board.InitializeBoard(player1, player2);
            gameState = GameState.InProgress;
            
            PlayerTurnChanged?.Invoke(currentPlayer);
            
            return true;
        }

        public bool IsValidMove(Move move)
        {
            // Verifica se é o turno do jogador correto
            if (currentPlayer?.Color != move.PlayerColor)
                return false;

            // Verifica se as posições são válidas
            if (!move.From.IsValid() || !move.To.IsValid())
                return false;

            // Verifica se há uma peça na posição de origem
            var piece = board.GetPieceAt(move.From);
            if (piece == null || piece.Color != move.PlayerColor)
                return false;

            // Verifica se a posição de destino está vazia
            if (!board.IsPositionEmpty(move.To))
                return false;

            // Valida o tipo de movimento
            return move.Type switch
            {
                MoveType.Adjacent => IsValidAdjacentMove(move.From, move.To),
                MoveType.Jump => IsValidJumpMove(move.From, move.To),
                _ => false
            };
        }

        private bool IsValidAdjacentMove(Position from, Position to)
        {
            var adjacentPositions = board.GetAdjacentPositions(from);
            return adjacentPositions.Contains(to);
        }

        private bool IsValidJumpMove(Position from, Position to)
        {
            var possibleJumps = board.GetPossibleJumps(from);
            return possibleJumps.Contains(to);
        }

        public bool ExecuteMove(Move move)
        {
            if (!IsValidMove(move))
                return false;

            // Executa o movimento no tabuleiro
            if (!board.MovePiece(move.From, move.To))
                return false;

            // Adiciona ao histórico
            moveHistory.Add(move);
            
            // Dispara evento
            MoveExecuted?.Invoke(move);

            // Verifica se o jogo terminou
            if (currentPlayer!.HasWon())
            {
                gameState = GameState.Finished;
                GameEnded?.Invoke(currentPlayer);
                return true;
            }

            // Troca o turno
            SwitchTurn();
            
            return true;
        }

        private void SwitchTurn()
        {
            (currentPlayer, waitingPlayer) = (waitingPlayer, currentPlayer);
            PlayerTurnChanged?.Invoke(currentPlayer!);
        }

        public List<Position> GetValidMovesForPiece(Position piecePosition)
        {
            var validMoves = new List<Position>();
            
            if (!piecePosition.IsValid())
                return validMoves;

            var piece = board.GetPieceAt(piecePosition);
            if (piece == null || piece.Color != currentPlayer?.Color)
                return validMoves;

            // Movimentos adjacentes
            var adjacentPositions = board.GetAdjacentPositions(piecePosition);
            validMoves.AddRange(adjacentPositions.Where(board.IsPositionEmpty));

            // Movimentos de salto
            var jumpPositions = board.GetPossibleJumps(piecePosition);
            validMoves.AddRange(jumpPositions);

            return validMoves.Distinct().ToList();
        }

        public List<Position> GetAllValidMoves(PlayerColor playerColor)
        {
            var allValidMoves = new List<Position>();
            
            var player = currentPlayer?.Color == playerColor ? currentPlayer : waitingPlayer;
            if (player == null) return allValidMoves;

            foreach (var piece in player.Pieces)
            {
                var validMoves = GetValidMovesForPiece(piece.Position);
                allValidMoves.AddRange(validMoves);
            }

            return allValidMoves.Distinct().ToList();
        }

        public void DisplayGameState()
        {
            Console.Clear();
            Console.WriteLine("=== HALMA GAME ===");
            Console.WriteLine($"Current Player: {currentPlayer?.Name} ({currentPlayer?.Color})");
            Console.WriteLine($"Game State: {gameState}");
            Console.WriteLine();
            
            board.DisplayBoard();
            
            Console.WriteLine();
            Console.WriteLine($"Black Player: {(currentPlayer?.Color == PlayerColor.Black ? currentPlayer : waitingPlayer)}");
            Console.WriteLine($"White Player: {(currentPlayer?.Color == PlayerColor.White ? currentPlayer : waitingPlayer)}");
            Console.WriteLine($"Moves played: {moveHistory.Count}");
        }

        public GameLogic Clone()
        {
            var cloned = new GameLogic
            {
                board = board.Clone(),
                currentPlayer = currentPlayer,
                waitingPlayer = waitingPlayer,
                gameState = gameState,
                moveHistory = new List<Move>(moveHistory)
            };
            
            return cloned;
        }
    }
}
