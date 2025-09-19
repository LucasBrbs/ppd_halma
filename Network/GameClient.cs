using System.Net.Sockets;
using System.Text;

namespace PPD_Sockets.Network
{
    public class GameClient
    {
        private TcpClient? cliente;
        private NetworkStream? stream;
        
        public bool ConectarServidor(string ip)
        {
            try
            {
                cliente = new TcpClient();
                cliente.Connect(ip, 8080);
                stream = cliente.GetStream();
                
                Console.WriteLine($"Conectado ao servidor {ip}:8080");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao conectar: {ex.Message}");
                return false;
            }
        }
        
        public void EnviarMensagem(string mensagem)
        {
            if (stream == null)
            {
                Console.WriteLine("Não conectado ao servidor!");
                return;
            }
            
            try
            {
                byte[] dados = Encoding.UTF8.GetBytes(mensagem);
                stream.Write(dados, 0, dados.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao enviar mensagem: {ex.Message}");
            }
        }
        
        public string? ReceberMensagem()
        {
            if (stream == null)
            {
                Console.WriteLine("Não conectado ao servidor!");
                return null;
            }
            
            try
            {
                byte[] buffer = new byte[1024];
                int bytesLidos = stream.Read(buffer, 0, buffer.Length);
                
                if (bytesLidos > 0)
                {
                    return Encoding.UTF8.GetString(buffer, 0, bytesLidos);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao receber mensagem: {ex.Message}");
            }
            
            return null;
        }
        
        public void Desconectar()
        {
            stream?.Close();
            cliente?.Close();
            Console.WriteLine("Desconectado do servidor");
        }
    }
}
