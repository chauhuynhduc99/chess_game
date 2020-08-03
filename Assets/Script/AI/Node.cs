using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Node
{
    private int value;
    public int Value
    {
        get
        {
            return this.value;
        }
    }

    private int depth;
    public int Depth
    {
        get
        {
            return this.depth;
        }
    }

    private BasePiece bestMove;
    public BasePiece BestMove
    {
        get
        {
            return this.bestMove;
        }
    }

    private ChessBoard board;
    public ChessBoard Board
    {
        get
        {
            return this.board;
        }
    }

    public Node(BasePiece move, int value, int depth, ChessBoard board)
    {
        this.bestMove = move;
        this.value = value;
        this.depth = depth;
        this.board = board;
    }
}