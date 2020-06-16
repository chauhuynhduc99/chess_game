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

    public cell[][] cells { get { return Cells; } }
    public float CELL_SIZE
    {
        get
        {
            if(Cell_size < 0)
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

        //castle
        GameObject _Castle = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Pieces/White_R"));
        _Castle.transform.parent = this.transform.GetChild(1);
        Castle castle = _Castle.GetComponent<Castle>();
        pieces.Add(castle);
        castle.SetOriginalLocation(0, 0);

        //knight
        GameObject _Knight = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Pieces/White_N"));
        _Knight.transform.parent = this.transform.GetChild(1);
        Knight knight = _Knight.GetComponent<Knight>();
        pieces.Add(knight);
        knight.SetOriginalLocation(1, 0);

    }
    private void Awake()
    {
        Current = this;
    }
    private void Update()
    {
        if(BaseGameCTL.Current.game_state == Egame_state.PLAYING)
        {
            CheckUserInput();
        }
    }

    private void CheckUserInput()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000, CellLayerMask.value))
        {
            //Debug.Log(hit.collider.name);
            cell newcell = hit.collider.GetComponent<cell>(); ;
            if (newcell != CurrentHoverCell)
            {
                if(CurrentHoverCell != null)
                    CurrentHoverCell.SetCellState(Ecell_state.NORMAL);
                CurrentHoverCell = newcell;
                CurrentHoverCell.SetCellState(Ecell_state.HOVER);
            }
            else
            {
                if (CurrentHoverCell != null)
                {
                    CurrentHoverCell.SetCellState(Ecell_state.NORMAL);
                    CurrentHoverCell = null;
                }
            }
        }
    }
}
