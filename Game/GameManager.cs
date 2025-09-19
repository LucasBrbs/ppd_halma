using PPD_Sockets.Models;

namespace PPD_Sockets.Game
{
    public class GameManager
    {
        private GameBoard tabuleiro;
        private Player? jogador1;  // Servidor (branco)
        private Player? jogador2;  // Cliente (preto)
        private Player? jogadorAtual;
        private Player? jogadorVencedor; // Armazena o vencedor
        private GameState estado;
        
        public GameBoard Tabuleiro => tabuleiro;
        public Player? JogadorAtual => jogadorAtual;
        public Player? JogadorVencedor => jogadorVencedor;
        public GameState Estado => estado;
        
        public GameManager()
        {
            tabuleiro = new GameBoard();
            estado = GameState.WaitingForPlayers;
        }
        
        // Adiciona o primeiro jogador (servidor)
        public void AdicionarJogador1(string nome)
        {
            jogador1 = new Player("1", nome, PlayerColor.White);
            Console.WriteLine($"Jogador 1 adicionado: {nome} (Branco)");
        }
        
        // Adiciona o segundo jogador (cliente) e inicia o jogo
        public bool AdicionarJogador2(string nome)
        {
            if (jogador1 == null)
            {
                Console.WriteLine("Erro: Jogador 1 não foi adicionado ainda!");
                return false;
            }
            
            jogador2 = new Player("2", nome, PlayerColor.Black);
            Console.WriteLine($"Jogador 2 adicionado: {nome} (Preto)");
            
            // Inicia o jogo
            IniciarJogo();
            return true;
        }
        
        private void IniciarJogo()
        {
            if (jogador1 == null || jogador2 == null) return;
            
            // Inicializa o tabuleiro com as peças dos jogadores
            tabuleiro.InitializeBoard(jogador1, jogador2);
            
            // Jogador 1 (branco) sempre começa
            jogadorAtual = jogador1;
            estado = GameState.InProgress;
            
            Console.WriteLine("=== JOGO INICIADO ===");
            Console.WriteLine($"Turno de: {jogadorAtual.Name} ({jogadorAtual.Color})");
            
            // Mostra o tabuleiro inicial
            tabuleiro.DisplayBoard();
        }
        
        // Processa um movimento usando string de coordenadas
        public (bool sucesso, string mensagem, bool jogoTerminou, Player? vencedor) ProcessarMovimento(string movimento, Player jogador)
        {
            if (jogadorAtual != jogador)
            {
                return (false, "Não é seu turno!", false, null);
            }

            string[] partes = movimento.Split(',');
            if (partes.Length != 2)
            {
                return (false, "Formato inválido! Use: A0,B1", false, null);
            }

            var origem = ParsePosition(partes[0].Trim());
            var destino = ParsePosition(partes[1].Trim());

            if (origem == null || destino == null)
            {
                return (false, "Posições inválidas!", false, null);
            }

            // Verifica se a peça pertence ao jogador
            var peca = tabuleiro.GetPieceAt(origem);
            if (peca == null || peca.Color != jogador.Color)
            {
                return (false, "Peça não encontrada ou não pertence ao jogador", false, null);
            }

            bool movimentoValido = tabuleiro.MovePiece(origem, destino);
            if (!movimentoValido)
            {
                return (false, "Movimento inválido!", false, null);
            }

            // Verifica vitória
            if (tabuleiro.VerificarVitoria(jogador))
            {
                estado = GameState.Finished;
                return (true, $"Jogador {jogador.Name} venceu!", true, jogador);
            }

            TrocarTurno();
            return (true, "Movimento realizado com sucesso!", false, null);
        }
        
        // Parse de posição tipo "A0" para Position
        private Position? ParsePosition(string pos)
        {
            if (string.IsNullOrEmpty(pos)) return null;
            
            // Remove espaços em branco
            pos = pos.Trim();
            
            // Formato: A0, A10, A15, etc.
            if (pos.Length < 2 || pos.Length > 3) return null;
            
            char coluna = char.ToUpper(pos[0]);
            string linhaStr = pos.Substring(1);
            
            // Verifica se a coluna é válida (A-P = 0-15)
            if (coluna < 'A' || coluna > 'P') return null;
            
            int x = coluna - 'A';  // A=0, B=1, ..., P=15
            
            // Parse da linha (0-15)
            if (!int.TryParse(linhaStr, out int y)) return null;
            
            // Verifica se as coordenadas estão dentro do tabuleiro
            if (x < 0 || x >= 16 || y < 0 || y >= 16) return null;
            
            return new Position(x, y);
        }

        // Processa um movimento
        public bool ProcessarMovimento(string jogadorId, Position de, Position para)
        {
            // Verifica se é o turno do jogador correto
            if (jogadorAtual?.Id != jogadorId)
            {
                Console.WriteLine($"Erro: Não é o turno do jogador {jogadorId}");
                return false;
            }
            
            // Verifica se a peça pertence ao jogador
            var peca = tabuleiro.GetPieceAt(de);
            if (peca == null || peca.Color != jogadorAtual.Color)
            {
                Console.WriteLine("Erro: Peça não encontrada ou não pertence ao jogador");
                return false;
            }
            
            // Tenta mover a peça
            if (tabuleiro.MovePiece(de, para))
            {
                Console.WriteLine($"Movimento executado: {de} → {para}");
                
                // Verifica se o jogador ganhou usando a verificação correta do tabuleiro
                if (tabuleiro.VerificarVitoria(jogadorAtual))
                {
                    estado = GameState.Finished;
                    jogadorVencedor = jogadorAtual; // Armazena o vencedor antes de trocar turno
                    Console.WriteLine($"🎉 {jogadorAtual.Name} GANHOU! 🎉");
                    return true;
                }
                
                // Troca o turno
                TrocarTurno();
                return true;
            }
            else
            {
                Console.WriteLine("Erro: Movimento inválido");
                return false;
            }
        }
        
        private void TrocarTurno()
        {
            jogadorAtual = (jogadorAtual == jogador1) ? jogador2 : jogador1;
            Console.WriteLine($"Turno de: {jogadorAtual?.Name} ({jogadorAtual?.Color})");
        }
        
        // Converte o estado do tabuleiro para string (para enviar via socket)
        public string ObterEstadoTabuleiro()
        {
            var estado = "";
            
            for (int y = 0; y < 16; y++)
            {
                for (int x = 0; x < 16; x++)
                {
                    var peca = tabuleiro.GetPieceAt(new Position(x, y));
                    if (peca == null)
                    {
                        estado += ".";
                    }
                    else
                    {
                        estado += (peca.Color == PlayerColor.Black) ? "B" : "W";
                    }
                }
            }
            
            return estado;
        }
        
        // Obtém informações do jogo atual
        public string ObterInfoJogo()
        {
            if (jogador1 == null || jogador2 == null) return "Aguardando jogadores...";
            
            var info = $"Jogador 1: {jogador1.Name} (Preto) - {jogador1.PiecesInTargetZone()}/19 peças no alvo\n";
            info += $"Jogador 2: {jogador2.Name} (Branco) - {jogador2.PiecesInTargetZone()}/19 peças no alvo\n";
            info += $"Turno atual: {jogadorAtual?.Name} ({jogadorAtual?.Color})\n";
            info += $"Estado: {estado}";
            
            return info;
        }
    }
}
