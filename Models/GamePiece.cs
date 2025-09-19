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
            // Para jogador preto (começa no canto inferior direito)
            // Zona alvo é o canto superior esquerdo - triângulo 1+2+3+4
            if (Color == PlayerColor.Black)
            {
                // Verifica se está na zona triangular superior esquerda
                bool inTargetZone = false;
                for (int linha = 0; linha < 4; linha++)
                {
                    for (int coluna = 0; coluna <= linha; coluna++)
                    {
                        int x = coluna;  // Da esquerda para direita
                        int y = linha;   // De cima para baixo
                        if (Position.X == x && Position.Y == y)
                        {
                            inTargetZone = true;
                            break;
                        }
                    }
                    if (inTargetZone) break;
                }
                IsInTargetZone = inTargetZone;
            }
            // Para jogador branco (começa no canto superior esquerdo)
            // Zona alvo é o canto inferior direito - triângulo 1+2+3+4
            else if (Color == PlayerColor.White)
            {
                // Verifica se está na zona triangular inferior direita
                bool inTargetZone = false;
                for (int linha = 0; linha < 4; linha++)
                {
                    for (int coluna = 0; coluna <= linha; coluna++)
                    {
                        int x = 15 - coluna;  // Da direita para esquerda
                        int y = 15 - linha;   // De baixo para cima
                        if (Position.X == x && Position.Y == y)
                        {
                            inTargetZone = true;
                            break;
                        }
                    }
                    if (inTargetZone) break;
                }
                IsInTargetZone = inTargetZone;
            }
        }

        public override string ToString()
        {
            return $"{Color} piece at {Position}";
        }
    }
}
