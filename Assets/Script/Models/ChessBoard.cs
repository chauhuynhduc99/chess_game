using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessBoard : MonoBehaviour
{
    #region Field
    private cell[][] Cells;
    public static ChessBoard Current;
    public GameObject cellPrefap;
    public Vector3 base_Position = Vector3.zero;
    public LayerMask CellLayerMask = 0;
    private List<BasePiece> Pieces = new List<BasePiece>();
    private List<BasePiece> black_Pieces = new List<BasePiece>();
    private List<BasePiece> white_Pieces = new List<BasePiece>();
    private King black_King;
    private King white_King;
    #endregion
    public List<BasePiece> All_Active_Pieces { get { return Pieces; } set { Pieces = value; } }
    public List<BasePiece> Black_Pieces
    {
        get
        {
            black_Pieces.Clear();
            foreach (BasePiece item in Pieces)
            {
                if (item.Player == Eplayer.BLACK)
                    black_Pieces.Add(item);
            }
            return black_Pieces;
        }
    }
    public List<BasePiece> White_Pieces
    {
        get
        {
            white_Pieces.Clear();
            foreach (BasePiece item in Pieces)
            {
                if (item.Player == Eplayer.WHITE)
                    white_Pieces.Add(item);
            }
            return white_Pieces;
        }
    }
    public King Black_King
    {
        get
        {
            foreach(BasePiece item in Pieces)
            {
                if (item.Type == Etype.KING && item.Player == Eplayer.BLACK)
                {
                    black_King = (King)item;
                    break;
                }
            }
            return black_King;
        }
    }
    public King White_King
    {
        get
        {
            foreach (BasePiece item in White_Pieces)
            {
                if (item.Type == Etype.KING && item.Player == Eplayer.WHITE)
                {
                    white_King = (King)item;
                    break;
                }
            }
            return white_King;
        }
    }
    public cell[][] cells { get { return Cells; } set { Cells = value; } }
    public Vector3 Calculate_Position(int i, int j)
    {
        return base_Position + new Vector3(i, j, 0);
    }

    [ContextMenu("Let_Start")]
    public void Let_Start()
    {
        Init_ChessBoard();
        Init_ChessPieces();
    }
    private void Init_ChessBoard()
    {
        Cells = new cell[8][];
        for(int i = 0; i < 8; i++)
        {
            Cells[i] = new cell[8];
            for (int j = 0; j < 8; j++)
            {
                GameObject c = GameObject.Instantiate(cellPrefap, Calculate_Position(i, j), Quaternion.identity) as GameObject;

                Cells[i][j] = c.GetComponent<cell>();
                c.transform.parent = this.transform;

                if ((i + j) % 2 == 0)
                    Cells[i][j].color = Ecell_color.WHITE;
                else
                    Cells[i][j].color = Ecell_color.BLACK;
            }
        }
    }
    private void Init_ChessPieces()
    {
        List<PieceInfo> List = new List<PieceInfo>();
        //King
        List.Add(new PieceInfo() { Name = "W_King", Path = "Pieces/White_K", X = 4, Y = 0 });
        List.Add(new PieceInfo() { Name = "B_King", Path = "Pieces/Black_K", X = 4, Y = 7 });

        //white
        List.Add(new PieceInfo() { Name = "W_Rook1", Path = "Pieces/White_R", X = 0, Y = 0});
        List.Add(new PieceInfo() { Name = "W_Rook2", Path = "Pieces/White_R", X = 7, Y = 0});
        List.Add(new PieceInfo() { Name = "W_Knight1", Path = "Pieces/White_N", X = 1, Y = 0});
        List.Add(new PieceInfo() { Name = "W_Knight2", Path = "Pieces/White_N", X = 6, Y = 0});
        List.Add(new PieceInfo() { Name = "W_Bishop1", Path = "Pieces/White_B", X = 2, Y = 0});
        List.Add(new PieceInfo() { Name = "W_Bishop2", Path = "Pieces/White_B", X = 5, Y = 0});
        List.Add(new PieceInfo() { Name = "W_Queen", Path = "Pieces/White_Q", X = 3, Y = 0});

        List.Add(new PieceInfo() { Name = "W_Pawn1", Path = "Pieces/White_P", X = 0, Y = 1});
        List.Add(new PieceInfo() { Name = "W_Pawn2", Path = "Pieces/White_P", X = 1, Y = 1});
        List.Add(new PieceInfo() { Name = "W_Pawn3", Path = "Pieces/White_P", X = 2, Y = 1});
        List.Add(new PieceInfo() { Name = "W_Pawn4", Path = "Pieces/White_P", X = 3, Y = 1});
        List.Add(new PieceInfo() { Name = "W_Pawn5", Path = "Pieces/White_P", X = 4, Y = 1});
        List.Add(new PieceInfo() { Name = "W_Pawn6", Path = "Pieces/White_P", X = 5, Y = 1});
        List.Add(new PieceInfo() { Name = "W_Pawn7", Path = "Pieces/White_P", X = 6, Y = 1});
        List.Add(new PieceInfo() { Name = "W_Pawn8", Path = "Pieces/White_P", X = 7, Y = 1});

        //black
        List.Add(new PieceInfo() { Name = "B_Rook1", Path = "Pieces/Black_R", X = 0, Y = 7});
        List.Add(new PieceInfo() { Name = "B_Rook2", Path = "Pieces/Black_R", X = 7, Y = 7});
        List.Add(new PieceInfo() { Name = "B_Knight1", Path = "Pieces/Black_N", X = 1, Y = 7});
        List.Add(new PieceInfo() { Name = "B_Knight2", Path = "Pieces/Black_N", X = 6, Y = 7});
        List.Add(new PieceInfo() { Name = "B_Bishop1", Path = "Pieces/Black_B", X = 2, Y = 7});
        List.Add(new PieceInfo() { Name = "B_Bishop2", Path = "Pieces/Black_B", X = 5, Y = 7});
        List.Add(new PieceInfo() { Name = "B_Queen", Path = "Pieces/Black_Q", X = 3, Y = 7});

        List.Add(new PieceInfo() { Name = "B_Pawn1", Path = "Pieces/Black_P", X = 0, Y = 6});
        List.Add(new PieceInfo() { Name = "B_Pawn2", Path = "Pieces/Black_P", X = 1, Y = 6});
        List.Add(new PieceInfo() { Name = "B_Pawn3", Path = "Pieces/Black_P", X = 2, Y = 6});
        List.Add(new PieceInfo() { Name = "B_Pawn4", Path = "Pieces/Black_P", X = 3, Y = 6});
        List.Add(new PieceInfo() { Name = "B_Pawn5", Path = "Pieces/Black_P", X = 4, Y = 6});
        List.Add(new PieceInfo() { Name = "B_Pawn6", Path = "Pieces/Black_P", X = 5, Y = 6});
        List.Add(new PieceInfo() { Name = "B_Pawn7", Path = "Pieces/Black_P", X = 6, Y = 6});
        List.Add(new PieceInfo() { Name = "B_Pawn8", Path = "Pieces/Black_P", X = 7, Y = 6});

        foreach (PieceInfo item in List)
        {
            GameObject chess_piece = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(item.Path));
            BasePiece p = chess_piece.GetComponent<BasePiece>();
            p.SetOriginalLocation(item.X, item.Y);
            chess_piece.transform.parent = this.transform;
            Pieces.Add(p);
        }
    }

    private void Awake()
    {
        Current = this;
        Init_ChessBoard();
        Init_ChessPieces();
    }
}
