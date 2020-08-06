using Assets.Script.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : BasePiece
{
    private bool can_do_castling = false;
    public override void Moving_rule()
    {
        #region King Location
        //0 +1
        c = new Clocation((int)Location.x, (int)Location.y + 1);
        if (c.Check_Location())
            list.Add(c);

        //0 -1
        c = new Clocation((int)Location.x, (int)Location.y - 1);
        if (c.Check_Location())
            list.Add(c);

        //+1 0
        c = new Clocation((int)Location.x + 1, (int)Location.y);
        if (c.Check_Location())
            list.Add(c);

        //-1 0
        c = new Clocation((int)Location.x - 1, (int)Location.y);
        if (c.Check_Location())
            list.Add(c);

        //+1 +1
        c = new Clocation((int)Location.x + 1, (int)Location.y + 1);
        if (c.Check_Location())
            list.Add(c);

        //+1 -1
        c = new Clocation((int)Location.x + 1, (int)Location.y - 1);
        if (c.Check_Location())
            list.Add(c);

        //-1 +1
        c = new Clocation((int)Location.x - 1, (int)Location.y + 1);
        if (c.Check_Location())
            list.Add(c);

        //-1 -1
        c = new Clocation((int)Location.x - 1, (int)Location.y - 1);
        if(c.Check_Location())
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
    }
    private void Castling_right()
    {
        if (ChessBoard.Current.cells[5][0].CurrentPiece == null && ChessBoard.Current.cells[6][0].CurrentPiece == null 
            && ChessBoard.Current.cells[7][0].CurrentPiece.Is_it_moved == false
            && ChessBoard.Current.cells[7][0].CurrentPiece.Player == this.Player)
            _canMovecells.Add(ChessBoard.Current.cells[6][0]);

        else if (ChessBoard.Current.cells[5][7].CurrentPiece == null && ChessBoard.Current.cells[6][7].CurrentPiece == null 
            && ChessBoard.Current.cells[7][7].CurrentPiece.Is_it_moved == false
            && ChessBoard.Current.cells[7][7].CurrentPiece.Player == this.Player)
            _canMovecells.Add(ChessBoard.Current.cells[6][7]);

        else
            return;
    }
    private void Castling_left()
    {
        if (ChessBoard.Current.cells[1][0].CurrentPiece == null && ChessBoard.Current.cells[2][0].CurrentPiece == null 
            && ChessBoard.Current.cells[3][0].CurrentPiece == null && ChessBoard.Current.cells[0][0].CurrentPiece.Is_it_moved == false 
            && ChessBoard.Current.cells[0][0].CurrentPiece.Player == this.Player)
            _canMovecells.Add(ChessBoard.Current.cells[2][0]);

        else if (ChessBoard.Current.cells[1][7].CurrentPiece == null && ChessBoard.Current.cells[2][7].CurrentPiece == null
            && ChessBoard.Current.cells[3][7].CurrentPiece == null && ChessBoard.Current.cells[0][7].CurrentPiece.Is_it_moved == false
            && ChessBoard.Current.cells[0][7].CurrentPiece.Player == this.Player)
            _canMovecells.Add(ChessBoard.Current.cells[2][7]);

        else
            return;
    }
    public bool isInCheck()
    {
        if(Player == Eplayer.BLACK)
        {
            foreach (BasePiece item in ChessBoard.Current.White_Pieces)
            {
                foreach (cell Cell in item.getTarget())
                    if (Cell == this.CurrentCell)
                        return true;
            }
        }
        else
        {
            foreach (BasePiece item in ChessBoard.Current.Black_Pieces)
            {
                foreach (cell Cell in item.getTarget())
                    if (Cell == this.CurrentCell)
                        return true;
            }
        }
        return false;
    }
    private void Awake()
    {
        value = 10000;
        type = Etype.KING;
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

