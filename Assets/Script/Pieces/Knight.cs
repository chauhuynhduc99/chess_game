using Assets.Script.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : BasePiece
{
    public override void Move()
    {
        List<Clocation> list = new List<Clocation>();
        Clocation c;

        #region Knight Location
        //+1 +2
        c = new Clocation((int)Location.x + 1, (int)Location.y + 2);
        if (c.Check_Location())
            list.Add(c);
        //+2 +1
        c = new Clocation((int)Location.x + 2, (int)Location.y + 1);
        if (c.Check_Location())
            list.Add(c);

        //+2 -1
        c = new Clocation((int)Location.x + 2, (int)Location.y - 1);
        if (c.Check_Location())
            list.Add(c);
        //+1 -2
        c = new Clocation((int)Location.x + 1, (int)Location.y - 2);
        if (c.Check_Location())
            list.Add(c);
        //-1 -2
        c = new Clocation((int)Location.x - 1, (int)Location.y - 2);
        if (c.Check_Location())
            list.Add(c);
        //-2 -1
        c = new Clocation((int)Location.x - 2, (int)Location.y - 1);
        if (c.Check_Location())
            list.Add(c);
        //-2 +1
        c = new Clocation((int)Location.x - 2, (int)Location.y + 1);
        if (c.Check_Location())
            list.Add(c);
        //-1 +2
        c = new Clocation((int)Location.x - 1, (int)Location.y + 2);
        if (c.Check_Location())
            list.Add(c);
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
}
