using Assets.Script.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Pawn : BasePiece
{
    public override void Move()
    {
        List<Clocation> list = new List<Clocation>();
        Clocation c;

        #region Pawn Location
        if (this.Player == Eplayer.WHITE)
        {
            //0 +1
            c = new Clocation((int)Location.x, (int)Location.y + 1);
            if (c.X < 8 && c.X >= 0 && c.Y < 8 && c.Y >= 0)
                list.Add(c);
            //0 +2
            c = new Clocation((int)Location.x, (int)Location.y + 2);
            if (c.X < 8 && c.X >= 0 && c.Y < 8 && c.Y >= 0)
                list.Add(c);

            //+1 +1
            c = new Clocation((int)Location.x + 1, (int)Location.y + 1);
            if (c.X < 8 && c.X >= 0 && c.Y < 8 && c.Y >= 0)
                list.Add(c);
            //-1 +1
            c = new Clocation((int)Location.x - 1, (int)Location.y + 1);
            if (c.X < 8 && c.X >= 0 && c.Y < 8 && c.Y >= 0)
                list.Add(c);
        }
        else
        {
            //0 -1
            c = new Clocation((int)Location.x, (int)Location.y - 1);
            if (c.X < 8 && c.X >= 0 && c.Y < 8 && c.Y >= 0)
                list.Add(c);
            //0 -2
            c = new Clocation((int)Location.x, (int)Location.y - 2);
            if (c.X < 8 && c.X >= 0 && c.Y < 8 && c.Y >= 0)
                list.Add(c);

            //-1 -1
            c = new Clocation((int)Location.x - 1, (int)Location.y - 1);
            if (c.X < 8 && c.X >= 0 && c.Y < 8 && c.Y >= 0)
                list.Add(c);
            //+1 -1
            c = new Clocation((int)Location.x + 1, (int)Location.y - 1);
            if (c.X < 8 && c.X >= 0 && c.Y < 8 && c.Y >= 0)
                list.Add(c);
        }
        #endregion

        foreach (var item in list)
        {
            cell Cell = ChessBoard.Current.cells[item.X][item.Y];
            if (Cell.CurrentPiece == null)
            {
                if (Is_it_moved == false && item.X == Location.x)
                    _canMovecells.Add(Cell);
                else if(item.X == Location.x && ((item.Y - Location.y) == 1 || (item.Y - Location.y) == -1))
                    _canMovecells.Add(Cell);
            }
            else if (Cell.CurrentPiece.Player != _player && item.X != Location.x)
                _target.Add(Cell);
        }

        foreach (var item in _canMovecells)
            item.SetCellState(Ecell_state.HOVER);
        foreach (var item in _target)
            item.SetCellState(Ecell_state.TARGETED);
    }
    protected override void OnMouseUp()
    {
        base.OnMouseUp();
        if((Location.y == 7 && Player == Eplayer.WHITE)|| (Location.y == 0 && Player == Eplayer.BLACK))
        {
            pro_P.Current.Promotion(this);
            Destroy(gameObject);
        }
    }
}
