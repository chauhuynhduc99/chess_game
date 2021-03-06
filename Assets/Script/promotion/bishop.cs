﻿using UnityEngine;

public class bishop : pro_P
{
    private void OnMouseDown()
    {
        if (ProPawn.Player == Eplayer.WHITE)
        {
            GameObject chess_piece = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Pieces/White_B"));
            BasePiece p = chess_piece.GetComponent<BasePiece>();
            p.SetOriginalLocation((int)ProPawn.Location.x, (int)ProPawn.Location.y);
            chess_piece.transform.parent = ChessBoard.Current.Chess_Pieces.transform;
            p.CurrentCell.SetPieces(p);
            ChessBoard.Current.White_Pieces.Add(p);
            ChessBoard.Current.All_piece.Add(p);
        }
        else
        {
            GameObject chess_piece = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Pieces/Black_B"));
            BasePiece p = chess_piece.GetComponent<BasePiece>();
            p.SetOriginalLocation((int)ProPawn.Location.x, (int)ProPawn.Location.y);
            chess_piece.transform.parent = ChessBoard.Current.Chess_Pieces.transform;
            p.CurrentCell.SetPieces(p);
            ChessBoard.Current.Black_Pieces.Add(p);
            ChessBoard.Current.All_piece.Add(p);
        }
        Destroy(ProPawn.gameObject);
        pro_P.Current.done = true;
    }
}