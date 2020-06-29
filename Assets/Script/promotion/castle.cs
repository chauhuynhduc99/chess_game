using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class castle : pro_P
{
    private void OnMouseDown()
    {
        Destroy(ChessBoard.Current.cells[(int)pro_P.ProPawn.transform.position.x][(int)pro_P.ProPawn.transform.position.y].CurrentPiece.gameObject);
        if (ProPawn.Player == Eplayer.WHITE)
        {
            GameObject chess_piece = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Pieces/White_R"));
            BasePiece p = chess_piece.GetComponent<BasePiece>();
            p.SetOriginalLocation((int)ProPawn.Location.x, (int)ProPawn.Location.y);
            chess_piece.transform.parent = ChessBoard.Current.transform;
        }
        else
        {
            GameObject chess_piece = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Pieces/Black_R"));
            BasePiece p = chess_piece.GetComponent<BasePiece>();
            p.SetOriginalLocation((int)ProPawn.Location.x, (int)ProPawn.Location.y);
            chess_piece.transform.parent = ChessBoard.Current.transform;
        }
        pro_P.Current.done = true;
    }
}
