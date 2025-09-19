# Jogo Halma com Comunicação via Sockets

## Descrição
Este é um jogo Halma (similar ao Damas Chinesas) implementado em C# .NET 9.0 com comunicação em tempo real via TCP Sockets. O jogo permite que dois jogadores joguem em máquinas diferentes através da rede.

## Características
- **Tabuleiro 16x16** com coordenadas hexadecimais (0-F)
- **Multiplayer via TCP Sockets** - cada jogador pode estar em uma máquina diferente
- **Peças Coloridas**: ● (Preto) e ○ (Branco)
- **Detecção automática de vitória**
- **Interface de console interativa**

## Como Executar

### Compilar o projeto:
```bash
dotnet build
```

### Executar:
```bash
dotnet run
```

## Como Jogar

### 1. Iniciar Servidor
- Escolha opção "1" no menu
- O servidor aguardará conexões na porta 8080

### 2. Conectar Cliente
- Em outra máquina (ou terminal), escolha opção "2"
- Digite o IP da máquina do servidor
- Digite seu nome quando solicitado

### 3. Movimentos
- Use formato: `ORIGEM,DESTINO`
- Exemplos: `A0,B1`, `C2,D3`, `F0,E1`
- Coordenadas vão de `0-F` (0-15 em hexadecimal)

### 4. Regras
- **Jogador Preto** (●): Começa primeiro, precisa mover suas peças do canto inferior direito para o superior esquerdo
- **Jogador Branco** (○): Precisa mover suas peças do canto superior esquerdo para o inferior direito
- **Movimentos válidos**: 
  - Mover para casa adjacente vazia
  - Pular sobre peças (suas ou do oponente)
- **Vitória**: Primeiro jogador a colocar todas as 9 peças na área de destino

## Estrutura do Projeto

```
ppd_sockets/
├── Program.cs              # Ponto de entrada com menu
├── Models/                 # Classes de dados
│   ├── Player.cs          # Representação do jogador
│   ├── GamePiece.cs       # Peça do jogo
│   ├── Position.cs        # Posição no tabuleiro
│   ├── Enums.cs           # Enumerações (cores, estados)
│   └── GameMessage.cs     # Mensagens de rede
├── Game/                   # Lógica do jogo
│   ├── GameManager.cs     # Gerenciamento do jogo
│   └── GameBoard.cs       # Tabuleiro e movimentos
└── Network/               # Comunicação de rede
    ├── GameServer.cs      # Servidor TCP
    └── GameClient.cs      # Cliente TCP
```

## Protocolo de Comunicação

### Formato das Mensagens
```
TIPO|REMETENTE|DADOS
```

### Tipos de Mensagem:
- **WELCOME**: Mensagem de boas-vindas
- **BOARD**: Estado atual do tabuleiro
- **TURN**: Notificação de turno
- **MOVE**: Comando de movimento
- **GAME_END**: Fim de jogo
- **ERROR**: Mensagem de erro

## Exemplos de Uso

### Servidor (Máquina 1):
```bash
$ dotnet run
=== JOGO HALMA ===
1. Iniciar servidor
2. Conectar como cliente
Escolha uma opção: 1
Aguardando conexão de outro jogador...
Servidor iniciado na porta 8080
```

### Cliente (Máquina 2):
```bash
$ dotnet run
=== JOGO HALMA ===
1. Iniciar servidor  
2. Conectar como cliente
Escolha uma opção: 2
Digite o IP do servidor: 192.168.1.100
Digite seu nome: João
Conectado ao servidor 192.168.1.100:8080
```

## Desenvolvimento

### Tecnologias Utilizadas:
- **C# .NET 9.0**
- **System.Net.Sockets** (TCP)
- **Async/Await** para programação assíncrona
- **Console Application** para interface

### Arquitetura:
- **Padrão MVC** separando Models, Game Logic e Network
- **Comunicação assíncrona** entre cliente e servidor
- **State management** para controle do jogo
- **Protocol design** para mensagens estruturadas

## Troubleshooting

### Problema: "Só funciona com dotnet run"
**Motivo**: Aplicações .NET console precisam ser executadas através do runtime .NET

**Solução**: 
- Use sempre `dotnet run` para executar
- Ou compile e execute: `dotnet build && dotnet bin/Debug/net9.0/ppd_sockets.dll`

### Problema: Conexão recusada
**Soluções**:
- Verifique se o servidor está rodando
- Confirme o IP correto da máquina servidor
- Verifique firewall (porta 8080)

### Problema: Movimento inválido
**Verificações**:
- Use formato correto: `A0,B1`
- Certifique-se que é seu turno
- Verifique se a peça pertence a você
- Movimento deve ser para casa vazia ou pular sobre peça

## Próximas Melhorias
- [ ] Interface gráfica (WPF/Avalonia)
- [ ] Suporte a mais de 2 jogadores
- [ ] Sistema de ranking/pontuação
- [ ] Replay de partidas
- [ ] Chat entre jogadores

## Objetivo
Implementar o jogo Halma utilizando Sockets para comunicação entre os tabuleiros que podem estar em máquinas diferentes.

## Funcionalidades Implementadas
- ✅ Estrutura básica do projeto
- ✅ Classes básicas de rede (GameServer, GameClient)
- ✅ Modelos básicos (Player, GamePiece, Position, Enums)
- ⏳ Lógica do tabuleiro Halma
- ⏳ Comunicação via Sockets TCP (funcional básico)
- ⏳ Sistema de turnos
- ⏳ Validação de movimentos
- ⏳ Chat durante a partida
- ⏳ Detecção de vencedor

## Arquitetura do Sistema
```
┌─────────────────┐    Socket TCP    ┌─────────────────┐
│   Cliente 1     │◄────────────────►│                 │
│   (Jogador 1)   │                  │                 │
└─────────────────┘                  │    Servidor     │
                                     │   (Controla     │
┌─────────────────┐    Socket TCP    │    Partida)     │
│   Cliente 2     │◄────────────────►│                 │
│   (Jogador 2)   │                  │                 │
└─────────────────┘                  └─────────────────┘
```

## Passo a Passo de Desenvolvimento

### Fase 1: Estrutura Base ✅
- [x] Configuração inicial do projeto
- [x] README com documentação

### Fase 2: Modelos e Lógica do Jogo 🔄
- [ ] Classe `GameBoard` (tabuleiro 16x16)
- [ ] Classe `Player` (jogador)
- [ ] Classe `GamePiece` (peça do jogo)
- [ ] Classe `GameLogic` (regras do Halma)
- [ ] Enum `MoveType` (tipos de movimento)

### Fase 3: Comunicação por Sockets 🔄
- [ ] Classe `GameServer` (servidor TCP)
- [ ] Classe `GameClient` (cliente TCP)
- [ ] Protocolo de mensagens (JSON)
- [ ] Tratamento de conexões

### Fase 4: Interface do Usuario 🔄
- [ ] Menu principal
- [ ] Visualização do tabuleiro
- [ ] Input de movimentos
- [ ] Chat entre jogadores

### Fase 5: Integração e Testes 🔄
- [ ] Testes de conexão
- [ ] Testes de gameplay
- [ ] Tratamento de erros
- [ ] Documentação final

## Como Rodar a Aplicação

### Modo de Jogo
O jogo funciona com **2 jogadores**:
- **Jogador 1**: Inicia a instância do servidor (possui o servidor)
- **Jogador 2**: Conecta no servidor do Jogador 1

### Passo a Passo para Jogar

#### 1️⃣ Jogador 1 (Servidor)
```bash
# Compilar o projeto
dotnet build

# Executar o programa
dotnet run

# No menu, escolher:
# Opção: 1 - Aguardar jogador (ficar como servidor)
```
O Jogador 1 ficará aguardando conexões na porta 8080.

#### 2️⃣ Jogador 2 (Cliente)
```bash
# Em outro terminal ou computador, executar:
dotnet run

# No menu, escolher:
# Opção: 2 - Conectar em outro servidor
# Digite o IP do servidor: [IP_DO_JOGADOR_1]

# Exemplos:
# - Mesmo computador: localhost ou 127.0.0.1
# - Rede local: 192.168.1.100 (IP do Jogador 1)
# - Internet: IP público do Jogador 1
```

### Exemplo Prático - Teste Local
**Terminal 1** (Jogador 1):
```bash
dotnet run
# Escolher opção 1
# Aguarda conexão...
```

**Terminal 2** (Jogador 2):
```bash
dotnet run
# Escolher opção 2
# Digite: localhost
# Conecta no Jogador 1
```

### Build de Produção
```bash
dotnet publish -c Release
```

## Regras do Halma
- Tabuleiro 16x16
- Cada jogador tem 19 peças
- Objetivo: mover todas as peças para o canto oposto
- Movimentos: adjacente ou "salto" sobre peças
- Ganha quem completar primeiro o objetivo 