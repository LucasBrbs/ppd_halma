# 🎮 Halma com Escada Triangular e Visão Invertida

## ✨ **Novidades Implementadas:**

### 🔺 **1. Formato de Escada Triangular (orientação correta):**

#### **Jogador PRETO (●) - Canto Inferior Direito:**
```
    A B C D E F G H I J K L M N O P
 C──·│·│·│·│·│·│·│·│·│·│·│·│·│·│·│●│ ←1 peça
 D──·│·│·│·│·│·│·│·│·│·│·│·│·│·│●│●│ ←2 peças
 E──·│·│·│·│·│·│·│·│·│·│·│·│·│●│●│●│ ←3 peças
 F──·│·│·│·│·│·│·│·│·│·│·│·│●│●│●│●│ ←4 peças
```
**Total: 1+2+3+4 = 10 peças** (escada aponta para o objetivo ↖)

#### **Jogador BRANCO (○) - Canto Superior Esquerdo:**
```
    A B C D E F G H I J K L M N O P
 0──○│·│·│·│·│·│·│·│·│·│·│·│·│·│·│·│ ←1 peça
 1──○│○│·│·│·│·│·│·│·│·│·│·│·│·│·│·│ ←2 peças
 2──○│○│○│·│·│·│·│·│·│·│·│·│·│·│·│·│ ←3 peças
 3──○│○│○│○│·│·│·│·│·│·│·│·│·│·│·│·│ ←4 peças
```
**Total: 1+2+3+4 = 10 peças** (escada aponta para o objetivo ↘)

### 🔄 **2. Visão Invertida para Jogador 2:**

#### **Servidor (Jogador 1) vê o tabuleiro normal:**
```
    A B C D E F G H I J K L M N O P
 0──○│·│·│·│·│·│·│·│·│·│·│·│·│·│·│·│
 1──○│○│·│·│·│·│·│·│·│·│·│·│·│·│·│·│
 2──○│○│○│·│·│·│·│·│·│·│·│·│·│·│·│·│
 3──○│○│○│○│·│·│·│·│·│·│·│·│·│·│·│·│
 ...
 C──·│·│·│·│·│·│·│·│·│·│·│·│·│·│·│●│
 D──·│·│·│·│·│·│·│·│·│·│·│·│·│·│●│●│
 E──·│·│·│·│·│·│·│·│·│·│·│·│·│●│●│●│
 F──·│·│·│·│·│·│·│·│·│·│·│·│●│●│●│●│
```

#### **Cliente (Jogador 2) vê o tabuleiro invertido 180°:**
```
    P O N M L K J I H G F E D C B A
 F──●│●│●│●│·│·│·│·│·│·│·│·│·│·│·│·│
 E──·│●│●│●│·│·│·│·│·│·│·│·│·│·│·│·│
 D──·│·│●│●│·│·│·│·│·│·│·│·│·│·│·│·│
 C──·│·│·│●│·│·│·│·│·│·│·│·│·│·│·│·│
 ...
 3──·│·│·│·│·│·│·│·│·│·│·│·│○│○│○│○│
 2──·│·│·│·│·│·│·│·│·│·│·│·│·│○│○│○│
 1──·│·│·│·│·│·│·│·│·│·│·│·│·│·│○│○│
 0──·│·│·│·│·│·│·│·│·│·│·│·│·│·│·│○│
```

### 🎯 **3. Objetivos dos Jogadores:**

#### **Jogador PRETO (●):**
- **Posição inicial**: Escada no canto inferior direito
- **Objetivo**: Formar a mesma escada no canto superior esquerdo
- **Movimentos típicos**: Para cima e para a esquerda

#### **Jogador BRANCO (○):**
- **Posição inicial**: Escada no canto superior esquerdo
- **Objetivo**: Formar a mesma escada no canto inferior direito
- **Movimentos típicos**: Para baixo e para a direita

### 🎲 **4. Exemplos de Movimentos Válidos:**

#### **Jogador PRETO (primeiro turno):**
```
✅ M12,L11  (diagonal superior-esquerda)
✅ N13,M12  (diagonal superior-esquerda)
✅ O14,N13  (diagonal superior-esquerda)
✅ P15,O14  (diagonal superior-esquerda)
```

#### **Jogador BRANCO (segundo turno):**
```
✅ A0,B1   (diagonal inferior-direita)
✅ B1,C2   (diagonal inferior-direita)
✅ C2,D3   (diagonal inferior-direita)
✅ D3,E4   (diagonal inferior-direita)
```

### 🔧 **5. Vantagens da Nova Implementação:**

1. **Formato Real do Halma**: Escada triangular como no jogo tradicional
2. **Experiência Realística**: Jogador 2 vê como se estivesse do outro lado da mesa
3. **Coordenadas Consistentes**: Mesmo com visão invertida, use coordenadas normais (A0, B1, etc.)
4. **Equilíbrio**: Ambos jogadores têm exatamente 10 peças
5. **Objetivo Claro**: Formar a escada espelhada no canto oposto

### 🚀 **Como Testar:**

```bash
dotnet run
```

1. **Terminal 1**: Escolha opção 1 (servidor) - visão normal
2. **Terminal 2**: Escolha opção 2 (cliente) - visão invertida 
3. **Teste movimentos**: Use as coordenadas normais em ambos

**Resultado**: Experiência de jogo muito mais realística e divertida! 🎉
