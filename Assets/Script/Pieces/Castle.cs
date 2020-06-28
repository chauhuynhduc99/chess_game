using Assets.Script.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : BasePiece
{
    public override void Move()
    {
        List<Clocation> list = new List<Clocation>();
        Clocation c;

        #region Castle Location
        for (int i = 1; i < 8; i++)
        {
            c = new Clocation((int)Location.x, (int)Location.y + i);
            if (c.X < 8 && c.X >= 0 && c.Y < 8 && c.Y >= 0)
            {
                if (ChessBoard.Current.cells[c.X][c.Y].CurrentPiece == null)
                    list.Add(c);
                else
                {
                    list.Add(c);
                    break;
                }
            }
        }
        for (int i = 1; i < 8; i++)
        {
            c = new Clocation((int)Location.x + i, (int)Location.y);
            if (c.X < 8 && c.X >= 0 && c.Y < 8 && c.Y >= 0)
            {
                if (ChessBoard.Current.cells[c.X][c.Y].CurrentPiece == null)
                    list.Add(c);
                else
                {
                    list.Add(c);
                    break;
                }
            }
        }
        for (int i = 1; i < 8; i++)
        {
            c = new Clocation((int)Location.x - i, (int)Location.y);
            if (c.X < 8 && c.X >= 0 && c.Y < 8 && c.Y >= 0)
            {
                if (ChessBoard.Current.cells[c.X][c.Y].CurrentPiece == null)
                    list.Add(c);
                else
                {
                    list.Add(c);
                    break;
                }
            }
        }
        for (int i = 1; i < 8; i++)
        {
            c = new Clocation((int)Location.x, (int)Location.y - i);
            if (c.X < 8 && c.X >= 0 && c.Y < 8 && c.Y >= 0)
            {
                if (ChessBoard.Current.cells[c.X][c.Y].CurrentPiece == null)
                    list.Add(c);
                else
                {
                    list.Add(c);
                    break;
                }
            }
        }
        #endregion

        foreach (var item in list)
        {
            cell Cell = ChessBoard.Current.cells[item.X][item.Y];
            if (Cell.CurrentPiece == null)
                _canMovecells.Add(Cell);
            else if (Cell.CurrentPiece.Player != _player)
                _target.Add(Cell);
        }

        foreach (var item in _canMovecells)
            item.SetCellState(Ecell_state.HOVER);
        foreach (var item in _target)
            item.SetCellState(Ecell_state.TARGETED);
    }
    public void Castling(Eplayer player, bool is_left)
    {
        if(player == Eplayer.BLACK )
        {
            if (is_left == true)
            {
                _currentCell = ChessBoard.Current.cells[3][7];
                mousePos = _currentCell.transform.position;
                mousePos.z = -1;
            }
            else
            {
                _currentCell = ChessBoard.Current.cells[5][7];
                mousePos = _currentCell.transform.position;
                mousePos.z = -1;
            }
        }
        else
        {
            if (is_left == true)
            {
                _currentCell = ChessBoard.Current.cells[3][0];
                mousePos = _currentCell.transform.position;
                mousePos.z = -1;
            }
            else
            {
                _currentCell = ChessBoard.Current.cells[5][0];
                mousePos = _currentCell.transform.position;
                mousePos.z = -1;
            }
        }
    }
}
