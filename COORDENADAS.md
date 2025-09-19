# 📍 Guia de Coordenadas do Halma

## 🗺️ **Como Funciona o Sistema de Coordenadas**

### **Formato: COLUNA + LINHA**
- **Coluna**: A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P (16 colunas)
- **Linha**: 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, A, B, C, D, E, F (16 linhas)

### **Exemplos de Coordenadas:**
```
A0 = Coluna A, Linha 0 (canto superior esquerdo)
A1 = Coluna A, Linha 1 (uma casa abaixo de A0)
B0 = Coluna B, Linha 0 (uma casa à direita de A0)
P0 = Coluna P, Linha 0 (canto superior direito)
PF = Coluna P, Linha F (canto inferior direito)
AF = Coluna A, Linha F (canto inferior esquerdo)
```

## 📋 **Visualização do Tabuleiro**

```
     A B C D E F G H I J K L M N O P
     │ │ │ │ │ │ │ │ │ │ │ │ │ │ │ │
 0───○│○│○│·│·│·│·│·│·│·│·│·│·│●│●│●│ ←linha 0
 1───○│○│○│·│·│·│·│·│·│·│·│·│·│●│●│●│ ←linha 1
 2───○│○│○│·│·│·│·│·│·│·│·│·│·│●│●│●│ ←linha 2
 3───·│·│·│·│·│·│·│·│·│·│·│·│·│·│·│·│ ←linha 3
 4───·│·│·│·│·│·│·│·│·│·│·│·│·│·│·│·│ ←linha 4
 5───·│·│·│·│·│·│·│·│·│·│·│·│·│·│·│·│ ←linha 5
 6───·│·│·│·│·│·│·│·│·│·│·│·│·│·│·│·│ ←linha 6
 7───·│·│·│·│·│·│·│·│·│·│·│·│·│·│·│·│ ←linha 7
 8───·│·│·│·│·│·│·│·│·│·│·│·│·│·│·│·│ ←linha 8
 9───·│·│·│·│·│·│·│·│·│·│·│·│·│·│·│·│ ←linha 9
 A───·│·│·│·│·│·│·│·│·│·│·│·│·│·│·│·│ ←linha A
 B───·│·│·│·│·│·│·│·│·│·│·│·│·│·│·│·│ ←linha B
 C───·│·│·│·│·│·│·│·│·│·│·│·│·│·│·│·│ ←linha C
 D───●│●│●│·│·│·│·│·│·│·│·│·│·│○│○│○│ ←linha D
 E───●│●│●│·│·│·│·│·│·│·│·│·│·│○│○│○│ ←linha E
 F───●│●│●│·│·│·│·│·│·│·│·│·│·│○│○│○│ ←linha F
```

## 🎯 **Exemplos Práticos de Movimentos**

### **Movimentos Básicos:**
```
N0,M1  → Move peça preta de N0 para M1
A0,B1  → Move peça branca de A0 para B1  
B1,C2  → Move peça de B1 para C2
F5,G6  → Move peça de F5 para G6
```

### **Pulos:**
```
N0,L1  → Pula sobre peça em M0, pousa em L1
A0,C2  → Pula sobre peça em B1, pousa em C2
```

## 🧭 **Dicas de Navegação**

### **Para encontrar uma coordenada rapidamente:**
1. **Primeira letra** = Coluna (horizontal)
2. **Segunda letra/número** = Linha (vertical)

### **Exemplos visuais:**
- **A0** → Primeira coluna (A), primeira linha (0) = canto superior esquerdo
- **P0** → Última coluna (P), primeira linha (0) = canto superior direito  
- **AF** → Primeira coluna (A), última linha (F) = canto inferior esquerdo
- **PF** → Última coluna (P), última linha (F) = canto inferior direito

### **Sequências de coordenadas:**
```
Colunas: A → B → C → D → E → F → G → H → I → J → K → L → M → N → O → P
Linhas:  0 → 1 → 2 → 3 → 4 → 5 → 6 → 7 → 8 → 9 → A → B → C → D → E → F
```

## 💡 **Agora é muito mais fácil!**

Com a nova visualização, você pode ver exatamente onde cada coordenada está:
- As **colunas** são mostradas no topo: A B C D E F...
- As **linhas** são mostradas à esquerda: 0, 1, 2, 3...
- Cada **peça** tem sua posição clara no grid

**Exemplo:** Para mover a peça preta de N0 para M1:
1. Encontre a coluna N (segunda da direita)
2. Encontre a linha 0 (primeira linha)
3. Encontre a coluna M (terceira da direita) 
4. Encontre a linha 1 (segunda linha)
5. Digite: `N0,M1`
