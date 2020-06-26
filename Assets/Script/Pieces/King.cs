using Assets.Script.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : BasePiece
{
    public override void Move()
    {
        List<Clocation> list = new List<Clocation>();

        Clocation c;

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
}

