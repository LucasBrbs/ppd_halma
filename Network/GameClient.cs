using System.Net.Sockets;
using System.Text;
using PPD_Sockets.Models;

namespace PPD_Sockets.Network
{
    public class GameClient
    {
        private TcpClient? cliente;
        private NetworkStream? stream;
        private bool jogoAtivo = false;
        
        public async Task<bool> ConectarServidor(string ip)
        {
            try
            {
                cliente = new TcpClient();
                await cliente.ConnectAsync(ip, 8080);
                stream = cliente.GetStream();
                
                Console.WriteLine($"Conectado ao servidor {ip}:8080");
                
                // Envia nome
                Console.Write("Digite seu nome: ");
                string nome = Console.ReadLine() ?? "Cliente";
                EnviarMensagem(nome);
                
                // Inicia loop do jogo
                await LoopJogoCliente();
                
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao conectar: {ex.Message}");
                return false;
            }
        }
        
        private async Task LoopJogoCliente()
        {
            jogoAtivo = true;
            string[,] tabuleiro = new string[16, 16];
            bool meuTurno = false;
            bool aguardandoMovimento = false;
            
            // Inicializa tabuleiro vazio
            for (int i = 0; i < 16; i++)
                for (int j = 0; j < 16; j++)
                    tabuleiro[i, j] = ".";
            
            while (jogoAtivo && stream != null)
            {
                // Recebe mensagens do servidor
                string? mensagem = await ReceberMensagemAsync();
                if (string.IsNullOrEmpty(mensagem)) break;
                
                string[] partes = mensagem.Split('|', 3);
                if (partes.Length < 2) continue;
                
                string tipo = partes[0];
                string remetente = partes[1];
                string dados = partes.Length > 2 ? partes[2] : "";
                
                switch (tipo)
                {
                    case "WELCOME":
                        Console.WriteLine($"[{remetente}]: {dados}");
                        break;
                        
                    case "BOARD":
                        AtualizarTabuleiro(tabuleiro, dados);
                        MostrarTabuleiro(tabuleiro);
                        break;
                        
                    case "TURN":
                        if (dados.Contains("Seu turno"))
                        {
                            meuTurno = true;
                            aguardandoMovimento = true;
                            ProcessarTurnoJogador();
                        }
                        else
                        {
                            Console.WriteLine("Aguardando turno do oponente...");
                            meuTurno = false;
                            aguardandoMovimento = false;
                        }
                        break;
                        
                    case "ERROR":
                        if (aguardandoMovimento)
                        {
                            Console.WriteLine($"❌ ERRO: {dados}");
                            Console.WriteLine("Tente novamente!");
                            ProcessarTurnoJogador();
                        }
                        break;
                        
                    case "GAME_END":
                        Console.WriteLine($"[{remetente}]: {dados}");
                        jogoAtivo = false;
                        break;
                        
                    default:
                        Console.WriteLine($"[{remetente}]: {dados}");
                        break;
                }
                
                await Task.Delay(100);
            }
        }
        
        private Task ProcessarTurnoJogador()
        {
            return Task.Run(() =>
            {
                while (true)
                {
                    Console.WriteLine("É seu turno! Digite movimento (ex: A0,B1) ou 'quit':");
                    
                    string? movimento = Console.ReadLine();
                    if (movimento?.ToLower() == "quit")
                    {
                        jogoAtivo = false;
                        break;
                    }
                    
                    if (!string.IsNullOrEmpty(movimento))
                    {
                        EnviarMensagem($"MOVE|Cliente|{movimento}");
                        break; // Sai do loop e aguarda resposta do servidor
                    }
                    else
                    {
                        Console.WriteLine("❌ Digite um movimento válido!");
                    }
                }
            });
        }
        
        private void AtualizarTabuleiro(string[,] tabuleiro, string estado)
        {
            int index = 0;
            for (int y = 0; y < 16; y++)
            {
                for (int x = 0; x < 16; x++)
                {
                    if (index < estado.Length)
                    {
                        char c = estado[index];
                        tabuleiro[x, y] = c == 'B' ? "○" : c == 'W' ? "●" : ".";
                        index++;
                    }
                }
            }
        }
        
        private void MostrarTabuleiro(string[,] tabuleiro)
        {
            Console.Clear();
            Console.WriteLine("=== JOGO HALMA - CLIENTE ===");
            Console.WriteLine();
            Console.WriteLine("📋 COMO LER AS COORDENADAS:");
            Console.WriteLine("   Formato: COLUNA+LINHA (ex: A0, B1, A10, P15)");
            Console.WriteLine("   Colunas: A B C D E F G H I J K L M N O P");
            Console.WriteLine("   Linhas:  0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15");
            Console.WriteLine();
            
            // TABULEIRO PADRONIZADO (mesma visualização que o servidor)
            Console.WriteLine("    A B C D E F G H I J K L M N O P");
            Console.WriteLine("    │ │ │ │ │ │ │ │ │ │ │ │ │ │ │ │");
            
            for (int y = 0; y < 16; y++)
            {
                Console.Write($"{y,2}──");
                for (int x = 0; x < 16; x++)
                {
                    string peca = tabuleiro[x, y];
                    if (peca == "●")
                        Console.Write("●│");
                    else if (peca == "○") 
                        Console.Write("○│");
                    else
                        Console.Write("·│");
                }
                Console.WriteLine($" ←linha {y}");
            }
            
            Console.WriteLine();
            Console.WriteLine("💡 EXEMPLO: Para mover peça de coluna A linha 0 para coluna B linha 1:");
            Console.WriteLine("   Digite: A0,B1");
            Console.WriteLine("💡 EXEMPLO: Para mover peça de coluna A linha 10 para coluna B linha 11:");
            Console.WriteLine("   Digite: A10,B11");
            Console.WriteLine();
            Console.WriteLine("🎯 POSIÇÕES INICIAIS:");
            Console.WriteLine("   ● Jogador Branco (SERVIDOR): Canto superior esquerdo (A0-D3)");
            Console.WriteLine("   ○ Jogador Preto (CLIENTE): Canto inferior direito (M12-P15)");
            Console.WriteLine();
        }
        
        private void EnviarMensagem(string mensagem)
        {
            if (stream == null) return;
            
            try
            {
                byte[] dados = Encoding.UTF8.GetBytes(mensagem + "\n");
                stream.Write(dados, 0, dados.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao enviar: {ex.Message}");
            }
        }
        
        private async Task<string?> ReceberMensagemAsync()
        {
            if (stream == null) return null;
            
            try
            {
                byte[] buffer = new byte[1024];
                int bytes = await stream.ReadAsync(buffer, 0, buffer.Length);
                return bytes > 0 ? Encoding.UTF8.GetString(buffer, 0, bytes).Trim() : null;
            }
            catch
            {
                return null;
            }
        }
        
        public void Desconectar()
        {
            jogoAtivo = false;
            stream?.Close();
            cliente?.Close();
            Console.WriteLine("Desconectado do servidor");
        }
    }
}
