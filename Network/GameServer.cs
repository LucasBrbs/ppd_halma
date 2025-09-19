using System.Net;
using System.Net.Sockets;
using System.Text;
using PPD_Sockets.Models;
using PPD_Sockets.Game;

namespace PPD_Sockets.Network
{
    public class GameServer
    {
        private TcpListener? servidor;
        private bool executando = false;
        private GameManager? jogo;
        
        public void IniciarServidor()
        {
            try
            {
                servidor = new TcpListener(IPAddress.Any, 8080);
                servidor.Start();
                executando = true;
                jogo = new GameManager();
                Console.WriteLine("Servidor iniciado na porta 8080");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao iniciar servidor: {ex.Message}");
            }
        }
        
        public async Task AguardarJogador()
        {
            if (servidor == null || !executando || jogo == null)
            {
                Console.WriteLine("Servidor não está rodando!");
                return;
            }

            try
            {
                // Pede nome do servidor
                Console.Write("Digite seu nome (Jogador 1 - Branca): ");
                string nomeServidor = Console.ReadLine() ?? "Servidor";
                jogo.AdicionarJogador1(nomeServidor);

                Console.WriteLine("Aguardando conexão do segundo jogador...");
                TcpClient cliente = servidor.AcceptTcpClient();
                NetworkStream stream = cliente.GetStream();

                // Recebe nome do cliente
                string? nomeCliente = ReceberMensagem(stream);
                if (string.IsNullOrEmpty(nomeCliente)) nomeCliente = "Cliente";
                
                Console.WriteLine($"Jogador 2 conectado: {nomeCliente}");
                
                if (jogo.AdicionarJogador2(nomeCliente))
                {
                    // Envia boas vindas
                    EnviarMensagem(stream, $"WELCOME|{nomeCliente}|Bem-vindo ao Halma!");
                    
                    // Envia estado inicial
                    string estadoTabuleiro = jogo.ObterEstadoTabuleiro();
                    EnviarMensagem(stream, $"BOARD|Servidor|{estadoTabuleiro}");
                    
                    // Envia notificação de turno inicial (jogador 1 sempre começa)
                    if (jogo.JogadorAtual?.Id == "1")
                    {
                        EnviarMensagem(stream, "TURN|Servidor|Aguardando turno do servidor");
                    }
                    else
                    {
                        EnviarMensagem(stream, $"TURN|{jogo.JogadorAtual?.Name}|Seu turno");
                    }
                    
                    // Loop do jogo
                    await LoopJogo(stream);
                }

                cliente.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }
        }

        private async Task LoopJogo(NetworkStream stream)
        {
            if (jogo == null) return;

            while (jogo.Estado == GameState.InProgress)
            {
                // Mostra estado do jogo
                Console.Clear();
                jogo.Tabuleiro.DisplayBoard();
                Console.WriteLine(jogo.ObterInfoJogo());

                if (jogo.JogadorAtual?.Id == "1") // Turno do servidor
                {
                    Console.WriteLine("Seu turno! Digite movimento (ex: A0,B1) ou 'quit':");
                    string? input = Console.ReadLine();
                    
                    if (input?.ToLower() == "quit") break;
                    
                    if (ProcessarComandoMovimento(input))
                    {
                        // Envia estado atualizado para cliente
                        string estadoAtualizado = jogo.ObterEstadoTabuleiro();
                        EnviarMensagem(stream, $"BOARD|Servidor|{estadoAtualizado}");
                        
                        if (jogo.Estado == GameState.Finished)
                        {
                            // Obtém o vencedor correto
                            string vencedor = jogo.JogadorVencedor?.Name ?? "Jogador desconhecido";
                            EnviarMensagem(stream, $"GAME_END|Servidor|🎉 {vencedor} VENCEU! 🎉");
                            break;
                        }
                        
                        EnviarMensagem(stream, $"TURN|{jogo.JogadorAtual?.Name}|Seu turno");
                    }
                }
                else // Turno do cliente
                {
                    Console.WriteLine("Aguardando movimento do oponente...");
                    
                    // Envia notificação de turno para o cliente
                    EnviarMensagem(stream, $"TURN|{jogo.JogadorAtual?.Name}|Seu turno");
                    
                    string? mensagem = await ReceberMensagemAsync(stream);
                    if (string.IsNullOrEmpty(mensagem)) break;
                    
                    string[] partes = mensagem.Split('|');
                    if (partes.Length >= 3 && partes[0] == "MOVE")
                    {
                        if (ProcessarComandoMovimento(partes[2]))
                        {
                            // Envia estado atualizado
                            string estadoAtualizado = jogo.ObterEstadoTabuleiro();
                            EnviarMensagem(stream, $"BOARD|Servidor|{estadoAtualizado}");
                            
                            if (jogo.Estado == GameState.Finished)
                            {
                                // Obtém o vencedor correto
                                string vencedor = jogo.JogadorVencedor?.Name ?? "Jogador desconhecido";
                                EnviarMensagem(stream, $"GAME_END|Servidor|🎉 {vencedor} VENCEU! 🎉");
                                break;
                            }
                        }
                        else
                        {
                            // Envia erro para o cliente
                            EnviarMensagem(stream, "ERROR|Servidor|Movimento inválido!");
                        }
                    }
                }

                await Task.Delay(100); // Pequena pausa
            }
        }

        private bool ProcessarComandoMovimento(string? comando)
        {
            if (jogo == null || string.IsNullOrEmpty(comando)) return false;

            try
            {
                string[] posicoes = comando.Split(',');
                if (posicoes.Length != 2) return false;

                Position de = ParsePosition(posicoes[0].Trim());
                Position para = ParsePosition(posicoes[1].Trim());

                return jogo.ProcessarMovimento(jogo.JogadorAtual?.Id ?? "", de, para);
            }
            catch
            {
                Console.WriteLine("Formato inválido! Use: A0,B1");
                return false;
            }
        }

        private Position ParsePosition(string pos)
        {
            if (string.IsNullOrEmpty(pos)) throw new ArgumentException("Posição inválida");
            
            // Remove espaços em branco
            pos = pos.Trim();
            
            // Formato: A0, A10, A15, etc.
            if (pos.Length < 2 || pos.Length > 3) throw new ArgumentException("Posição inválida");
            
            char coluna = char.ToUpper(pos[0]);
            string linhaStr = pos.Substring(1);
            
            // Verifica se a coluna é válida (A-P = 0-15)
            if (coluna < 'A' || coluna > 'P') throw new ArgumentException("Coluna inválida");
            
            int x = coluna - 'A';  // A=0, B=1, ..., P=15
            
            // Parse da linha (0-15)
            if (!int.TryParse(linhaStr, out int y)) throw new ArgumentException("Linha inválida");
            
            // Verifica se as coordenadas estão dentro do tabuleiro
            if (x < 0 || x >= 16 || y < 0 || y >= 16) throw new ArgumentException("Coordenadas fora do tabuleiro");
            
            return new Position(x, y);
        }

        private void EnviarMensagem(NetworkStream stream, string mensagem)
        {
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

        private string? ReceberMensagem(NetworkStream stream)
        {
            try
            {
                byte[] buffer = new byte[1024];
                int bytes = stream.Read(buffer, 0, buffer.Length);
                return bytes > 0 ? Encoding.UTF8.GetString(buffer, 0, bytes).Trim() : null;
            }
            catch
            {
                return null;
            }
        }

        private async Task<string?> ReceberMensagemAsync(NetworkStream stream)
        {
            return await Task.Run(() => ReceberMensagem(stream));
        }
        
        public void PararServidor()
        {
            executando = false;
            servidor?.Stop();
        }
    }
}
