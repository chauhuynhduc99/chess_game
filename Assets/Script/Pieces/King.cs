using Assets.Script.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : BasePiece
{
    private bool can_do_castling = false;

    public override void Move()
    {
        List<Clocation> list = new List<Clocation>();
        Clocation c;

        #region King Location
        //0 +1
        c = new Clocation((int)Location.x, (int)Location.y + 1);
        if (c.X < 8 && c.X >= 0 && c.Y < 8 && c.Y >= 0)
            list.Add(c);

        //0 -1
        c = new Clocation((int)Location.x, (int)Location.y - 1);
        if (c.X < 8 && c.X >= 0 && c.Y < 8 && c.Y >= 0)
            list.Add(c);

        //+1 0
        c = new Clocation((int)Location.x + 1, (int)Location.y);
        if (c.X < 8 && c.X >= 0 && c.Y < 8 && c.Y >= 0)
            list.Add(c);

        //-1 0
        c = new Clocation((int)Location.x - 1, (int)Location.y);
        if (c.X < 8 && c.X >= 0 && c.Y < 8 && c.Y >= 0)
            list.Add(c);

        //+1 +1
        c = new Clocation((int)Location.x + 1, (int)Location.y + 1);
        if (c.X < 8 && c.X >= 0 && c.Y < 8 && c.Y >= 0)
            list.Add(c);

        //+1 -1
        c = new Clocation((int)Location.x + 1, (int)Location.y - 1);
        if (c.X < 8 && c.X >= 0 && c.Y < 8 && c.Y >= 0)
            list.Add(c);

        //-1 +1
        c = new Clocation((int)Location.x - 1, (int)Location.y + 1);
        if (c.X < 8 && c.X >= 0 && c.Y < 8 && c.Y >= 0)
            list.Add(c);

        //-1 -1
        c = new Clocation((int)Location.x - 1, (int)Location.y - 1);
        if(c.X < 8 && c.X >= 0 && c.Y < 8 && c.Y >= 0)
            list.Add(c);

        //Nhập thành
        if (Is_it_moved == false)
        {
            Castling_left();
            Castling_right();
            if (_canMovecells != null)
                can_do_castling = true;
        }
        else
            can_do_castling = false;
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

    private void Castling_right()
    {
        if (ChessBoard.Current.cells[5][0].CurrentPiece == null && ChessBoard.Current.cells[6][0].CurrentPiece == null 
            && ChessBoard.Current.cells[7][0].CurrentPiece.Is_it_moved == false)
            _canMovecells.Add(ChessBoard.Current.cells[6][0]);

        else if (ChessBoard.Current.cells[5][7].CurrentPiece == null && ChessBoard.Current.cells[6][7].CurrentPiece == null 
            && ChessBoard.Current.cells[7][7].CurrentPiece.Is_it_moved == false)
            _canMovecells.Add(ChessBoard.Current.cells[6][7]);

        else
            return;
    }
    private void Castling_left()
    {
        if (ChessBoard.Current.cells[1][0].CurrentPiece == null && ChessBoard.Current.cells[2][0].CurrentPiece == null 
            && ChessBoard.Current.cells[3][0].CurrentPiece == null && ChessBoard.Current.cells[0][0].CurrentPiece.Is_it_moved == false )
            _canMovecells.Add(ChessBoard.Current.cells[2][0]);

        else if (ChessBoard.Current.cells[1][7].CurrentPiece == null && ChessBoard.Current.cells[2][7].CurrentPiece == null
            && ChessBoard.Current.cells[3][7].CurrentPiece == null && ChessBoard.Current.cells[0][7].CurrentPiece.Is_it_moved == false)
            _canMovecells.Add(ChessBoard.Current.cells[2][7]);

        else
            return;
    }

    protected override void OnMouseUp()
    {
        base.OnMouseUp();
        if (can_do_castling == true)
        {
            if (_currentCell == ChessBoard.Current.cells[6][0] && Player == Eplayer.WHITE)
            {
                Castle p = ChessBoard.Current.cells[7][0].CurrentPiece as Castle;
                p.Castling(Player, false);
            }
            else if (_currentCell == ChessBoard.Current.cells[6][7] && Player == Eplayer.BLACK)
            {
                Castle p = ChessBoard.Current.cells[7][7].CurrentPiece as Castle;
                p.Castling(Player, false);
            }
            else if (_currentCell == ChessBoard.Current.cells[2][0] && Player == Eplayer.WHITE)
            {
                Castle p = ChessBoard.Current.cells[0][0].CurrentPiece as Castle;
                p.Castling(Player, true);
            }
            else if (_currentCell == ChessBoard.Current.cells[2][7] && Player == Eplayer.BLACK)
            {
                Castle p = ChessBoard.Current.cells[0][7].CurrentPiece as Castle;
                p.Castling(Player, true);
            }
            else
                can_do_castling = false;
        }
    }
}

