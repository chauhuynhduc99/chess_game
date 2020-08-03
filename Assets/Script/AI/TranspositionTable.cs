using System;
using UnityEngine;

class TranspositionTable
{
    private int[,] hashTableValue = new int[64, 12];
    private Node[] hashTable;
    private bool duplicateValue(int i, int j)
    {
        for (int m = 0; m < i; m++)
            for (int n = 0; n < j; n++)
                if (hashTableValue[m, n] == hashTableValue[i, j])
                {
                    return true;
                }
        return false;
    }

    public TranspositionTable()
    {
        System.Random rand = new System.Random();
        for (int i = 0; i < 64; i++)
        {
            for (int j = 0; j < 12; j++)
            {
                hashTableValue[i, j] = rand.Next() % 500000;
                while (duplicateValue(i, j))
                {
                    hashTableValue[i, j] = rand.Next() % 500000;
                }
            }
        }

        this.hashTable = new Node[10000003];
    }

    public int columnIndex(BasePiece piece)
    {
        Eplayer player = piece.Player;
        Etype type = piece.Type;
        int index = 0;
        if (player == Eplayer.BLACK)
            index += 6;
        if (type == Etype.KING)
            return 0 + index;
        else if (type == Etype.QUEEN)
            return 1 + index;
        else if (type == Etype.CASTLE)
            return 2 + index;
        else if (type == Etype.BISHOP)
            return 3 + index;
        else if (type == Etype.KNIGHT)
            return 4 + index;
        return 5 + index;
    }

    public int hash(ChessBoard board)
    {
        int value = 0;
        foreach (BasePiece piece in board.AllActivePieces)
        {
            int i = piece.Piece_Pos;
            int j = columnIndex(piece);
            value ^= hashTableValue[i, j];
        }
        return value % 10000003;
    }

    public void add(int key, Node node)
    {
        this.hashTable[key] = node;
    }

    public Node getNode(int key, int depth, ChessBoard board)
    {
        Node node = this.hashTable[key];
        if (node != null && node.Depth >= depth && board.isTheSame(node.Board))
            return node;
        else
            return null;
    }


}