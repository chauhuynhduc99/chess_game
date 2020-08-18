using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessBoard : MonoBehaviour
{
    #region Field
    private cell[][] Cells;
    public int depth = 0;
    public static ChessBoard Current;
    public GameObject cellPrefap;
    public GameObject Chess_Pieces;
    public Vector3 base_Position = Vector3.zero;
    public LayerMask CellLayerMask = 0;
    private List<BasePiece> pieces = new List<BasePiece>();
    private List<BasePiece> black_Pieces = new List<BasePiece>();
    private List<BasePiece> white_Pieces = new List<BasePiece>();
    private King black_King;
    private King white_King;
    #endregion
    public List<BasePiece> All_piece { get { return pieces; } set { pieces = value; } }
    public List<BasePiece> Black_Pieces { get { return black_Pieces; } set { black_Pieces = value; } }
    public List<BasePiece> White_Pieces { get { return white_Pieces; } set { white_Pieces = value; } }
    public List<BasePiece> AI_Pieces
    {
        get
        {
            if (black_King.Side == Eside.AI)
                return Black_Active_Pieces();
            else
                return White_Active_Pieces();
        }
    }
    public List<BasePiece> HUMAN_Pieces
    {
        get
        {
            if (black_King.Side == Eside.HUMAN)
                return Black_Active_Pieces();
            else
                return White_Active_Pieces();
        }
    }
    public King Black_King { get { return black_King; } }
    public King White_King { get { return white_King; } }
    public King HUMAN_King
    {
        get
        {
            if (black_King.Side == Eside.HUMAN)
                return black_King;
            else
                return white_King;
        }
    }
    public King AI_King
    {
        get
        {
            if (black_King.Side == Eside.AI)
                return black_King;
            else
                return white_King;
        }
    }
    public cell[][] cells { get { return Cells; } set { Cells = value; } }
    public List<BasePiece> All_Active_Pieces()
    {
        List<BasePiece> All_pieces_active = new List<BasePiece>();
        foreach(BasePiece item in pieces)
        {
            if (item.Is_it_active)
                All_pieces_active.Add(item);
        }
        return All_pieces_active;
    }
    public List<BasePiece> White_Active_Pieces()
    {
        List<BasePiece> All_pieces_active = new List<BasePiece>();
        foreach (BasePiece item in white_Pieces)
        {
            if (item.Is_it_active)
                All_pieces_active.Add(item);
        }
        return All_pieces_active;
    }
    public List<BasePiece> Black_Active_Pieces()
    {
        List<BasePiece> All_pieces_active = new List<BasePiece>();
        foreach (BasePiece item in black_Pieces)
        {
            if (item.Is_it_active)
                All_pieces_active.Add(item);
        }
        return All_pieces_active;
    }
    public Vector3 Calculate_Position(int i, int j)
    {
        return base_Position + new Vector3(i, j, 0);
    }
    public void CHOOSE_WHITE()
    {
        Init_ChessBoard();
        Init_ChessPieces(Eplayer.WHITE);
        get_Side(Eplayer.WHITE);
        AI.Player = Eplayer.BLACK;
    }
    [ContextMenu("CHOOSE_WHITE")]
    public void CHOOSE_BLACK()
    {
        Init_ChessBoard();
        Init_ChessPieces(Eplayer.BLACK);
        get_Side(Eplayer.BLACK);
        AI.Player = Eplayer.WHITE;
    }
    [ContextMenu("CHOOSE_BLACK")]
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
    private void Init_ChessPieces(Eplayer player)
    {
        List<PieceInfo> List = new List<PieceInfo>();
        if (player == Eplayer.WHITE)
        {
            //King
            List.Add(new PieceInfo() { Name = "W_King", Path = "Pieces/White_K", X = 4, Y = 0 });
            List.Add(new PieceInfo() { Name = "B_King", Path = "Pieces/Black_K", X = 4, Y = 7 });

            //white
            List.Add(new PieceInfo() { Name = "W_Rook1", Path = "Pieces/White_R", X = 0, Y = 0 });
            List.Add(new PieceInfo() { Name = "W_Rook2", Path = "Pieces/White_R", X = 7, Y = 0 });
            List.Add(new PieceInfo() { Name = "W_Knight1", Path = "Pieces/White_N", X = 1, Y = 0 });
            List.Add(new PieceInfo() { Name = "W_Knight2", Path = "Pieces/White_N", X = 6, Y = 0 });
            List.Add(new PieceInfo() { Name = "W_Bishop1", Path = "Pieces/White_B", X = 2, Y = 0 });
            List.Add(new PieceInfo() { Name = "W_Bishop2", Path = "Pieces/White_B", X = 5, Y = 0 });
            List.Add(new PieceInfo() { Name = "W_Queen", Path = "Pieces/White_Q", X = 3, Y = 0 });

            List.Add(new PieceInfo() { Name = "W_Pawn1", Path = "Pieces/White_P", X = 0, Y = 1 });
            List.Add(new PieceInfo() { Name = "W_Pawn2", Path = "Pieces/White_P", X = 1, Y = 1 });
            List.Add(new PieceInfo() { Name = "W_Pawn3", Path = "Pieces/White_P", X = 2, Y = 1 });
            List.Add(new PieceInfo() { Name = "W_Pawn4", Path = "Pieces/White_P", X = 3, Y = 1 });
            List.Add(new PieceInfo() { Name = "W_Pawn5", Path = "Pieces/White_P", X = 4, Y = 1 });
            List.Add(new PieceInfo() { Name = "W_Pawn6", Path = "Pieces/White_P", X = 5, Y = 1 });
            List.Add(new PieceInfo() { Name = "W_Pawn7", Path = "Pieces/White_P", X = 6, Y = 1 });
            List.Add(new PieceInfo() { Name = "W_Pawn8", Path = "Pieces/White_P", X = 7, Y = 1 });

            //black
            List.Add(new PieceInfo() { Name = "B_Rook1", Path = "Pieces/Black_R", X = 0, Y = 7 });
            List.Add(new PieceInfo() { Name = "B_Rook2", Path = "Pieces/Black_R", X = 7, Y = 7 });
            List.Add(new PieceInfo() { Name = "B_Knight1", Path = "Pieces/Black_N", X = 1, Y = 7 });
            List.Add(new PieceInfo() { Name = "B_Knight2", Path = "Pieces/Black_N", X = 6, Y = 7 });
            List.Add(new PieceInfo() { Name = "B_Bishop1", Path = "Pieces/Black_B", X = 2, Y = 7 });
            List.Add(new PieceInfo() { Name = "B_Bishop2", Path = "Pieces/Black_B", X = 5, Y = 7 });
            List.Add(new PieceInfo() { Name = "B_Queen", Path = "Pieces/Black_Q", X = 3, Y = 7 });

            List.Add(new PieceInfo() { Name = "B_Pawn1", Path = "Pieces/Black_P", X = 0, Y = 6 });
            List.Add(new PieceInfo() { Name = "B_Pawn2", Path = "Pieces/Black_P", X = 1, Y = 6 });
            List.Add(new PieceInfo() { Name = "B_Pawn3", Path = "Pieces/Black_P", X = 2, Y = 6 });
            List.Add(new PieceInfo() { Name = "B_Pawn4", Path = "Pieces/Black_P", X = 3, Y = 6 });
            List.Add(new PieceInfo() { Name = "B_Pawn5", Path = "Pieces/Black_P", X = 4, Y = 6 });
            List.Add(new PieceInfo() { Name = "B_Pawn6", Path = "Pieces/Black_P", X = 5, Y = 6 });
            List.Add(new PieceInfo() { Name = "B_Pawn7", Path = "Pieces/Black_P", X = 6, Y = 6 });
            List.Add(new PieceInfo() { Name = "B_Pawn8", Path = "Pieces/Black_P", X = 7, Y = 6 });
        }
        else
        {
            //King
            List.Add(new PieceInfo() { Name = "W_King", Path = "Pieces/White_K", X = 4, Y = 7 });
            List.Add(new PieceInfo() { Name = "B_King", Path = "Pieces/Black_K", X = 4, Y = 0 });

            //white
            List.Add(new PieceInfo() { Name = "W_Rook1", Path = "Pieces/White_R", X = 0, Y = 7 });
            List.Add(new PieceInfo() { Name = "W_Rook2", Path = "Pieces/White_R", X = 7, Y = 7 });
            List.Add(new PieceInfo() { Name = "W_Knight1", Path = "Pieces/White_N", X = 1, Y = 7 });
            List.Add(new PieceInfo() { Name = "W_Knight2", Path = "Pieces/White_N", X = 6, Y = 7 });
            List.Add(new PieceInfo() { Name = "W_Bishop1", Path = "Pieces/White_B", X = 2, Y = 7 });
            List.Add(new PieceInfo() { Name = "W_Bishop2", Path = "Pieces/White_B", X = 5, Y = 7 });
            List.Add(new PieceInfo() { Name = "W_Queen", Path = "Pieces/White_Q", X = 3, Y = 7 });

            List.Add(new PieceInfo() { Name = "W_Pawn1", Path = "Pieces/White_P", X = 0, Y = 6 });
            List.Add(new PieceInfo() { Name = "W_Pawn2", Path = "Pieces/White_P", X = 1, Y = 6 });
            List.Add(new PieceInfo() { Name = "W_Pawn3", Path = "Pieces/White_P", X = 2, Y = 6 });
            List.Add(new PieceInfo() { Name = "W_Pawn4", Path = "Pieces/White_P", X = 3, Y = 6 });
            List.Add(new PieceInfo() { Name = "W_Pawn5", Path = "Pieces/White_P", X = 4, Y = 6 });
            List.Add(new PieceInfo() { Name = "W_Pawn6", Path = "Pieces/White_P", X = 5, Y = 6 });
            List.Add(new PieceInfo() { Name = "W_Pawn7", Path = "Pieces/White_P", X = 6, Y = 6 });
            List.Add(new PieceInfo() { Name = "W_Pawn8", Path = "Pieces/White_P", X = 7, Y = 6 });

            //black
            List.Add(new PieceInfo() { Name = "B_Rook1", Path = "Pieces/Black_R", X = 0, Y = 0 });
            List.Add(new PieceInfo() { Name = "B_Rook2", Path = "Pieces/Black_R", X = 7, Y = 0 });
            List.Add(new PieceInfo() { Name = "B_Knight1", Path = "Pieces/Black_N", X = 1, Y = 0 });
            List.Add(new PieceInfo() { Name = "B_Knight2", Path = "Pieces/Black_N", X = 6, Y = 0 });
            List.Add(new PieceInfo() { Name = "B_Bishop1", Path = "Pieces/Black_B", X = 2, Y = 0 });
            List.Add(new PieceInfo() { Name = "B_Bishop2", Path = "Pieces/Black_B", X = 5, Y = 0 });
            List.Add(new PieceInfo() { Name = "B_Queen", Path = "Pieces/Black_Q", X = 3, Y = 0 });

            List.Add(new PieceInfo() { Name = "B_Pawn1", Path = "Pieces/Black_P", X = 0, Y = 1 });
            List.Add(new PieceInfo() { Name = "B_Pawn2", Path = "Pieces/Black_P", X = 1, Y = 1 });
            List.Add(new PieceInfo() { Name = "B_Pawn3", Path = "Pieces/Black_P", X = 2, Y = 1 });
            List.Add(new PieceInfo() { Name = "B_Pawn4", Path = "Pieces/Black_P", X = 3, Y = 1 });
            List.Add(new PieceInfo() { Name = "B_Pawn5", Path = "Pieces/Black_P", X = 4, Y = 1 });
            List.Add(new PieceInfo() { Name = "B_Pawn6", Path = "Pieces/Black_P", X = 5, Y = 1 });
            List.Add(new PieceInfo() { Name = "B_Pawn7", Path = "Pieces/Black_P", X = 6, Y = 1 });
            List.Add(new PieceInfo() { Name = "B_Pawn8", Path = "Pieces/Black_P", X = 7, Y = 1 });
        }

        foreach (PieceInfo item in List)
        {
            GameObject chess_piece = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(item.Path));
            BasePiece p = chess_piece.GetComponent<BasePiece>();
            p.SetOriginalLocation(item.X, item.Y);
            chess_piece.transform.parent = Chess_Pieces.transform;
            pieces.Add(p);
        }
        foreach(BasePiece item in pieces)
        {
            if (item.Player == Eplayer.BLACK)
                black_Pieces.Add(item);
            else
                white_Pieces.Add(item);
            if (item.Player == Eplayer.BLACK && item.Type == Etype.KING)
                black_King = (King)item;
            else if (item.Player == Eplayer.WHITE && item.Type == Etype.KING)
                white_King = (King)item;
        }
    }
    private void get_Side(Eplayer player)
    {
        foreach (BasePiece item in pieces)
        {
            if (item.Player == player)
                item.Side = Eside.HUMAN;
            else
                item.Side = Eside.AI;
        }
    }
    public bool HUMAN_is_checkmated()
    {
        bool flag = false;
        foreach (BasePiece item in HUMAN_Pieces)
        {
            bool breaking = false;
            if (item.getLegalMoves().Count == 0)
                continue;
            else
            {
                cell old_cell = item.CurrentCell;
                foreach (cell move in item.getLegalMoves().ToArray())
                {
                    BasePiece piece = move.CurrentPiece;
                    item.Calculate_move(move);
                    if (ChessBoard.Current.HUMAN_King.isInCheck())
                    {
                        item.Return(old_cell, move, piece);
                        flag = true;
                    }
                    else
                    {
                        item.Return(old_cell, move, piece);
                        flag = false;
                        breaking = true;
                        break;
                    }
                }
                if (breaking)
                    break;
            }
        }
        return flag;
    }
    public bool AI_is_checkmated()
    {
        bool flag = false;
        foreach (BasePiece item in AI_Pieces)
        {
            bool breaking = false;
            if (item.getLegalMoves().Count == 0)
                continue;
            else
            {
                cell old_cell = item.CurrentCell;
                foreach (cell move in item.getLegalMoves().ToArray())
                {
                    BasePiece piece = move.CurrentPiece;
                    item.Calculate_move(move);
                    if (ChessBoard.Current.AI_King.isInCheck())
                    {
                        item.Return(old_cell, move, piece);
                        flag = true;
                    }
                    else
                    {
                        item.Return(old_cell, move, piece);
                        flag = false;
                        breaking = true;
                        break;
                    }
                }
                if (breaking)
                    break;
            }
        }
        return flag;
    }
    private void Awake()
    {
        Current = this;
    }
}
