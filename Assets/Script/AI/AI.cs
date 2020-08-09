using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI
{
    //private Heuristic heuristic = new Heuristic();
    public static Eplayer player;
    System.Random random = new System.Random();
    public void find_move(ChessBoard board)
    {
        //BasePiece best_piece = null;
        //cell best_move = null;
        //int score = 0;
        int r = random.Next(board.Black_Pieces.Count);
        Debug.Log(r);
        List<cell> move = board.Black_Pieces[r].getLegalMoves();
        if (move.Count > 0)
        {
            int x = random.Next(move.Count);
            Debug.Log(x);
            board.Black_Pieces[r].AI_move(move[x]);
        }
        else
        {
            find_move(board);
        }
    }
}
