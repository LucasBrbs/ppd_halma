namespace PPD_Sockets.Models
{
    public class GamePiece
    {
        public PlayerColor Color { get; set; }
        public Position Position { get; set; }
        public bool IsInTargetZone { get; set; }

        public GamePiece(PlayerColor color, Position position)
        {
            Color = color;
            Position = position;
            IsInTargetZone = false;
        }

        public void MoveTo(Position newPosition)
        {
            Position = newPosition;
            CheckIfInTargetZone();
        }

        private void CheckIfInTargetZone()
        {
            // Para jogador preto (começa no canto superior esquerdo)
            // Zona alvo é o canto inferior direito
            if (Color == PlayerColor.Black)
            {
                IsInTargetZone = Position.X >= 13 && Position.Y >= 13;
            }
            // Para jogador branco (começa no canto inferior direito)
            // Zona alvo é o canto superior esquerdo
            else if (Color == PlayerColor.White)
            {
                IsInTargetZone = Position.X <= 2 && Position.Y <= 2;
            }
        }

        public override string ToString()
        {
            return $"{Color} piece at {Position}";
        }
    }
}
