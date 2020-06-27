using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessBoard : MonoBehaviour
{
    private cell[][] Cells;
    private cell CurrentHoverCell = null;
    private float Cell_size = -1;
    public static ChessBoard Current;
    private List<BasePiece> pieces;
    public GameObject cellPrefap;
    public Vector3 base_Position = Vector3.zero;
    public LayerMask CellLayerMask = 0;

    public cell[][] cells { get { return Cells; } set { Cells = value; } }
    public float CELL_SIZE
    {
        get
        {
            if (Cell_size < 0)
                Cell_size = cellPrefap.GetComponent<cell>().size;
            return Cell_size;
        }
    }
    
    public Vector3 Calculate_Position(int i, int j)
    {
        float size = cellPrefap.GetComponent<cell>().size;
        return base_Position + new Vector3(i * size, j * size,0);
    }

    [ContextMenu("Init_ChessBoard")]
    public void Init_ChessBoard()
    {
        Cells = new cell[8][];
        for(int i = 0; i < 8; i++)
        {
            Cells[i] = new cell[8];
            for (int j = 0; j < 8; j++)
            {
                GameObject c = GameObject.Instantiate(cellPrefap, Calculate_Position(i, j), Quaternion.identity)
                    as GameObject;

                Cells[i][j] = c.GetComponent<cell>();
                c.transform.parent = this.transform;
                
                if ((i + j) % 2 == 0)
                    Cells[i][j].color = Ecell_color.WHITE;
                else
                    Cells[i][j].color = Ecell_color.BLACK;
            }
        }
    }

    [ContextMenu("Init_ChessPieces")]
    public void Init_ChessPieces()
    {
        pieces = new List<BasePiece>();
        List<PieceInfo> List = new List<PieceInfo>();

        //white
        List.Add(new PieceInfo() { Name = "W_Rook1", Path = "Pieces/White_R", X = 0, Y = 0 });
        List.Add(new PieceInfo() { Name = "W_Rook2", Path = "Pieces/White_R", X = 7, Y = 0 });
        List.Add(new PieceInfo() { Name = "W_Knight1", Path = "Pieces/White_N", X = 1, Y = 0 });
        List.Add(new PieceInfo() { Name = "W_Knight2", Path = "Pieces/White_N", X = 6, Y = 0 });
        List.Add(new PieceInfo() { Name = "W_Bishop1", Path = "Pieces/White_B", X = 2, Y = 0 });
        List.Add(new PieceInfo() { Name = "W_Bishop2", Path = "Pieces/White_B", X = 5, Y = 0 });
        List.Add(new PieceInfo() { Name = "W_King", Path = "Pieces/White_K", X = 4, Y = 0 });
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
        List.Add(new PieceInfo() { Name = "B_King", Path = "Pieces/Black_K", X = 4, Y = 7 });
        List.Add(new PieceInfo() { Name = "B_Queen", Path = "Pieces/Black_Q", X = 3, Y = 7 });

        List.Add(new PieceInfo() { Name = "B_Pawn1", Path = "Pieces/Black_P", X = 0, Y = 6 });
        List.Add(new PieceInfo() { Name = "B_Pawn2", Path = "Pieces/Black_P", X = 1, Y = 6 });
        List.Add(new PieceInfo() { Name = "B_Pawn3", Path = "Pieces/Black_P", X = 2, Y = 6 });
        List.Add(new PieceInfo() { Name = "B_Pawn4", Path = "Pieces/Black_P", X = 3, Y = 6 });
        List.Add(new PieceInfo() { Name = "B_Pawn5", Path = "Pieces/Black_P", X = 4, Y = 6 });
        List.Add(new PieceInfo() { Name = "B_Pawn6", Path = "Pieces/Black_P", X = 5, Y = 6 });
        List.Add(new PieceInfo() { Name = "B_Pawn7", Path = "Pieces/Black_P", X = 6, Y = 6 });
        List.Add(new PieceInfo() { Name = "B_Pawn8", Path = "Pieces/Black_P", X = 7, Y = 6 });

        foreach (PieceInfo item in List)
        {
            GameObject chess_piece = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(item.Path));
            BasePiece p = chess_piece.GetComponent<BasePiece>();
            p.SetOriginalLocation(item.X, item.Y);
            pieces.Add(p);
            chess_piece.transform.parent = this.transform;
        }
    }
    private void Awake()
    {
        Current = this;
    }
    private void Start()
    {
        Init_ChessBoard();
        Init_ChessPieces();
    }
    private void Update()
    {
        Current = this;
    }
}
