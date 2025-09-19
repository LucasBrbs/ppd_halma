using PPD_Sockets.Network;

Console.WriteLine("=== JOGO HALMA ===");

// Cria o servidor local
var servidor = new PPD_Sockets.Network.GameServer();
servidor.IniciarServidor();

Console.WriteLine();
Console.WriteLine("Escolha uma opção:");
Console.WriteLine("1 - Aguardar jogador (ficar como servidor)");
Console.WriteLine("2 - Conectar em outro servidor");
Console.Write("Opção: ");

string? opcao = Console.ReadLine();

if (opcao == "1")
{
    Console.WriteLine("Aguardando conexão de outro jogador...");
    await servidor.AguardarJogador();
}
else if (opcao == "2")
{
    Console.Write("Digite o IP do servidor: ");
    string? ip = Console.ReadLine();
    
    if (string.IsNullOrEmpty(ip))
    {
        Console.WriteLine("IP inválido!");
        return;
    }
    
    GameClient cliente = new GameClient();
    if (await cliente.ConectarServidor(ip))
    {
        Console.WriteLine("Conectado com sucesso!");
        Console.WriteLine("Pressione qualquer tecla para desconectar...");
        Console.ReadKey();
        cliente.Desconectar();
    }
}
else
{
    Console.WriteLine("Opção inválida!");
}

servidor.PararServidor();