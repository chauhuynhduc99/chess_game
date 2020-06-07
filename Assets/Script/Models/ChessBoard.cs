using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessBoard : MonoBehaviour
{
    [ContextMenu("check")]
    public void check()
    {
        Init_ChessBoard();
    }

    private cell[][] Cells;
    public GameObject cellPrefap;
    public Vector3 base_Position = Vector3.zero;

    public cell[][] cells { get { return Cells; } }
    public Vector3 Calculate_Position(int i, int j)
    {
        float size = cellPrefap.GetComponent<cell>().size;
        return base_Position + new Vector3(i * size, j * size,0);
    }

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
    
    private void Update()
    {
        
    }

    private void CheckUserInput()
    {

    }
}
