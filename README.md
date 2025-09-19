# Projeto de Sockets - Jogo Halma

## Objetivo
Implementar o jogo Halma utilizando Sockets para comunicação entre os tabuleiros que podem estar em máquinas diferentes.

## Funcionalidades Implementadas
- ✅ Estrutura básica do projeto
- ⏳ Servidor do jogo (gerencia partidas)
- ⏳ Cliente do jogo (interface para jogadores)
- ⏳ Tabuleiro e lógica do Halma
- ⏳ Comunicação via Sockets TCP
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