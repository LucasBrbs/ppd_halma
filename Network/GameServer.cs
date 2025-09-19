using System.Net;
using System.Net.Sockets;
using System.Text;

namespace PPD_Sockets.Network
{
    public class GameServer
    {
        private TcpListener? servidor;
        private bool executando = false;
        
        public void IniciarServidor()
        {
            try
            {
                servidor = new TcpListener(IPAddress.Any, 8080);
                servidor.Start();
                executando = true;
                
                Console.WriteLine("Servidor iniciado na porta 8080");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao iniciar servidor: {ex.Message}");
            }
        }
        
        public void AguardarJogador()
        {
            if (servidor == null || !executando)
            {
                Console.WriteLine("Servidor não está rodando!");
                return;
            }
            
            try
            {
                Console.WriteLine("Aguardando conexão de jogador...");
                
                TcpClient cliente = servidor.AcceptTcpClient();
                Console.WriteLine("Jogador conectado!");
                
                // TODO: Implementar lógica do jogo aqui
                
                cliente.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao aguardar jogador: {ex.Message}");
            }
        }
        
        public void PararServidor()
        {
            executando = false;
            servidor?.Stop();
            Console.WriteLine("Servidor parado");
        }
    }
}
